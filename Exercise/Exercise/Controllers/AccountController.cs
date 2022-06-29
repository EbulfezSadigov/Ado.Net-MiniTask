using Exercise.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exercise.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }

        public async Task<IActionResult> Register()
        {
            IdentityUser newUser = new IdentityUser()
            {
                Email = "ebulfez@code.edu.az",
                UserName = "ebulfez"
            };

            IdentityResult result = await userManager.CreateAsync(newUser, "Ebulfez123@");

            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return Content("Not Okay");
            }

            return Content("Okay");
        }

        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            IdentityUser loggingUser = await userManager.FindByEmailAsync(viewModel.Email);

            if (loggingUser == null)
            {
                ModelState.AddModelError("", "Email or password is wrong.");
                return View(viewModel);
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(loggingUser, viewModel.Password, viewModel.StayLoggedIn, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "You are locked out. Please, try again after 30 minutes.");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is wrong.");
                }
                return View(viewModel);
            }

            return RedirectToAction("Index", new { area = "Admin" });
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
