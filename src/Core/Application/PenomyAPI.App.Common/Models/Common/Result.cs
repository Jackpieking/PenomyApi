namespace PenomyAPI.App.Common.Models.Common;

public sealed class Result<TValue>
{
    /// <summary>
    ///     Check if the result is success or not to get its inner value.
    /// </summary>
    public bool IsSuccess { get; set; }

    public TValue Value { get; set; }

    public Result() { }

    public Result(TValue value)
    {
        Value = value;
    }

    /// <summary>
    ///     A short hand to create a Success <see cref="Result{T}"/> instance
    ///     with input <paramref name="value"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    ///     A <see cref="Result{T}"/> with (IsSucess = <see langword="true"/>)
    /// </returns>
    public static Result<TValue> Success(TValue value) => new(value) { IsSuccess = true };

    /// <summary>
    ///     A short hand to create a Failed <see cref="Result{T}"/> instance
    ///     with empty return value.
    /// </summary>
    /// <returns>
    ///     A <see cref="Result{T}"/> with (IsSucess = <see langword="false"/>)
    /// </returns>
    public static Result<TValue> Failed() => new(default)
    {
        IsSuccess = false
    };

    /// <summary>
    ///     A short hand to create a Failed <see cref="Result{T}"/> instance
    ///     with return value.
    /// </summary>
    /// <returns>
    ///     A <see cref="Result{T}"/> with (IsSucess = <see langword="false"/>)
    /// </returns>
    public static Result<TValue> Failed(TValue value) => new(value)
    {
        IsSuccess = false,
        Value = value
    };
}
