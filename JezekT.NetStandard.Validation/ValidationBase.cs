using System.Collections.Generic;

namespace JezekT.NetStandard.Validation
{
    public abstract class ValidationBase<T> : IValidation<T> where T : class
    {
        protected readonly IValidationDictionary ValidationDictionary;

        public bool HasValidationError => !ValidationDictionary.IsValid;


        public Dictionary<string, string> GetValidationErrors()
        {
            return ValidationDictionary?.GetErrors();
        }


        public abstract bool Validate(T objToValidate);


        protected ValidationBase()
        {
            ValidationDictionary = new ValidationDictionary();
        }
    }
}
