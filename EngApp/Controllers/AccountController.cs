using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElphatehApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace StoreProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly  UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole>_roleManager;
       
        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser>signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(Registration registration)
        {
            if (ModelState.IsValid) {
                var user = new IdentityUser{ UserName = registration.UserName, Email = registration.UserName };
                var result=await  _userManager.CreateAsync(user,registration.Password);
                if (result.Succeeded)
                {
                    if (_userManager.Users.Count() <= 2 )
                    {
                        var roledata = await _roleManager.FindByNameAsync("Admin");
                        var userdata = await _userManager.FindByNameAsync(registration.UserName);
                        await _userManager.AddToRoleAsync(userdata, roledata.Name);
                    }
                    else if (_userManager.Users.Count() > 2 )
                    {
                        var roledata = await _roleManager.FindByNameAsync("User");
                        var userdata = await _userManager.FindByNameAsync(registration.UserName);
                        await _userManager.AddToRoleAsync(userdata, roledata.Name);
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index","Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registration);
        }
        [HttpGet][AllowAnonymous]
        public async Task<IActionResult> Login(String ReturnUrl)
        {
           TempData["url"] = ReturnUrl;
            if (_roleManager.Roles.Count() == 0)
            {
                var role1 = new IdentityRole { Name = "Admin" };
                await _roleManager.CreateAsync(role1);
                //----------------------------------------------
                var role2 = new IdentityRole { Name = "User" };
                await _roleManager.CreateAsync(role2);
            }
            if (_userManager.Users.Count() == 0)
            {
                var user = new IdentityUser { UserName = "Abdelaziz", Email = "Abdelaziz" };
                await _userManager.CreateAsync(user, "Koko_yasmin2020");
                var userdata = await _userManager.FindByNameAsync("Abdelaziz");
                var roledata = await _roleManager.FindByNameAsync("Admin");
                await _userManager.AddToRoleAsync(userdata, roledata.Name); 
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Login(Login login)
        {
            string ReturnUrl = null;
            if (TempData["url"]!=null)
                ReturnUrl = TempData["url"].ToString();
            if (ModelState.IsValid)
            {
              var result= await _signInManager.PasswordSignInAsync(login.UserName,login.Password,false,false);
                if (result.Succeeded)
                {
                    if (ReturnUrl != null&& Url.IsLocalUrl(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else
                        return Redirect("/");
                }
                    ModelState.AddModelError("","الاسم او الباسورد غير صحح");
            }
            return View(login);
        }
        public async Task<IActionResult> LogOut()
        {
           await _signInManager.SignOutAsync();
           return RedirectToAction(nameof(Login));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
         public IActionResult ListUser()
        {
            List<IdentityUser> users = new List<IdentityUser>();
            var data = _userManager.Users.OrderByOrdinal(x=>x.Email).ToArray();
            for(int i = 0; i < data.Count(); i++)
            {
                if (i == 0)
                    continue;
                users.Add(data[i]);
            }
            return View(users);
        }
        [ActionName("EditUsers"),HttpPost]
        public async Task<IActionResult> Edituser(String olduser,String UserName)
        {
            var user = await _userManager.FindByNameAsync(olduser);
            user.UserName = UserName;
            user.Email = UserName;
            user.NormalizedEmail = UserName.ToUpper();
            user.NormalizedUserName = UserName.ToUpper();
            await _userManager.UpdateAsync(user);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(String id)
        {
            var user =await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(ListUser));

            }
            ViewBag.error = "this id can't be fount";
            return View();
        }

    }
    
}
