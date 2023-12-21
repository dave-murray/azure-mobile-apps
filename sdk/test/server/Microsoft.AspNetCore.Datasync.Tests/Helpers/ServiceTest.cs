﻿// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Datasync.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Datasync.Tests.Helpers;

public abstract class ServiceTest
{
    protected readonly ServiceApplicationFactory factory;
    protected readonly HttpClient client;
    protected readonly JsonSerializerOptions serializerOptions;
    protected readonly DateTimeOffset StartTime = DateTimeOffset.UtcNow;

    protected ServiceTest(ServiceApplicationFactory factory)
    {
        this.factory = factory;
        this.client = factory.CreateClient();
        this.serializerOptions = GetSerializerOptions();
    }

    private static JsonSerializerOptions GetSerializerOptions()
    {
        JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new DateTimeOffsetConverter());
        options.Converters.Add(new DateTimeConverter());
        return options;
    }

    protected void SeedKitchenSink()
    {
        DateOnly SourceDate = new(2022, 1, 1);
        for (int i = 0; i < 365; i++)
        {
            DateOnly date = SourceDate.AddDays(i);
            InMemoryKitchenSink model = new()
            {
                Id = string.Format("id-{0:000}", i),
                Version = Guid.NewGuid().ToByteArray(),
                UpdatedAt = DateTimeOffset.UtcNow,
                Deleted = false,
                DateOnlyValue = date,
                TimeOnlyValue = new TimeOnly(date.Month, date.Day)
            };
            factory.Store(model);
        }
    }
}
