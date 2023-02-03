using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApiDemo.Model
{
    public class FeedbackModel
    {
        [Key]
        public int Id { get; set; }
        public string? Feedback { get; set; }
        public int? rate { get; set; }

        [ForeignKey(nameof(UserModel)), Column(Order = 0)]
        public int? LawyerId { get; set; }

        [ForeignKey(nameof(UserModel)), Column(Order = 1)]
        public int UserId { get; set; }

        public UserModel? User { get; set; }

    }
}
