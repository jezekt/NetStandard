using System.Collections.Generic;

namespace JezekT.NetStandard.Validation
{
    public interface IProvideValidationErrors
    {
        bool HasValidationError { get; }

        Dictionary<string, string> GetValidationErrors();
    }
}
