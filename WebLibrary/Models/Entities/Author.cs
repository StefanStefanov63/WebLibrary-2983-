using System.ComponentModel.DataAnnotations;

namespace WebLibrary.Models.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
        public Author() { }
    }
}
