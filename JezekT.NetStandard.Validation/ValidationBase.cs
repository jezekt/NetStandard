using System.Collections.Generic;

namespace JezekT.NetStandard.Validation
{
    public abstract class ValidationBase<T> : IValidation<T> where T : class
    {
        protected IValidationDictionary ValidationDictionary;

        public bool HasValidationError => !ValidationDictionary.IsValid;

        public Dictionary<string, string> GetValidationErrors()
        {
            return ValidationDictionary?.GetErrors();
        }

        public abstract bool Validate(T objToValidate);
    }
}
