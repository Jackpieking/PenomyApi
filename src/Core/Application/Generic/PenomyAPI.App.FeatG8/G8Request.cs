using PenomyAPI.App.Common;
using PenomyAPI.App.FeatG8.OtherHandlers;

namespace PenomyAPI.App.FeatG8;

public class G8Request : IFeatureRequest<G8Response>
{
    public long Id { get; set; }
    public int StartPage { get; set; }
    public int PageSize => G8GetPaginationOptionsResponse.DEFAULT_PAGE_SIZE;
}
