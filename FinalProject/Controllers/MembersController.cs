using Business.DTOs;
using Business.Services;
using Domain.Extensions;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[Route("admin/members")]
public class MembersController(IMemberService memberService) : Controller
{
    private readonly IMemberService _memberService = memberService;



    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var result = await _memberService.GetAllAsync();
        var members = result.Succeeded && result.Result != null
            ? result.Result.Select(member =>
            {
                var mapped = member.MapTo<MemberViewModel>();
                mapped.Id = member.Id; // Force map the Id
                return mapped;
            }).ToList()
            : new List<MemberViewModel>();

        var viewModel = new MembersViewModel
        {
            Members = result.Succeeded && result.Result != null
                ? result.Result.Select(member => member.MapTo<MemberViewModel>()).ToList()
                : new List<MemberViewModel>(),
            AddMemberFormData = new AddMemberViewModel(),
            EditMemberFormData = new EditMemberViewModel()
        };

        if (!result.Succeeded)
            TempData["Error"] = result.Error;

        return View(viewModel);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new AddMemberViewModel());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(AddMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                              kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage))
                .ToArray();

            return BadRequest(new { success = false, errors });
        }

        string? imagePath = null;

        // Save image to wwwroot/uploads and get the relative path
        if (model.MemberImage != null && model.MemberImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.MemberImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.MemberImage.CopyToAsync(stream);
            }

            // Save the relative path so it can be used in <img src="">
            imagePath = $"/uploads/{uniqueFileName}";
        }
        else
        {
            imagePath = "/images/users/user-template-male-green.svg"; // Default image path if no image is uploaded
        }

        var formData = model.MapTo<AddMemberFormData>();
        formData.MemberImage = imagePath; // Set the saved image path


        // Handle other data for service

        var result = await _memberService.CreateAsync(formData);
        if (!result.Succeeded)
        {
            TempData["CreateErrors"] = new List<string> { result.Error! };
            TempData["ShowCreateModal"] = true;
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }



    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        var result = await _memberService.GetByIdAsync(id);
        if (!result.Succeeded || result.Result == null)
        {
            TempData["Error"] = result.Error ?? "Member not found.";
            return RedirectToAction("Index");
        }

        return View(result.Result);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EditMemberViewModel model)
    {
        if (id != model.Id)
        {
            ModelState.AddModelError(string.Empty, "Mismatched ID.");
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        string? imagePath = null;

        if (model.MemberImage != null && model.MemberImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.MemberImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.MemberImage.CopyToAsync(stream);
            }

            imagePath = $"/uploads/{uniqueFileName}";
        }
        else
        {
            var existing = await _memberService.GetByIdAsync(id); 
            if (existing.Succeeded && existing.Result != null)
            {
                imagePath = existing.Result.MemberImage;
            }
        }

        var formData = model.MapTo<AddMemberFormData>();
        formData.MemberImage = imagePath; 

        var result = await _memberService.UpdateAsync(id, formData);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }


    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _memberService.DeleteAsync(id);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
        }
        else
        {
            TempData["Success"] = "Member deleted.";
        }

        return RedirectToAction("Index");
    }



}
