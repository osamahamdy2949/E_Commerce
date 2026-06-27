using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class Result
    {
        protected Result(bool isSuccess, IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }
        public bool IsSuccess { get; }
        public IReadOnlyList<Error> Errors { get; }

        public static Result Ok() => new(true, Array.Empty<Error>());
        public static Result Fail(Error error) => new(false, new[] { error });
        public static Result Fail(IReadOnlyList<Error> errors) => new(false, errors);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Data => IsSuccess ? _value : throw new InvalidOperationException("Cannot access value of a failed result.");
        private Result(bool isSuccess, TValue? value, IReadOnlyList<Error> errors) : base(isSuccess, errors)
        {
            _value = value!;
        }
        public static Result<TValue> Ok(TValue value) => new(true, value, Array.Empty<Error>());
        public static new Result<TValue> Fail(Error error) => new(false, default, new[] { error });
        public static new Result<TValue> Fail(IReadOnlyList<Error> errors) => new(false, default, errors);

        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
    }
}
