using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApiDemo.Model
{
    public class AnswerModel
    {
        [Key]
        public int Id { get; set; }
        public string Answer { get; set; }

        [ForeignKey(nameof(QuestionaryModel))]
        public int QuestionId { get; set; }

        [ForeignKey(nameof(UserModel))]
        public int LawyerId { get; set; }

        public QuestionaryModel? Questionary { get; set; }
        public UserModel? User { get; set; }
    }
}
