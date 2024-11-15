namespace studentDetails_Api.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningKey { get; set; }
        public string EncryptionKey { get; set; }
    }
}
