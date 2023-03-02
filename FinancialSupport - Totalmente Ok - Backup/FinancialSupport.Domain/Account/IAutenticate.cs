namespace FinancialSupport.Domain.Account
{
    public interface IAutenticate
    {
        Task<bool> Authenticate(string email, string password);
        Task<bool> RegisterUser(string email, string password, string role);
        Task<bool> TrocaSenhaUser(string email, string password, string newPassword);
        Task<List<ConsultaLogins>> ListaUsuarios();
        Task<bool> LockUser(string email);
        Task<bool> UnlockUser(string email);
        Task Logout();
        Task<bool> ResetPassword(string email, string newPassword);
        Task<bool> DeleteUser(string User);
    }
}
