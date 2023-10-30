using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string CatName { get; set; }
        [DisplayName("Category Display Order")]
        [Range(1,100,ErrorMessage ="range must be between 1 and 100")]
        [Required]
        public int DisplayOrder { get; set; }
    }
}
