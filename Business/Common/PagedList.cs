using Microsoft.EntityFrameworkCore;

namespace Business;
public class PagedList<T>
{
    public List<T> Items { get; set; } = []; // e.g 10 items at current page
    public int CurrentPage { get; set; } // e.g 4th page
    public int TotalPages { get; set; } // e.g 5 pages
    public int TotalCount { get; set; } // e.g 47 items
    public bool HasNextPage => CurrentPage < TotalPages; // e.g has 5th page
    public bool HasPreviousPage => CurrentPage > 1; // e.g has 3rd page

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
    {
        var totalCount = await query.CountAsync(); 
      
        var items = 
            await query.Skip((page - 1) * pageSize) // e.g (2-1) * 10  if required page 2 skip first 10 item then take from 11th to 20
            .Take(pageSize) 
            .ToListAsync();

        return new PagedList<T>
        {
            Items = items,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize), // e.g 4.7 
            TotalCount = totalCount
        };
    }
}
