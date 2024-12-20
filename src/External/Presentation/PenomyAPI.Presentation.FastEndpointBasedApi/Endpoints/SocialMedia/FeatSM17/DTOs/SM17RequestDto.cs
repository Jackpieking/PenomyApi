namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM17.DTOs;

public class SM17RequestDto
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public string PostId { get; set; }

    public bool IsGroupPost { get; set; }
}
