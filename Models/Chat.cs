using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPRM.Models;

[Table("chats")]
public partial class Chat
{
    [Key]
    [Column("chat_id")]
    public int ChatId { get; set; }

    [Column("sender_id")]
    [StringLength(450)]
    public string SenderId { get; set; }

    [Column("receiver_id")]
    [StringLength(450)]
    public string ReceiverId { get; set; }

    [Column("message", TypeName = "text")]
    public string Message { get; set; }

    [Column("sent_at", TypeName = "datetime")]
    public DateTime? SentAt { get; set; }

    [ForeignKey("ReceiverId")]
    [InverseProperty("ChatReceivers")]
    public virtual AspNetUser Receiver { get; set; }

    [ForeignKey("SenderId")]
    [InverseProperty("ChatSenders")]
    public virtual AspNetUser Sender { get; set; }
}
