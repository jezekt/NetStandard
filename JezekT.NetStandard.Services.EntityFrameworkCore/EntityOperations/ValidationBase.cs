using System.Collections.Generic;
using System.Linq;
using JezekT.NetStandard.Validation;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.EntityOperations
{
    public abstract class ValidationBase<TEntity>
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
                return errors != null && errors.Any();
            }
        }


        public Dictionary<string, string> GetValidationErrors()
        {
            return Validation?.GetValidationErrors();
        }


        protected ValidationBase(IValidation<TEntity> validation = null)
        {
            Validation = validation;
        }
    }
}
