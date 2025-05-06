namespace TaktTusur.InMemoria.Api.Models;

public class PagedResultModel<T>
{
    public IEnumerable<T> Items { get; set; }

    public int TotalCount { get; set; }

    public int Take { get; set; }

    public int Skip { get; set; }
}
