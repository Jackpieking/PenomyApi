namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM41.DTOs;

public class SM41RequestDto
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public string GroupId { get; set; }

    public string MemberId { get; set; }
}
