namespace PenomyAPI.App.SM38;

public enum SM38ResponseStatusCode
{
    SUCCESS = 0,
    INVALID_REQUEST = 1,
    FAILED = 2,
    UN_AUTHORIZED = 3,
    FORBIDDEN = 4,
    INVALID_FILE_EXTENSION = 5,
    INVALID_FILE_FORMAT = 6,
    MAXIMUM_IMAGE_FILE_SIZE = 7,
    DATABSE_ERROR = 7,
}
