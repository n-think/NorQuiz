namespace WebApplication1.EF.Models
{
    public class Answer
    {
        public virtual int Id { get; set; }
        public virtual int QuestionId { get; set; }
        public virtual string Value { get; set; }
        public virtual bool Correct { get; set; }
        public virtual Question QuestionNav { get; set; }
    }
}