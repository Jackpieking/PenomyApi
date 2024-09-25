namespace PenomyAPI.Presentation.FastEndpointBasedApi.Options
{
    public sealed class NSwagOptions
    {
        public DocOption Doc { get; set; } = new();

        public bool EnableJWTBearerAuth { get; set; }

        public sealed class DocOption
        {
            public PostProcessOption PostProcess { get; set; } = new();

            public AuthOption Auth { get; set; } = new();

            public sealed class PostProcessOption
            {
                public InfoOption Info { get; set; } = new();

                public sealed class InfoOption
                {
                    public string Version { get; set; }

                    public string Title { get; set; }

                    public string Description { get; set; }

                    public ContactOption Contact { get; set; } = new();

                    public LicenseOption License { get; set; } = new();

                    public sealed class ContactOption
                    {
                        public string Name { get; set; }

                        public string Email { get; set; }
                    }

                    public sealed class LicenseOption
                    {
                        public string Name { get; set; }

                        public string Url { get; set; }
                    }
                }
            }

            public sealed class AuthOption
            {
                public BearerOption Bearer { get; set; } = new();

                public sealed class BearerOption
                {
                    public int Type { get; set; }

                    public int In { get; set; }

                    public string Scheme { get; set; }

                    public string BearerFormat { get; set; }

                    public string Description { get; set; }
                }
            }
        }
    }
}
