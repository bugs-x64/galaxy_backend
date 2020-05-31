using System;

namespace GalaxyDto
{
    /// <summary>
    /// DTO пользователя системы.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Баланс.
        /// </summary>
        public decimal Amount { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
