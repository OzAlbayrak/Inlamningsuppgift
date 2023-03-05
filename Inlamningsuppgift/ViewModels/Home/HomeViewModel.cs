using Inlamningsuppgift.Models.Entities;


namespace Inlamningsuppgift.ViewModels.Home
{
    public class HomeViewModel
    {
        public ICollection<ProductEntity> Product { get; set; } = new List<ProductEntity>();
    }
}
