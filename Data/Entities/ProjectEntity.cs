using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    //Project Data

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? ProjectImage { get; set; } 
    public string ProjectName { get; set; } = null!;
    public string? ClientName { get; set; }
    public string? Description { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; } 

    [Column(TypeName = "date")]
    public DateTime Created { get; set; } = DateTime.Now;


    //Foreign Keys

    //[ForeignKey(nameof(Client))]
    //public string ClientId { get; set; } = null!;
    //public ClientEntity Client { get; set; } = null!;


    [ForeignKey(nameof(User))]
    public string? UserId { get; set; } 
    public virtual UserEntity? User { get; set; } 

    [ForeignKey(nameof(Member))]
    public string? MemberId { get; set; } 
    public virtual MemberEntity? Member { get; set; } 


    [ForeignKey(nameof(Status))]
    public int? StatusId { get; set; }
    public virtual StatusEntity? Status { get; set; } 
}

