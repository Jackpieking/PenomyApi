namespace PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;

public enum Art22GetChapterDetailResponseAppCode
{
    SUCCESS = 1,

    CHAPTER_IS_TEMPORARILY_REMOVED,

    NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR,

    DATABASE_ERROR
}
