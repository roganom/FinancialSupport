namespace FinancialSupport.Domain.Validation
{
    public class DomainExceptionValidation : Exception
    {
        public DomainExceptionValidation(string error) : base(error)
        {}

        public static void When (bool hanError, string error)
        {
            if (hanError)
                throw new DomainExceptionValidation(error); 
        }
    }
}
