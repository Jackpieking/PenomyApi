namespace PenomyAPI.App.FeatG31;

public enum G31ResponseStatusCode
{
    SUCCESS = 1,

    INPUT_VALIDATION_FAIL,

    USER_NOT_FOUND,

    PASSWORD_IS_INCORRECT,

    TEMPORARY_BANNED,

    DATABASE_ERROR
}
