using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;

[ProtoContract]
public class CategoryDto
{
    [ProtoMember(1)]
    public string CategoryId { get; set; }

    [ProtoMember(2)]
    public string CategoryName { get; set; }
}
