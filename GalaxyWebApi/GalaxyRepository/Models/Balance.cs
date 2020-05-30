namespace GalaxyRepository.Models
{
    public partial class Balance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }

        public virtual User User { get; set; }
    }
}
