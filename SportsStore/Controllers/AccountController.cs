using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
namespace SportsStore.Controllers {
    public class AccountController : Controller {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr) {
            userManager = userMgr;
            signInManager = signInMgr;
        }
        public ViewResult Login(string returnUrl) {
            return View(new LoginModel {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel) {
            if (ModelState.IsValid) {
                IdentityUser user =
                    await userManager.FindByNameAsync(loginModel.Name);
                if (user != null) {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user,
                        loginModel.Password, false, false)).Succeeded) {

                        if (IdentitySeedData.adminPassword== loginModel.Password)
                        {
                            return Redirect("/Account/ChangePassword");
                        }
                        else
                        {
                            return Redirect(loginModel?.ReturnUrl ?? "/Admin");
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong name or password");
                    }
                }
            }
            return View(loginModel);
        }
        [Authorize]
        public async Task<RedirectResult> Logout(string returnUrl = "/") {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        public ViewResult ChangePassword(string returnUrl) {
            return View(new ChangePswdModel {
                ReturnUrl = returnUrl
            });
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePswdModel changePswdModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync("admin");
                IdentityResult result=await userManager.ChangePasswordAsync(user,changePswdModel.OldPassword, changePswdModel.Password);
                if (result.Succeeded)
                {
                    return Redirect("/Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong old password");
                }
               
            }
            return View(changePswdModel);
        }
        
    }
}