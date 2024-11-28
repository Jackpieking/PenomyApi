namespace PenomyAPI.App.G25.OtherHandlers.AddGuestViewHistory;

public enum G25AddGuestViewHistoryResponseAppCode
{
    SUCCESS = 1,

    GUEST_ID_NOT_FOUND,

    /// <summary>
    ///     If the artworkId and chapterId are not correct.
    /// </summary>
    CHAPTER_IS_NOT_FOUND,

    DATABASE_ERROR,
}
