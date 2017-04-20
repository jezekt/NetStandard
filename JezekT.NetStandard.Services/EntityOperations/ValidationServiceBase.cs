using System.Collections.Generic;
using System.Linq;
using JezekT.NetStandard.Validation;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public abstract class ValidationServiceBase<TEntity>
        where TEntity : class
    {
        protected readonly IValidation<TEntity> Validation;
        public string ExceptionMessage { get; protected set; }
        public bool HasValidationError
        {
            get
            {
                if (Validation == null) return false;
                var errors = Validation.GetValidationErrors();
                return errors != null && Enumerable.Any<KeyValuePair<string, string>>(errors);
            }
        }


        public Dictionary<string, string> GetValidationErrors()
        {
            return Validation?.GetValidationErrors();
        }


        protected ValidationServiceBase(IValidation<TEntity> validation = null)
        {
            Validation = validation;
        }
    }
}
