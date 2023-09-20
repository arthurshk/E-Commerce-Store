using Microsoft.AspNetCore.Identity;

namespace E_Commerce_Store.Models
{
    public class User : IdentityUser<int>
    {
        public User() : base()
        {

        }
    }
}
