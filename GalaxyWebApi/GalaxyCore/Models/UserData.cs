using System;

namespace GalaxyCore.Models
{
    /// <summary>
    /// ������ ������ ������������.
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// ���.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ���� ��������.
        /// </summary>
        public DateTime BirthData { get; set; }

        /// <summary>
        /// ������.
        /// </summary>
        public double Amount { get; set; }
    }
}