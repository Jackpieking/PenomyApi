namespace PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;

public enum Art12GetChapterDetailResponseAppCode
{
    SUCCESS = 0,

    CHAPTER_IS_NOT_FOUND = 1,

    CHAPTER_IS_TEMPORARILY_REMOVED = 2,

    NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR = 3,

    DATABASE_ERROR = 4
}
