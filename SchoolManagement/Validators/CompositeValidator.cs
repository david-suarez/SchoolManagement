using System.Linq;

using SchoolManagement.Interfaces;

namespace SchoolManagement.Validators
{
    public class CompositeValidator<T> : IValidator<T>
    {
        private readonly IValidator<T>[] validators;

        public CompositeValidator() { }

        public CompositeValidator(params IValidator<T>[] validators)
        { 
            Guard.ArgumentIsNotNull(validators, nameof(validators));

            this.validators = validators;
        }

        public bool Validate(T target)
        {
            return this.validators.All(validator => validator.Validate(target));
        }
    }
}
