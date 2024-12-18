using FileTypeChecker.Abstracts;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles.VideoFileTypes;

public class Mkv : FileType, IFileType
{
    public const string TypeName = "Matroska";
    public const string TypeExtension = "mkv";
    /// <summary>
    ///     This magic bytes will be used to identify the real content of the file.
    /// </summary>
    /// <remarks>
    ///     To get the magic bytes: https://en.wikipedia.org/wiki/List_of_file_signatures.
    ///     Article about magic bytes: https://library.mosse-institute.com/articles/2022/04/file-magic-numbers-the-easy-way-to-identify-file-extensions/file-magic-numbers-the-easy-way-to-identify-file-extensions.html
    /// </remarks>
    private static readonly byte[] MagicBytes = { 0x1A, 0x45, 0xDF, 0xA3 };

    public Mkv() : base(TypeName, TypeExtension, MagicBytes)
    {
    }
}
