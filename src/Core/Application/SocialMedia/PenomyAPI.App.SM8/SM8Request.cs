using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;

namespace PenomyAPI.App.SM8
{
    public class SM8Request : IFeatureRequest<SM8Response>
    {
        private string _userId;

        public string GetUserId() => _userId;

        public void SetUserId(string userId) => _userId = userId;

        public AppFileInfo CoverImageFileInfo { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public bool RequireApprovedWhenPost { get; set; }
    }
}
