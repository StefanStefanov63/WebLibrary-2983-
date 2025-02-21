using System.ComponentModel.DataAnnotations;

namespace WebLibrary.Models.Entities
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public bool IsReturned { get; set; }
        public int BookId { get; set; }
        //public virtual Book Book { get; set; }
        //public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
        public string UserId { get; set; }
        //public virtual User User { get; set; }
        //public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        public Log() { }
    }
}
