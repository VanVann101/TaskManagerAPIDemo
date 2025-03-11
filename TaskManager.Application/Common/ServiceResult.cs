﻿namespace TaskManager.Application.Common {
    public class Result {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        private Result(bool isSuccess, string errorMessage) {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true, null);

        public static Result Failure(string errorMessage) => new Result(false, errorMessage);
    }

    public class Result<T> {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string ErrorMessage { get; }

        private Result(bool isSuccess, T data, string errorMessage) {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null);
        public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
    }
}
