using FinancialSupport.Domain.Account;
using Microsoft.AspNetCore.Identity;

namespace FinancialSupport.Infra.Data.Identity
{
    public class AutenticateService : IAutenticate
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManger;
        public AutenticateService(SignInManager<ApplicationUser> signInManger, UserManager<ApplicationUser> userManager)
        {
            _signInManger = signInManger;
            _userManager = userManager;
        }
        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await _signInManger.PasswordSignInAsync(email,
                password, false, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManger.SignOutAsync();
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
            {
                await _signInManger.SignInAsync(applicationUser, isPersistent: false);
            }
            return result.Succeeded;
        }
    }
}
