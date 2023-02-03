using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApiDemo.Model
{ 
    public class ConversationModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(QuestionaryModel))]
        public int QuestionId { get; set; }

        [ForeignKey(nameof(AnswerModel))]
        public int AnswerId { get; set; }


        public QuestionaryModel? Questionary { get; set; }
        public AnswerModel? Answer { get; set; }


    }
}
