namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM40.DTOs;

public class SM40RequestDto
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public string GroupId { get; set; }

    public string MemberId { get; set; }
}
