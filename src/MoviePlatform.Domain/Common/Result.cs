namespace MoviePlatform.Domain.Common;

public class Result
{
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;

	private readonly List<Error> _errors = [];
	public IReadOnlyList<Error> Errors => _errors.AsReadOnly();

	protected Result(bool isSuccess, IEnumerable<Error> errors)
	{
		List<Error>	errorList = errors.ToList();

		if (isSuccess && errorList.Count != 0)
		{
			throw new InvalidOperationException("Successful result cannot have errors.");
		}

		if (!isSuccess && errorList.Count == 0)
		{
			throw new InvalidOperationException("Failed result must have at least one error.");
		}

		IsSuccess = isSuccess;
		_errors.AddRange(errorList);
	}

	public static Result Success() => new(true, []);
	public static Result Failure(Error error) => new(false, [error]);
	public static Result Failure(IEnumerable<Error> errors) => new(false, errors);

	public static Result<TValue> Success<TValue>(TValue value) => new(value, true, []);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, [error]);
    public static Result<TValue> Failure<TValue>(IEnumerable<Error> errors) => new(default, false, errors);
}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access value of a failed result.");

    internal Result(TValue? value, bool isSuccess, IEnumerable<Error> errors) : base(isSuccess, errors)
    {
        _value = value;
    }
}
