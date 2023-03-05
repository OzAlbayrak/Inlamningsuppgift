using Inlamningsuppgift.Contexts;
using Inlamningsuppgift.Models.Entities;
using Inlamningsuppgift.Models.Forms;
using Inlamningsuppgift.Models.Identity;
using Inlamningsuppgift.Services;
using Inlamningsuppgift.ViewModels.Account;
using Inlamningsuppgift.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Inlamningsuppgift.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityContext _identityContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserService _userService;
        private readonly ProfileService _profileService;
        private readonly DataContext _dataContext;
        private readonly ProductService _productService;


        public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IdentityContext identityContext, RoleManager<IdentityRole> roleManager, UserService userService, ProfileService profileService, DataContext dataContext, ProductService productService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityContext = identityContext;
            _roleManager = roleManager;
            _userService = userService;
            _profileService = profileService;
            _dataContext = dataContext;
            _productService = productService;
        }

        [AllowAnonymous]
        public async  Task<IActionResult> Installation(string ReturnUrl = null!)
        {
            if (await _userManager.Users.AnyAsync())
                return RedirectToAction("SignIn", "Account");

            var form = new SignUpForm
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/")
            };

            return View(form);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Installation(SignUpForm form)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.Roles.AnyAsync()) 
                {
                    await _roleManager.CreateAsync(new IdentityRole("Administrator"));
                    await _roleManager.CreateAsync(new IdentityRole("UserManager"));
                    await _roleManager.CreateAsync(new IdentityRole("ProductManager"));
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                
                var identityUser = new IdentityUser
                {
                    Email = form.Email,
                    UserName = form.Email
                };

                var result = await _userManager.CreateAsync(identityUser, form.Password);
                if (result.Succeeded)
                {
                    _identityContext.UserProfiles.Add(new UserProfileEntity
                    {
                        UserId = identityUser.Id,
                        FirstName = form.FirstName,
                        LastName = form.LastName,
                        StreetName = form.StreetName,
                        PostalCode = form.PostalCode,
                        City = form.City,
                        PhoneNumber = form.PhoneNumber ?? null!,
                        Company = form.Company ?? null!,
                        ImageName = await _profileService.UploadProfileImageAsync(form.ProfileImage) ?? null!
                    });
                    await _identityContext.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(identityUser, "Administrator");

                    var signInResult = await _signInManager.PasswordSignInAsync(identityUser, form.Password, false, false);
                    if (signInResult.Succeeded)
                        return LocalRedirect(form.ReturnUrl);
                    else
                        return RedirectToAction("SignIn", "Account");
                }
            }

            ModelState.AddModelError(string.Empty, "Unable to create an Account.");
            return View(form);
        }



        public async Task<IActionResult> UserManagement()
        {
            var viewModel = new AdminEditUserAccountViewModel(); 
            var appIdentityUsers = await _userManager.Users.ToListAsync();

            foreach (var user in appIdentityUsers)
            {
                var userAccount = await _userService.GetUserAccountAsync(user.UserName!);
                viewModel.Users.Add(userAccount);
            }
             return View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {

            var viewModel = new UserAccountEditViewModel();
            var identityUser = await _identityContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (identityUser != null)
            {
                var userProfileEntity = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityUser.Id);

                var identityProfile = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityUser.Id);
                if (userProfileEntity != null)
                {
                    viewModel.UserId = identityUser.Id;
                    viewModel.Form = new UserProfileEditForm
                    {
                        FirstName = userProfileEntity.FirstName,
                        LastName = userProfileEntity.LastName,
                        StreetName = userProfileEntity.StreetName,
                        PostalCode = userProfileEntity.PostalCode,
                        City = userProfileEntity.City
                    };
                }
            }
            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(UserAccountEditViewModel viewModel)
        {
            var userProfileEntity = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == viewModel.Id);

            if (userProfileEntity != null)
            {
                userProfileEntity.FirstName = viewModel.Form.FirstName;
                userProfileEntity.LastName = viewModel.Form.LastName;
                userProfileEntity.StreetName = viewModel.Form.StreetName;
                userProfileEntity.PostalCode = viewModel.Form.PostalCode;
                userProfileEntity.City = viewModel.Form.City;

                _identityContext.Update(userProfileEntity);
                await _identityContext.SaveChangesAsync();
            }

            return RedirectToAction("UserManagement");
        }
       

        public IActionResult Index() 
        {
            return View();
        }

    }
}
