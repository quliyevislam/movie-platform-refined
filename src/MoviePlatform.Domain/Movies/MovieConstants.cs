namespace MoviePlatform.Domain.Movies;

public static class MovieConstants
{
	public const int DefaultPage = 1;
	public const int DefaultPageSize = 10;

	public static class Title
	{
		public static readonly int MaxLength = 255;
	}

	public static class Description
	{
		public static readonly int MaxLength = 500;
	}

	public static class Score
	{
		public static readonly int MinScore = 1;
		public static readonly int MaxScore = 5;
	}

	public static class AverageScore
	{
		public static readonly int MaxDigitsPrecision = 2;
		public static readonly int DecimalPlacesScale = 1;

		public static readonly double MinScore = 1.0D;
		public static readonly double MaxScore = 5.0D;
	}

	public static class Content
	{
		public static readonly int MaxLength = 500;
	}
}
