using Business.Services;
using Business.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace FinalProject.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    [Route("auth/signup")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    [Route("auth/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var signUpFormData = model.MapTo<SignUpFormData>();
        var result = await _authService.SignUpAsync(signUpFormData);
        if (result.Succeeded)
        {
            return RedirectToAction("login", "Auth");
        }

        ViewBag.ErrorMessage = result.Error;
        return View();
    }

    [Route("auth/login")]
    public IActionResult Login(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [HttpPost]
    [Route("auth/login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        ViewBag.ErrorMessage = null;
        //ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var signInFormData = model.MapTo<SignInFormData>();
        var result = await _authService.SignInAsync(signInFormData);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Projects");
        }

        ViewBag.ErrorMessage = result.Error;
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.SignOutAsync();

        if (result.Succeeded)
        {
            return RedirectToAction("Login", "Auth"); // Or wherever you want to redirect
        }
        else
        {
            TempData["Error"] = "Logout failed.";
            return RedirectToAction("Index", "Home");
        }
    }
}



