namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.DTOs;

public class SM9RequestDto
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public int MaxRecord { get; set; }
}
