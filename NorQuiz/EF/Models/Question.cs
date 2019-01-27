using System.Collections.Generic;

namespace WebApplication1.EF.Models
{
    public class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public virtual int Id { get; set; }
        public virtual int QuizId { get; set; }
        public virtual string Value { get; set; }
        public virtual Quiz QuizNav { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}