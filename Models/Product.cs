using System.ComponentModel.DataAnnotations;

namespace CrashProduto.API.Models
{
    public class Product
    {
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
