namespace GalaxyDto
{
    public class TokenDto
    {
        public string username { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public double expires_in { get; set; }
    }
}