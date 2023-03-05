using Inlamningsuppgift.Contexts;
using Inlamningsuppgift.Models.Entities;
using Inlamningsuppgift.Services;
using Inlamningsuppgift.ViewModels.Admin;
using Inlamningsuppgift.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inlamningsuppgift.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _dataContext;

        public ProductsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Authorize(Roles = "Administrator, ProductManager")]
        public IActionResult AddProduct() 
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
