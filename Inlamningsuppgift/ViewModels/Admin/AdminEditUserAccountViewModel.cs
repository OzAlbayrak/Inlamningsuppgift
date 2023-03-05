using Inlamningsuppgift.Models.Forms;
using Inlamningsuppgift.Models.Identity;

namespace Inlamningsuppgift.ViewModels.Admin
{
    public class AdminEditUserAccountViewModel
    {
        public ICollection<UserAccount> Users { get; set; } = new List<UserAccount>();
    }
}
