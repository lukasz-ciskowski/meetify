using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class CreateRoomModel
{
    [Required]
    [MaxLength(30)]
    public string Title { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; }
    
    [Required]
    public RoomVisibilities Visibility { get; set; }
}