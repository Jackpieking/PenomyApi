using PenomyAPI.App.Common.Models.ArtworkCreation;
using PenomyAPI.App.FeatArt4.Infrastructures;

namespace PenomyAPI.Infra.FeatArt4;

public class FeatArt4FileUploadService : IFeatArt4FileUploadService
{
    public FeatArt4FileUploadService()
    {
    }

    public Task<bool> UploadFileAsync(ImageFileInfo imageFileInfo)
    {
        return Task.FromResult(true);
    }
}

public class GoogleDriveOptions
{
    public string SecretKey { get; set; }

    public string ApiKey { get; set; }
}
