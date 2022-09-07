namespace ApiApplication3.Options;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Server.Kestrel.Core;

/// <summary>
/// All options for the application.
/// </summary>
public class ApplicationOptions
{
    public ApplicationOptions() => this.CacheProfiles = new CacheProfileOptions();

    [Required]
    public CacheProfileOptions CacheProfiles { get; }

    [Required]
    public CompressionOptions Compression { get; set; } = default!;

    [Required]
    public ForwardedHeadersOptions ForwardedHeaders { get; set; } = default!;

    [Required]
    public HostOptions Host { get; set; } = default!;

    [Required]
    public KestrelServerOptions Kestrel { get; set; } = default!;
}
