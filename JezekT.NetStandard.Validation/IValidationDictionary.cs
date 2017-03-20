using System.Collections.Generic;

namespace JezekT.NetStandard.Validation
{
    public interface IValidationDictionary
    {
        bool IsValid { get; }

        void AddError(string key, string errorMessage);
        Dictionary<string, string> GetErrors();
    }
}
