using Inlamningsuppgift.Contexts;
using Inlamningsuppgift.Models.Entities;
using Inlamningsuppgift.Models.Forms;
using Inlamningsuppgift.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inlamningsuppgift.Controllers
{
    //[Authorize(Roles = "ProductManager, Administrator")]
    public class ProductManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityContext _identityContext;
        private readonly UserService _userService;
        private readonly ProfileService _profileService;
        private readonly DataContext _dataContext;
        private readonly ProductService _productService;

        public ProductManagerController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IdentityContext identityContext, UserService userService, ProfileService profileService, DataContext dataContext, ProductService productService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityContext = identityContext;
            _userService = userService;
            _profileService = profileService;
            _dataContext = dataContext;
            _productService = productService;
        }

        //[AllowAnonymous]
        public async Task<IActionResult> Installation(string ReturnUrl = null!)
        {
            //if (await _userManager.Users.AnyAsync())
            //    return RedirectToAction("SignIn", "Account");

            if (await _userManager.Users.CountAsync() == 2)
                return RedirectToAction("SignIn", "Account");

            var form = new SignUpForm
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/")
            };

            return View(form);
        }

        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Installation(SignUpForm form)
        {
            if (ModelState.IsValid)
            {
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

                    await _userManager.AddToRoleAsync(identityUser, "ProductManager");

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

        [Authorize(Roles = "ProductManager, Administrator")]
        public async Task<IActionResult> AddProduct(string ReturnUrl = null!)
        {
            var form = new AddProductForm
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/")
            };
            return View(form);
        }
        [Authorize(Roles = "ProductManager, Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductForm form)
        {
            if (ModelState.IsValid)
            {
                var productEntity = new ProductEntity
                {
                    ArticleNumber = form.ArticleNumber,
                    Name = form.Name,
                    Price = form.Price,
                    Category = form.Category,
                    Rating = form.Rating,
                    ProductDescriptionShort = form.ProductDescriptionShort ?? null!,
                    ProductDescriptionLong = form.ProductDescriptionLong ?? null!,
                    ProductImageName = await _productService.UploadProductImageAsync(form.ProductImageName) ?? null!
                };
                _dataContext.Products.Add(productEntity);

                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Index", "Account");
            }

            ModelState.AddModelError(string.Empty, "Unable to add a Product.");
            return View(form);
        }
        [Authorize(Roles = "ProductManager")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
