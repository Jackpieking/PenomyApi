namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.DTOs;

public class SM23RequestDto
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public string PostId { get; set; }

}
