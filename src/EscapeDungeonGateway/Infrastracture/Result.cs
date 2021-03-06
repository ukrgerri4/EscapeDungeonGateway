using System;

namespace EscapeDungeonGateway.Infrastracture
{
    public interface IResult
    {
        bool IsSuccess { get; }
        string Error { get; }
    }

    public interface IResult<out TValue> : IResult
    {
        TValue Value { get; }
    }

    public class Result : IResult
    {
        public string Error { get; }

        public bool IsSuccess { get; }
        public bool IsFail => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrWhiteSpace(error))
            {
                throw new InvalidOperationException("Success result can't come with an error");
            }
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public T Value { get; }
        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }
    }
}
