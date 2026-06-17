namespace MoviePlatform.Domain.Movies;

public static class MovieConstants
{
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
		public static readonly int MaxDigitsPrecision = 2;
		public static readonly int DecimalPlacesScale = 1;

		public static readonly int MinScore = 1;
		public static readonly int MaxScore = 5;
	}

	public static class Content
	{
		public static readonly int MaxLength = 500;
	}
}
