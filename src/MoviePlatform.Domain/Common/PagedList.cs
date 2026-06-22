namespace MoviePlatform.Domain.Common;

public sealed class PagedList<T>
{
    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public long TotalCount { get; }
    public int TotalPages => (int) Math.Ceiling((double) TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

	private PagedList(List<T> items, int page, int pageSize, long totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public static PagedList<T> Create(List<T> items, int page, int pageSize, long totalCount)
    {
        return new(items, page, pageSize, totalCount);
    }
}
