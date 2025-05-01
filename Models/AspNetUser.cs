using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPRM.Models;

[Index("NormalizedEmail", Name = "EmailIndex")]
public partial class AspNetUser
{
    [Key]
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Column("IDDocumentNumber")]
    public string IddocumentNumber { get; set; }

    [Column("IDDocumentPath")]
    public string IddocumentPath { get; set; }

    public string PhotoPath { get; set; }

    public bool IsVerified { get; set; }

    public DateTime VerificationDate { get; set; }

    [StringLength(256)]
    public string UserName { get; set; }

    [StringLength(256)]
    public string NormalizedUserName { get; set; }

    [StringLength(256)]
    public string Email { get; set; }

    [StringLength(256)]
    public string NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    [Column("partner_id")]
    public int? PartnerId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    [InverseProperty("User")]
    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    [InverseProperty("User")]
    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    [InverseProperty("Receiver")]
    public virtual ICollection<Chat> ChatReceivers { get; set; } = new List<Chat>();

    [InverseProperty("Sender")]
    public virtual ICollection<Chat> ChatSenders { get; set; } = new List<Chat>();

    [InverseProperty("User")]
    public virtual ICollection<LoginLog> LoginLogs { get; set; } = new List<LoginLog>();

    [ForeignKey("PartnerId")]
    [InverseProperty("AspNetUsers")]
    public virtual Partner Partner { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
