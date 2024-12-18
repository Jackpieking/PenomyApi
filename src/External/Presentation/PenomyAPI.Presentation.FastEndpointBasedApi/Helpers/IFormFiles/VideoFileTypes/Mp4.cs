using FileTypeChecker.Abstracts;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles.VideoFileTypes;

public class Mp4 : FileType, IFileType
{
    public const string TypeName = "MPEG-4 Part 14";
    public const string TypeExtension = "mp4";
    /// <summary>
    ///     This magic bytes will be used to identify the real content of the file.
    /// </summary>
    /// <remarks>
    ///     To get the magic bytes: https://en.wikipedia.org/wiki/List_of_file_signatures.
    ///     Article about magic bytes: https://library.mosse-institute.com/articles/2022/04/file-magic-numbers-the-easy-way-to-identify-file-extensions/file-magic-numbers-the-easy-way-to-identify-file-extensions.html
    /// </remarks>
    private static readonly byte[] MagicBytes = { 0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56 };

    public Mp4() : base(TypeName, TypeExtension, MagicBytes)
    {
    }
}
