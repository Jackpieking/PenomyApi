namespace PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;

public enum G34CompleteResetPasswordResponseStatusCode
{
    SUCCESS = 1,

    DATABASE_ERROR,

    INVALID_PASSWORD,

    INVALID_TOKEN,

    INPUT_VALIDATION_FAIL
}
