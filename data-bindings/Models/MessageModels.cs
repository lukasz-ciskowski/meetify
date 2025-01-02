using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class SendMessageModel
{
    [Required]
    [MaxLength(255)]
    public string MessageText { get; set; }
}