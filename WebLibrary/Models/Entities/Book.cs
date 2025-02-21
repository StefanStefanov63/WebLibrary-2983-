using System.ComponentModel.DataAnnotations;

namespace WebLibrary.Models.Entities
{
    public class Book
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public int AuthorId { get; set; }
        //public virtual Author Author { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public Book() { }
    }
}
