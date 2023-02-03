using System.ComponentModel.DataAnnotations;

namespace CoreWebApiDemo.Model
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public long MobileNo { get; set; }
        public DateTime Birthdate { get; set; }
        public string? Role { get; set; }

    }
}
