namespace PenomyAPI.Persist.Postgres.Common.DatabaseConstants;

public static class DatabaseNativeTypes
{
    public const string TEXT = "TEXT";

    public const string TIMESTAMPTZ = "TIMESTAMPTZ";

    public static string DECIMAL(int precision, int scale) => $"DECIMAL({precision},{scale})";
}
