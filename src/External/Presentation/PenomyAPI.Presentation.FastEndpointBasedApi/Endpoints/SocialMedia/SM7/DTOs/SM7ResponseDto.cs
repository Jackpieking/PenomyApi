using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM7.DTOs;

public class SM7ResponseDto
{
    public IEnumerable<GroupDto> Groups { get; set; }
}

public class GroupDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public string Description { get; set; }
    public string CoverPhotoUrl { get; set; }
    public int TotalMembers { get; set; }
    public bool RequireApprovedWhenPost { get; set; }
    public SocialGroupStatus GroupStatus { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ActivityTime { get; set; }
}