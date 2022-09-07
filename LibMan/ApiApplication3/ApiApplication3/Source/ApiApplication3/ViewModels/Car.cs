namespace ApiApplication3.ViewModels;

/// <summary>
/// A make and model of car.
/// </summary>
public class Car
{
    /// <summary>
    /// Gets or sets the cars unique identifier.
    /// </summary>
    /// <example>1</example>
    public int CarId { get; set; }

    /// <summary>
    /// Gets or sets the number of cylinders in the cars engine.
    /// </summary>
    /// <example>6</example>
    public int Cylinders { get; set; }

    /// <summary>
    /// Gets or sets the make of the car.
    /// </summary>
    /// <example>Honda</example>
    public string Make { get; set; } = default!;

    /// <summary>
    /// Gets or sets the model of the car.
    /// </summary>
    /// <example>Civic</example>
    public string Model { get; set; } = default!;

    /// <summary>
    /// Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
    /// </summary>
    /// <example>/cars/1</example>
    public Uri Url { get; set; } = default!;
}
