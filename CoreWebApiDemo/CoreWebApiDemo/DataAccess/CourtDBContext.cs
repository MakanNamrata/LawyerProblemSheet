using CoreWebApiDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace CoreWebApiDemo.DataAccess
{
    public class CourtDBContext : DbContext
    {
        public CourtDBContext(DbContextOptions<CourtDBContext> options) : base(options)
        {  }

        public DbSet<UserModel> userModels { get; set; }
        public DbSet<QuestionaryModel> questionaryModels { get; set; }
        public DbSet<AnswerModel> answerModels { get; set; }
        public DbSet<ConversationModel> conversationModels { get; set; }
        public DbSet<FeedbackModel> feedbackModels { get; set; }

    }
}
