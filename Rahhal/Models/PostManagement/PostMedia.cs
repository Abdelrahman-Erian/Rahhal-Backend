using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rahhal.Models.PostManagement;

public class PostMedia : BaseModel
{
    public string? Media_URL { get; set; }

    [Required] public Guid Post_Id { get; set; }
    [ForeignKey(nameof(Post_Id))] public Post Post { get; set; }
}