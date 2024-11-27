namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM44.DTOs;

public class SM44RequestDto
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public string GroupId { get; set; }
}
