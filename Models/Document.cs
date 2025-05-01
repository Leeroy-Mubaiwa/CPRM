using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPRM.Models;

[Table("documents")]
public partial class Document
{
    [Key]
    [Column("document_id")]
    public int DocumentId { get; set; }

    [Column("partner_id")]
    public int? PartnerId { get; set; }

    [Column("document_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string DocumentType { get; set; }

    [Column("file_name")]
    [StringLength(255)]
    [Unicode(false)]
    public string FileName { get; set; }

    [Column("file_url")]
    [StringLength(255)]
    [Unicode(false)]
    public string FileUrl { get; set; }

    [Column("uploaded_at", TypeName = "datetime")]
    public DateTime? UploadedAt { get; set; }

    [ForeignKey("PartnerId")]
    [InverseProperty("Documents")]
    public virtual Partner Partner { get; set; }
}
