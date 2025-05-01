using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPRM.Models;

[Table("partners")]
[Index("Email", Name = "UQ__partners__AB6E6164B08F9C53", IsUnique = true)]
public partial class Partner
{
    [Key]
    [Column("partner_id")]
    public int PartnerId { get; set; }

    [Column("full_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string FullName { get; set; }

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; }

    [Column("phone_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string PhoneNumber { get; set; }

    [Column("national_id")]
    [StringLength(40)]
    [Unicode(false)]
    public string NationalId { get; set; }

    [Column("id_photo_url")]
    [StringLength(255)]
    [Unicode(false)]
    public string IdPhotoUrl { get; set; }

    [Column("live_photo_url")]
    [StringLength(255)]
    [Unicode(false)]
    public string LivePhotoUrl { get; set; }

    [Column("is_verified")]
    public bool? IsVerified { get; set; }

    [Column("registration_date", TypeName = "datetime")]
    public DateTime? RegistrationDate { get; set; }

    [InverseProperty("Partner")]
    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();

    [InverseProperty("Partner")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [InverseProperty("Partner")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Partner")]
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
