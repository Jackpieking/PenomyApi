namespace PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;

public enum G34CompleteResetPasswordResponseStatusCode
{
    SUCCESS = 1,

    DATABASE_ERROR,

    PASSWORD_INVALID,

    INVALID_TOKEN,

    USER_EXIST,

    INPUT_VALIDATION_FAIL
}
