using System.ComponentModel.DataAnnotations;

namespace Email.API.Entities
{
    public class EmailLogger
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime EmailSent { get; set; }
    }
}
