using FinancialSupport.Domain.Account;
using Microsoft.AspNetCore.Identity;

namespace FinancialSupport.Infra.Data.Identity
{
    public class AutenticateService : IAutenticate
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManger;
        public AutenticateService(SignInManager<ApplicationUser> signInManger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManger = signInManger;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<ConsultaLogins>> ListaUsuarios()
        {
            var retorno = new List<ConsultaLogins>();

            var roles = _roleManager.Roles.ToList();

            foreach (var role in roles)
            {
                var resultUser = await _signInManger.UserManager.GetUsersInRoleAsync(role.NormalizedName);
                foreach (var item in resultUser)
                {
                    bool bloqueio;

                    if (item.LockoutEnd > DateTime.Now)
                        bloqueio = true; else bloqueio = false;

                    if (item.AccessFailedCount != 999999) // COLOCAR ALGUM FILTRO PARA USUÁRIO "DELETADO"
                        retorno.Add (new ConsultaLogins { Role = role.NormalizedName, User = item.UserName, Bloqueio = bloqueio });
                };
            }

            return (retorno);
        }
        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await _signInManger.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            return result.Succeeded;
        }
        public async Task<bool> RegisterUser(string email, string password, string role)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
            {
                if (role != "ADMIN")
                    role = "USER";

                await _userManager.AddToRoleAsync(applicationUser, role);
            }
            return result.Succeeded;
        }
        public async Task<bool> TrocaSenhaUser(string email, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                if (result.Succeeded)
                {
                    return result.Succeeded;
                }
                else
                    return false;
            }
            return false;
        }
        public async Task<bool> LockUser(string user)
        {
            var applicationUser = await _userManager.FindByNameAsync(user);

            if (applicationUser != null)
            {
                applicationUser.LockoutEnabled = true;
                applicationUser.LockoutEnd = DateTime.Now.AddYears(100);

                await _userManager.UpdateAsync(applicationUser);

                return true;
            }
            else
                return false;
        }
        public async Task<bool> UnlockUser(string User)
        {
            var applicationUser = await _userManager.FindByNameAsync(User);

            if (applicationUser != null)
            {
                applicationUser.LockoutEnabled = true;
                applicationUser.LockoutEnd = DateTime.Now;

                await _userManager.UpdateAsync(applicationUser);

                return true;
            }
            else
                return false;
        }
        public async Task<bool> ResetPassword(string user, string newPassword)
        {
            var applicationUser = await _userManager.FindByNameAsync(user);

            if (applicationUser != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

                await _userManager.ResetPasswordAsync(applicationUser, token, newPassword);

                return true;
            }
            else
                return false;
        }
        public async Task<bool> DeleteUser(string User)
        {
            var applicationUser = await _userManager.FindByNameAsync(User);

            if (applicationUser != null)
            {
                applicationUser.AccessFailedCount = 999999; // Neste caso o usuário está "deletado'

                await _userManager.UpdateAsync(applicationUser);

                return true;
            }
            else
                return false;
        }
        public async Task Logout()
        {
            await _signInManger.SignOutAsync();
        }
    }
}
