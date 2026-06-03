using Domain.Common;
using Domain.Enums;

namespace Application.Common.Results;

/// <summary>
/// Generic result object for all application operations
/// </summary>
public class Result<T>
{
    public ResultStatus Status { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = [];

    public bool IsSuccess => Status == ResultStatus.Success;

    public static Result<T> Success(T? data = default, string? message = null)
    {
        return new Result<T>
        {
            Status = ResultStatus.Success,
            Data = data,
            Message = message
        };
    }

    public static Result<T> NotFound(string message = "Resource not found")
    {
        return new Result<T>
        {
            Status = ResultStatus.NotFound,
            Message = message
        };
    }

    public static Result<T> BadRequest(string message = "Bad request", List<string>? errors = null)
    {
        return new Result<T>
        {
            Status = ResultStatus.BadRequest,
            Message = message,
            Errors = errors ?? []
        };
    }

    public static Result<T> Unauthorized(string message = "Unauthorized")
    {
        return new Result<T>
        {
            Status = ResultStatus.Unauthorized,
            Message = message
        };
    }

    public static Result<T> Forbidden(string message = "Forbidden")
    {
        return new Result<T>
        {
            Status = ResultStatus.Forbidden,
            Message = message
        };
    }

    public static Result<T> Conflict(string message = "Conflict")
    {
        return new Result<T>
        {
            Status = ResultStatus.Conflict,
            Message = message
        };
    }

    public static Result<T> InternalServerError(string message = "Internal server error")
    {
        return new Result<T>
        {
            Status = ResultStatus.InternalServerError,
            Message = message
        };
    }
}

/// <summary>
/// Non-generic result object for operations that don't return data
/// </summary>
public class Result
{
    public ResultStatus Status { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = [];

    public bool IsSuccess => Status == ResultStatus.Success;

    public static Result Success(string? message = null)
    {
        return new Result
        {
            Status = ResultStatus.Success,
            Message = message
        };
    }

    public static Result NotFound(string message = "Resource not found")
    {
        return new Result
        {
            Status = ResultStatus.NotFound,
            Message = message
        };
    }

    public static Result BadRequest(string message = "Bad request", List<string>? errors = null)
    {
        return new Result
        {
            Status = ResultStatus.BadRequest,
            Message = message,
            Errors = errors ?? []
        };
    }

    public static Result Unauthorized(string message = "Unauthorized")
    {
        return new Result
        {
            Status = ResultStatus.Unauthorized,
            Message = message
        };
    }

    public static Result Forbidden(string message = "Forbidden")
    {
        return new Result
        {
            Status = ResultStatus.Forbidden,
            Message = message
        };
    }

    public static Result Conflict(string message = "Conflict")
    {
        return new Result
        {
            Status = ResultStatus.Conflict,
            Message = message
        };
    }

    public static Result InternalServerError(string message = "Internal server error")
    {
        return new Result
        {
            Status = ResultStatus.InternalServerError,
            Message = message
        };
    }
}
