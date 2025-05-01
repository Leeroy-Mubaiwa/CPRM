using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPRM.Models;

[Table("sales")]
public partial class Sale
{
    [Key]
    [Column("sale_id")]
    public int SaleId { get; set; }

    [Column("partner_id")]
    public int? PartnerId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("amount")]
    public double? Amount { get; set; }

    [Column("sale_date", TypeName = "datetime")]
    public DateTime? SaleDate { get; set; }

    [ForeignKey("PartnerId")]
    [InverseProperty("Sales")]
    public virtual Partner Partner { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Sales")]
    public virtual Product Product { get; set; }
}
