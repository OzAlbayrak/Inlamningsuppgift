namespace Inlamningsuppgift.Services
{
    public class ProductService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadProductImageAsync(IFormFile productImageName)
        {
            var productPath = $"{_webHostEnvironment.WebRootPath}/images/products";
            var imageName = $"product{Guid.NewGuid()}{Path.GetExtension(productImageName.FileName)}";
            string filePath = $"{productPath}/{imageName}";
            using var fs = new FileStream(filePath, FileMode.Create);
            await productImageName.CopyToAsync(fs);
            return imageName;
        }
    }
}
