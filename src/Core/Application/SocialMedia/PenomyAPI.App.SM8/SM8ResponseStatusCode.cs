namespace PenomyAPI.App.SM8;

public enum SM8ResponseStatusCode
{
    SUCCESS = 0,
    INVALID_REQUEST = 1,
    FAILED = 2,
    UN_AUTHORIZED = 3,
    FORBIDDEN = 4,
    INVALID_FILE_EXTENSION = 5,
    INVALID_FILE_FORMAT = 6,
    MAXIMUM_IMAGE_FILE_SIZE = 7,
}
