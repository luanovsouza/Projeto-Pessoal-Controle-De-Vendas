namespace ControleVendasAPI.Pagination;

public class QueryStringParameters
{
    const int MaxPageSize = 50;
    public int pageNumber { get; set; } = 1;
    private int _pageSize { get; set; } = MaxPageSize;

    public int PageSize
    {
        get => _pageSize;

        set
        {
            _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}