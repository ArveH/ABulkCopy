namespace CreateMssTestDatabase.Type;

public enum TError
{
    ConnectionError = 0,
    HttpError = 1,
    ObjectNotFound = 2,
    UnprocessableObject = 3, // HttpStatusCode 422
    StatusCodeError = 4,
    UnexpectedError = 5
}