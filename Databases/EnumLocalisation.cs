using APPZ.Enums;

namespace APPZ.Databases;

public static class EnumLocalisation
{
    public static string Get(UserPublicProps publicProp) => publicProp switch
    {
        UserPublicProps.Name => "Ім'я",
        UserPublicProps.Surname => "Прізвище",
        UserPublicProps.Age => "Вік",
        UserPublicProps.Number => "Номер",
        _ => publicProp.ToString()
    };
    
    public static string Get(UserPrivateProps privateProp) => privateProp switch
    {
        UserPrivateProps.Login => "Логін",
        UserPrivateProps.Password => "Пароль",
        _ => privateProp.ToString()
    };

    public static string Get(UserPosts userPosts) => userPosts switch
    {
        UserPosts.Accountant => "Бухгалтер",
        UserPosts.Administrator => "Адміністратор",
        UserPosts.Fop => "ФОП",
        UserPosts.HumanRes => "Відділ кадрів",
        _ => userPosts.ToString()
    };
    
    public static string Get(UserFunctions func) => func switch
    {
        UserFunctions.InfoShow => "Переглянути інформацію про користувача",
        UserFunctions.ChangeRequest => "Внести поправки у запит",
        UserFunctions.ApproveRequest => "Затвердити рішення",
        UserFunctions.InfoBudget => "Переглянути інформацію про бюджет",
        UserFunctions.InfoPersonnel => "Переглянути інформацію про персонал",
        UserFunctions.SendBudget => "Відправити бюджет на затвердження",
        UserFunctions.SendPersonnel => "Відправити персонал на затвердження",
        UserFunctions.AddUser => "Додати користувача",
        UserFunctions.Settings => "Налаштування аккаунту",
        _ => func.ToString()
    };
    
    public static string Get(RequestProps prop) => prop switch
    {
        RequestProps.ReviewAuthor => "Запит перевірив",
        
        RequestProps.BudgetType => "Тип бюджету",
        RequestProps.Value => "Бюджет",
        RequestProps.Description => "Ціль",
        
        RequestProps.PersonInfo => "Інформація про особу",
        RequestProps.DesiredPosts => "Бажані посади",
        RequestProps.IsAccountNeeded => "Аккаунт потрібен?",
        
        _ => prop.ToString()
    };
    
    public static string Get(RequestBudgetType type) => type switch
    {
        RequestBudgetType.Expenses => "Витрата",
        RequestBudgetType.Income => "Прибуток",
        _ => type.ToString()
    };
    
    public static string Get(YesNo type) => type switch
    {
        YesNo.No => "Ні",
        YesNo.Yes => "Так",
        _ => type.ToString()
    };
    
    public static string Get(RequestType type) => type switch
    {
        RequestType.Budget => "Бюджет",
        RequestType.Personnel => "Персонал",
        _ => type.ToString()
    };
}