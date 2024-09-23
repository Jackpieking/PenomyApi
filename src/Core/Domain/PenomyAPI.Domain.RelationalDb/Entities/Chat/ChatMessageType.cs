namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

/// <summary>
///     This enum is used to specify the type of chat message
///     that will be send to the chat group.
/// </summary>
/// <remarks>
///     Please read in the 02_ERD_Description (supported document)
///     about the table (ChatMessage) for more detail information.
/// </remarks>
public enum ChatMessageType
{
    /// <summary>
    ///     The chat message that send by the user
    ///     by default will have this type.
    /// </summary>
    NormalMessage = 1,

    /// <summary>
    ///     The chat message with this type is automatically
    ///     send by the system when the users do some
    ///     special events in the chat group.
    /// </summary>
    NotificationMessage = 2
}
