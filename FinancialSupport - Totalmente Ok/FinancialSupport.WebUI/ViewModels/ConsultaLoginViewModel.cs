using FinancialSupport.WebUI.ViewModels.Shared;

namespace FinancialSupport.WebUI.ViewModels
{
    public class ConsultaLoginViewModel
    {
        public string? Role { get; set; }
        public string? User { get; set; }
        public bool Bloqueio { get; set; }
        public CustomMessagePartialViewModel? CustomMessagePartial { get; set; }
        public ConsultaLoginViewModel()
        {
            CustomMessagePartial = new CustomMessagePartialViewModel();
        }
    }
}
