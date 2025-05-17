namespace FinalProject.Models;

public class MembersViewModel
{
    public IEnumerable<MemberViewModel> Members { get; set; } = [];
    public AddMemberViewModel AddMemberFormData { get; set; } = new();
    public EditMemberViewModel EditMemberFormData { get; set; } = new();
}
