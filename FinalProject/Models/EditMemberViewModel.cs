using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Models;

public class EditMemberViewModel
{
    public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<SelectListItem> Members { get; set; } = [];
}
