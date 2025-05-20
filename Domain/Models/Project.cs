namespace Domain.Models;

public class Project
{
    public string Id { get; set; } = null!;
    public string? ProjectImage { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? ClientName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }

    //public Client Client { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Status Status { get; set; } = null!;
}