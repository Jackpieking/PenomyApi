namespace PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;

public enum G1CompleteRegistrationResponseStatusCode
{
    SUCCESS = 1,

    DATABASE_ERROR,

    PASSWORD_INVALID,

    INVALID_TOKEN,

    USER_EXIST,

    INPUT_VALIDATION_FAIL
}
