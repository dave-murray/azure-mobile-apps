﻿// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Datasync.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Microsoft.AspNetCore.Datasync.InMemory.Tests;

[ExcludeFromCodeCoverage]
public class InMemoryMovie : InMemoryTableData, IMovie, IEquatable<IMovie>
{
    /// <summary>
    /// True if the movie won the oscar for Best Picture
    /// </summary>
    public bool BestPictureWinner { get; set; }

    /// <summary>
    /// The running time of the movie
    /// </summary>
    [Required]
    [Range(60, 360)]
    public int Duration { get; set; }

    /// <summary>
    /// The MPAA rating for the movie, if available.
    /// </summary>
    public MovieRating Rating { get; set; } = MovieRating.Unrated;

    /// <summary>
    /// The release date of the movie.
    /// </summary>
    [Required]
    public DateOnly ReleaseDate { get; set; }

    /// <summary>
    /// The title of the movie.
    /// </summary>
    [Required]
    [StringLength(60, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The year that the movie was released.
    /// </summary>
    [Required]
    [Range(1920, 2030)]
    public int Year { get; set; }

    /// <summary>
    /// Determines if this movie has the same content as another movie.
    /// </summary>
    /// <param name="other">The other movie</param>
    /// <returns>true if the content is the same</returns>
    public bool Equals(IMovie other)
        => other != null
        && other.BestPictureWinner == BestPictureWinner
        && other.Duration == Duration
        && other.Rating == Rating
        && other.ReleaseDate == ReleaseDate
        && other.Title == Title
        && other.Year == Year;

    /// <summary>
    /// Clones this movie into another new movie.
    /// </summary>
    /// <returns>The new movie</returns>
    public InMemoryMovie Clone() => new()
    {
        Id = this.Id,
        Deleted = this.Deleted,
        UpdatedAt = this.UpdatedAt,
        Version = this.Version.ToArray(),
        BestPictureWinner = this.BestPictureWinner,
        Duration = this.Duration,
        Rating = this.Rating,
        ReleaseDate = this.ReleaseDate,
        Title = this.Title,
        Year = this.Year
    };

    /// <summary>
    /// Converts this object to a dictionary.
    /// </summary>
    public Dictionary<string, object> ToDictionary()
    {
        string json = JsonSerializer.Serialize(this);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
    }
}