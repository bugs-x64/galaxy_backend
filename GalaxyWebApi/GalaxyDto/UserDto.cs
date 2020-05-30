using System;

namespace GalaxyDto
{
    /// <summary>
    /// DTO пользователя системы.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime BirthData { get; set; }

        /// <summary>
        /// Баланс.
        /// </summary>
        public double Amount { get; set; }
    }
}
