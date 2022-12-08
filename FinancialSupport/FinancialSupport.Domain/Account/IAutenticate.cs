using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSupport.Domain.Account
{
    public interface IAutenticate
    {
        Task<bool> Authenticate(string email, string password);

        Task<bool> RegisterUser(string email, string password);

        Task Logout();
    }
}
