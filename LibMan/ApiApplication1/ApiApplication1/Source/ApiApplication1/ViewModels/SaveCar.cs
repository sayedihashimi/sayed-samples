namespace ApiApplication1.ViewModels;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// A make and model of car.
/// </summary>
public class SaveCar
{
    /// <summary>
    /// Gets or sets the number of cylinders in the cars engine.
    /// </summary>
    /// <example>6</example>
    [Range(1, 20)]
    public int Cylinders { get; set; }

    /// <summary>
    /// Gets or sets the make of the car.
    /// </summary>
    /// <example>Honda</example>
    [Required]
    public string Make { get; set; } = default!;

    /// <summary>
    /// Gets or sets the model of the car.
    /// </summary>
    /// <example>Civic</example>
    [Required]
    public string Model { get; set; } = default!;
}
