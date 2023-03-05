namespace Inlamningsuppgift.Services
{
    public class ProfileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadProfileImageAsync(IFormFile profileImage) 
        {
            var profilePath = $"{_webHostEnvironment.WebRootPath}/images/profiles";
            var imageName = $"profile{Guid.NewGuid()}{Path.GetExtension(profileImage.FileName)}";
            string filePath = $"{profilePath}/{imageName}";
            using var fs = new FileStream(filePath, FileMode.Create);
            await profileImage.CopyToAsync(fs);
            return imageName;
        }
    }
}
