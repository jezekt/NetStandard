using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace JezekT.NetStandard.Validation
{
    public class ValidationDictionary : IValidationDictionary
    {
        private readonly Dictionary<string, string> _validationDictionary;

        public bool IsValid => !_validationDictionary.Any();


        public void AddError(string key, string errorMessage)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _validationDictionary.Add(key, errorMessage);
        }

        public Dictionary<string, string> GetErrors()
        {
            return _validationDictionary;
        }


        public ValidationDictionary()
        {
            _validationDictionary = new Dictionary<string, string>();
        }

    }
}
