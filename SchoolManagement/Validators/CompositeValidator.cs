using System.Collections.Generic;

using SchoolManagement.Interfaces;

namespace SchoolManagement.Validators
{
    public class CompositeValidator : IValidator
    {
        private readonly List<IValidator> validators;

        public CompositeValidator() { }

        public CompositeValidator(List<IValidator> validators)
        { 
            Guard.ArgumentIsNotNull(validators, nameof(validators));

            this.validators = validators;
        }

        public bool Validate()
        {
            foreach(var validator in this.validators)
            {
                if (!validator.Validate())
                {
                    return false;
                };
            }

            return true;
        }

        public void AddValidator(IValidator validator)
        {
            this.validators.Add(validator);
        }
    }
}
