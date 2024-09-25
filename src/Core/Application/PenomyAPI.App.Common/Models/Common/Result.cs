namespace PenomyAPI.App.Common.Models.Common
{
    public sealed class Result<TValue>
    {
        public TValue Value { get; set; }

        public Result() { }

        public Result(TValue value) {  Value = value; }
    }
}
