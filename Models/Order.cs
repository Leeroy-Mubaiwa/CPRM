using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPRM.Models;

[Table("orders")]
public partial class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("partner_id")]
    public int? PartnerId { get; set; }

    [Column("order_date", TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; }

    [Column("total_amount")]
    public double? TotalAmount { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("PartnerId")]
    [InverseProperty("Orders")]
    public virtual Partner Partner { get; set; }
}
