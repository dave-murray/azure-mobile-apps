﻿// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;
using System.Collections.ObjectModel;

namespace Microsoft.Azure.Mobile.Server.TestModels
{
    public static class TestData
    {
        public static Collection<Movie> Movies
        {
            get
            {
                return CreateMovies();
            }
        }

        private static Collection<Movie> CreateMovies()
        {
            return new Collection<Movie>
            {
                new Movie { Id = "1", Name = "Bambi", Category = "Family", Rating = "G", ReleaseDate = new DateTime(1942, 8, 21), RunTimeMinutes = 69, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "2", Name = "Rocky", Category = "Drama", Rating = "PG-13", ReleaseDate = new DateTime(1976, 11, 21), RunTimeMinutes = 125, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "3", Name = "Star Wars", Category = "Action", Rating = "PG-13", ReleaseDate = new DateTime(1977, 5, 22), RunTimeMinutes = 121, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "4", Name = "Wizard of Oz", Category = "Family", Rating = "G", ReleaseDate = new DateTime(1939, 8, 25), RunTimeMinutes = 101, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "5", Name = "Eraserhead", Category = "Drama", Rating = "R", ReleaseDate = new DateTime(1977, 1, 1), RunTimeMinutes = 100, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "6", Name = "Top Gun", Category = "Drama", Rating = "PG-13", ReleaseDate = new DateTime(1986, 5, 18), RunTimeMinutes = 110, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "7", Name = "E.T. the Extra Terrestrial", Category = "Science Fiction", Rating = "PG-13", ReleaseDate = new DateTime(1982, 6, 11), RunTimeMinutes = 114, Deleted = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "8", Name = "Goonies", Category = "Family", Rating = "G", ReleaseDate = new DateTime(1985, 6, 7), RunTimeMinutes = 115, Deleted = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "9", Name = "Friday the 13th", Category = "Horror", Rating = "R", ReleaseDate = new DateTime(2009, 6, 16), RunTimeMinutes = 98, Deleted = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "10", Name = "The Shining", Category = "Drama", Rating = "R", ReleaseDate = new DateTime(1980, 5, 23), RunTimeMinutes = 100, Deleted = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Movie { Id = "11", Name = "Bill & Ted's Excellent Adventure", Category = "Action", Rating = "PG-13", ReleaseDate = new DateTime(1989, 2, 17), RunTimeMinutes = 105, Deleted = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
        }
    }
}
