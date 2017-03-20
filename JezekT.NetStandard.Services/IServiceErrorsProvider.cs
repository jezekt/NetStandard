using JezekT.NetStandard.Validation;

namespace JezekT.NetStandard.Services
{
    public interface IServiceErrorsProvider : IWithExceptionMessage, IProvideValidationErrors
    {
    }
}
