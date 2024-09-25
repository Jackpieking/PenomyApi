using PenomyAPI.App.Common.Models.ArtworkCreation;

namespace PenomyAPI.App.FeatArt4.Infrastructures
{
    public interface IFeatArt4FileUploadService
    {
        Task<bool> UploadFileAsync(ImageFileInfo imageFileInfo);
    }
}
