namespace GalaxyRepository.Models
{
    public partial class Password
    {
        public int Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public int Userid { get; set; }

        public virtual User User { get; set; }
    }
}
