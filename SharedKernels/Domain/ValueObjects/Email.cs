using System.Collections.Generic;
using System.Text.RegularExpressions;
using SharedKernels.Results;

namespace SharedKernels.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; private set; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<Email>(Error.Validation("Email.Empty", "Email cannot be empty."));

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Failure<Email>(Error.Validation("Email.Invalid", "Invalid email format."));

            return Result<Email>.Success(new Email(email));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
