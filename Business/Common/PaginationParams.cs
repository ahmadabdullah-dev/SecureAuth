namespace Business;

public class PaginationParams
{
    private const int MaxPageSize = 50; // max items can come at a page
    private int _pageSize = 10; // Default page size
    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; // Enforce max limit
    }
}