using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rahhal.Models.UserManagment;

namespace Rahhal.Models.PostManagement;

public class Post : BaseModel
{
    public string? Description { get; set; }

    [Required] public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))] public virtual User? User { get; set; }

    public List<PostMedia>? Media { get; set; }
}