using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.DataModels.Entities
{
    public class ProductEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }


        [ForeignKey(nameof(ProductTypeId))]
        public ProductTypeEntity ProductType { get; set; } = null!;
        public int ProductTypeId { get; set; }


        [ForeignKey(nameof(ProductBrandId))]
        public ProductBrandEntity ProductBrand { get; set; } = null!;
        public int ProductBrandId { get; set; }

    }
}
