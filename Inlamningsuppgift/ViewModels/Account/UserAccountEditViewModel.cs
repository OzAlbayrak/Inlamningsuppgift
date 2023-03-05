using Inlamningsuppgift.Models.Forms;
using Inlamningsuppgift.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Inlamningsuppgift.ViewModels.Account
{
    public class UserAccountEditViewModel : IdentityUser
    {
        public string UserId { get; set; } = null!;
        public UserProfileEditForm Form { get; set; } = new UserProfileEditForm();
    }
}
