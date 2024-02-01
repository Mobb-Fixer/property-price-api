using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace property_price_purchase_service.Models;

public class Order
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Reference { get; set; }
    
    [ForeignKey("ProductFK")]
    public Product Product { get; set; }
}