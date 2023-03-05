using Inlamningsuppgift.Contexts;
using Inlamningsuppgift.Models.Entities;
using Inlamningsuppgift.Models.Forms;
using Inlamningsuppgift.Services;
using Inlamningsuppgift.ViewModels.Account;
using Inlamningsuppgift.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inlamningsuppgift.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityContext _identityContext;
        private readonly UserService _userService;
        private readonly ProfileService _profileService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IdentityContext identityContext, UserService userService, ProfileService profileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityContext = identityContext;
            _userService = userService;
            _profileService = profileService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignUp(string ReturnUrl = null!)
        {
            if (!await _userManager.Users.AnyAsync())
                return RedirectToAction("Installation", "Admin");
            else if (await _userManager.Users.CountAsync() == 1)
                return RedirectToAction("Installation", "ProductManager");
            else
            {
                var form = new SignUpForm
                {
                    ReturnUrl = ReturnUrl ?? Url.Content("~/")
                };
                return View(form);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpForm form)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.Users.AnyAsync(x => x.Email == form.Email))
                {
                    ModelState.AddModelError(string.Empty, "A user with tha same email already exists.");
                    return View(form);
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

                    await _userManager.AddToRoleAsync(identityUser, "User");

                    var signInResult = await _signInManager.PasswordSignInAsync(identityUser, form.Password, false, false);
                    if (signInResult.Succeeded)
                        return LocalRedirect(form.ReturnUrl);
                    else
                        return RedirectToAction("SignIn");
                }
            }

            ModelState.AddModelError(string.Empty, "Unable to create an Account.");
            return View(form);
        }



        [AllowAnonymous]
        public IActionResult SignIn(string ReturnUrl = null!)
        {
            var form = new SignInForm
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/")
            };
            return View(form);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInForm form)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);
                if (result.Succeeded)
                    return LocalRedirect(form.ReturnUrl);
            }

            ModelState.AddModelError(string.Empty, "Incorrect Email ore Password");
            return View(form);
        }

        public async Task<IActionResult> SignOut()
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            return LocalRedirect("/");
        }

        public async Task<IActionResult> Edit()
        {
            var viewModel = new UserAccountEditViewModel();
            var userAccount = await _userService.GetUserAccountAsync(User.Identity!.Name!);

            if (userAccount != null)
            {
                viewModel.UserId = userAccount.Id;
                viewModel.Form = new UserProfileEditForm
                {
                    FirstName = userAccount.FirstName,
                    LastName = userAccount.LastName,
                    StreetName = userAccount.StreetName,
                    PostalCode = userAccount.PostalCode,
                    City = userAccount.City
                };
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserAccountEditViewModel viewModel)
        {
            var identityUser = await _identityContext.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name!);
            var userProfileEntity = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityUser.Id);

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

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel();
            viewModel.UserAccount = await _userService.GetUserAccountAsync(User.Identity!.Name!);
            return View(viewModel);
        }
    }
}
