using System.Collections.Generic;
using System.Windows.Controls;
using APPZ.Enums;

namespace APPZ.Decorator.Users;

public interface IUser
{
    /// <summary>
    /// Метод рахування кількості копії певних "обгорток"-функцій. Потрібнен для запобігання повторних функцій при
    /// змішанні певних посад.
    /// </summary>
    /// <returns>Кількість копій-"обгорток".</returns>
    int GetCount();
    
    /// <summary>
    /// Метод повного перезапуску користувача.
    /// </summary>
    void Reset();
    
    /// <summary>
    /// Метод отримання посад, які має користувач.
    /// </summary>
    /// <returns>Список посад, які посідає користувач.</returns>
    List<UserPosts> GetPosts();

    /// <summary>
    /// Метод отримання кнопок, над якими можна здійснювати редагування поза декоратором.
    /// </summary>
    /// <returns>Словник (<see cref="UserFunctions">Функція</see>, за яку відповідає кнопка; Кнопка).</returns>
    Dictionary<UserFunctions, Button> GetEditableButtons();

    /// <summary>
    /// Метод отримання ID користувача.
    /// </summary>
    /// <returns>ID користувача.</returns>
    int GetId();
    
    /// <summary>
    /// Метод створення кнопок з відповідними функціями.
    /// </summary>
    /// <param name="grid">Сітка, на яку потрібно розмістити кнопку.</param>
    void CreateButton(Grid grid);

    /// <summary>
    /// Метод отримання <b>відкритої</b> інформації про користувача.
    /// </summary>
    /// <returns>Словник (<see cref="UserPublicProps">Поле</see>, яке містить клас Користувач;
    /// Інформація про користувача).</returns>
    Dictionary<UserPublicProps, string> GetProperties();

    /// <summary>
    /// Метод прирівння двох користувачів.
    /// </summary>
    /// <param name="obj">Користувач, з яким порівнюється користувач, який викликав метод.</param>
    /// <returns>Чи однакові користувачі.</returns>
    bool Equals(IUser obj);
}
