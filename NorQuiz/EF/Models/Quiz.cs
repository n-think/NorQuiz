using System.Collections.Generic;

namespace WebApplication1.EF.Models
{
    public class Quiz
    {
        public Quiz()
        {
            Questions = new HashSet<Question>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}