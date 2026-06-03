namespace Domain.Enums;

/// <summary>
/// Represents the result status of operations
/// </summary>
public enum ResultStatus
{
    Success = 0,
    NotFound = 1,
    BadRequest = 2,
    Unauthorized = 3,
    Forbidden = 4,
    Conflict = 5,
    InternalServerError = 6
}
