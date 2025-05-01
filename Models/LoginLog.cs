using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPRM.Models;

[Table("login_logs")]
public partial class LoginLog
{
    [Key]
    [Column("log_id")]
    public int LogId { get; set; }

    [Column("user_id")]
    [StringLength(450)]
    public string UserId { get; set; }

    [Column("login_time", TypeName = "datetime")]
    public DateTime? LoginTime { get; set; }

    [Column("ip_address")]
    [StringLength(50)]
    [Unicode(false)]
    public string IpAddress { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("LoginLogs")]
    public virtual AspNetUser User { get; set; }
}
