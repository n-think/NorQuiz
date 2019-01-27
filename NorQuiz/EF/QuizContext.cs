using Microsoft.EntityFrameworkCore;
using WebApplication1.EF.Models;

namespace WebApplication1.EF
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions options):base(options)
        {
        }
        
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>(e =>
            {
                e.HasKey(x => x.Id);
            });
            
            modelBuilder.Entity<Question>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.QuizNav)
                    .WithMany(x => x.Questions)
                    .HasForeignKey(x=>x.QuizId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<Answer>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.QuestionNav)
                    .WithMany(x => x.Answers)
                    .HasForeignKey(x=>x.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}