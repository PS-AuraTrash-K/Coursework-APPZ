
using System.Collections.Generic;
using System.Linq;
using APPZ.Databases;
using APPZ.Decorator.Users;
using APPZ.Enums;

namespace APPZ.Decorator.Requests;

public class RequestPersonnel: Request
{
    private readonly Dictionary<UserPublicProps, string> _personInfo;
    private readonly List<string> _desiredPosts;
    private readonly YesNo _isAccountNeeded;

    public RequestPersonnel(Dictionary<UserPublicProps, string> personInfo, List<string> desiredPosts,
        bool isAccountNeeded, IUser author)
        : base(author)
    {
        _desiredPosts = desiredPosts;
        _isAccountNeeded = isAccountNeeded ? YesNo.Yes : YesNo.No;
        _personInfo = personInfo;
    }
    
    public RequestPersonnel(Dictionary<string, string> dictionary, IUser author)
        : base(author)
    {
        _desiredPosts = dictionary["DesiredPosts"].Split(' ').ToList();
        _isAccountNeeded = dictionary["IsAccountNeeded"] == "Так" ? YesNo.Yes : YesNo.No;
        _personInfo = StringCoding.DecodeToDictionary(dictionary["PersonInfo"]);
    }

    public override Dictionary<RequestProps, string> GetProperties() => new()
    {
        {RequestProps.PersonInfo, StringCoding.Encode(_personInfo)},
        {RequestProps.DesiredPosts, StringCoding.Encode(_desiredPosts)},
        {RequestProps.IsAccountNeeded, EnumLocalisation.Get(_isAccountNeeded)},
    };

    public static List<RequestProps> GetRawProperties() => new()
    {
        RequestProps.PersonInfo,
        RequestProps.DesiredPosts,
        RequestProps.IsAccountNeeded,
    };

    public override RequestType GetRequestType() => RequestType.Personnel;

    public override string ToString()
    {
        return $"-- Запит №{Id} --\n" +
               $"Прізвище: {_personInfo[UserPublicProps.Surname]}\n" +
               $"Ім'я: {_personInfo[UserPublicProps.Name]}\n" +
               $"Вік: {_personInfo[UserPublicProps.Age]}\n" +
               $"Номер телефону: {_personInfo[UserPublicProps.Number]}\n" +
               $"--------------\n" +
               $"Бажані посади:\n{string.Join("\n", StringCoding.DecodeToList(GetProperties()[RequestProps.DesiredPosts]))}" +
               $"--------------\n" +
               $"Чи потрібен аккаунт: {GetProperties()[RequestProps.IsAccountNeeded]}\n" +
               $"Автор запиту: {GetAuthorProperties()[UserPublicProps.Surname] + " " +
                                GetAuthorProperties()[UserPublicProps.Name]}";
    }
}