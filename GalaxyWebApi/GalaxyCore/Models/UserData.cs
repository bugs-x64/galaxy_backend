using System;

namespace GalaxyCore.Models
{
    /// <summary>
    /// Модель данных пользователя.
    /// </summary>
    public class UserData
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