namespace Domain.Entities.Models
{
    public class User
    {
        public string Document {  get; set; }
        public string Name { get; set; }
        public DateOnly DateBirth { get; set; }
    }
}
