using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApiDemo.Model
{
    public class QuestionaryModel
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
        public string Media { get; set; }
        public bool isPicked { get; set; }

        [ForeignKey(nameof(UserModel)), Column(Order = 0)]
        public int? LawyerId { get; set; }

        [ForeignKey(nameof(UserModel)), Column(Order = 1)]
        public int UserId { get; set; }

        public UserModel? User { get; set; }
    }
}
