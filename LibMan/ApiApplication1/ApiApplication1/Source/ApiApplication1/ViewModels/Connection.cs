namespace ApiApplication1.ViewModels;

/// <summary>
/// A paged collection of items.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
public class Connection<T>
{
    public Connection() => this.Items = new List<T>();

    /// <summary>
    /// Gets or sets the total count of items.
    /// </summary>
    /// <example>100</example>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the page information.
    /// </summary>
    public PageInfo PageInfo { get; set; } = default!;

    /// <summary>
    /// Gets the items.
    /// </summary>
    public List<T> Items { get; }
}
