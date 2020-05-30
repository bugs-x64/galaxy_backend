using System;

namespace GalaxyDto
{
    /// <summary>
    /// DTO нового пользователя.
    /// </summary>
    public class NewUserDto
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Имя пользователя(логин).
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}