using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace JezekT.NetStandard.Services.Extensions
{
    public static class ServiceErrorsProviderExtensions
    {
        public static void ResolveErrors(this IServiceErrorsProvider errorsProvider, Dictionary<string, string> validationDictionary, Action<string> exceptionMessageDelegate)
        {
            if (validationDictionary == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            if (errorsProvider.HasValidationError)
            {
                var errors = errorsProvider.GetValidationErrors();
                foreach (var error in errors)
                {
                    if (!validationDictionary.ContainsKey(error.Key))
                    {
                        validationDictionary.Add(error.Key, error.Value);
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorsProvider.ExceptionMessage))
            {
                exceptionMessageDelegate?.Invoke(errorsProvider.ExceptionMessage);
            }
        }

    }
}
