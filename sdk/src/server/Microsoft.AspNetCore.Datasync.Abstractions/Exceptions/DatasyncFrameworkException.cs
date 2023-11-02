﻿// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Microsoft.AspNetCore.Datasync.Abstractions;

/// <summary>
/// The base exception class for all exceptions thrown by Azure Mobile Apps.
/// </summary>
[ExcludeFromCodeCoverage]
public class DatasyncFrameworkException : Exception
{
    /// <inheritdoc />
    public DatasyncFrameworkException()
    {
    }

    /// <inheritdoc />
    public DatasyncFrameworkException(string? message) : base(message)
    {
    }

    /// <inheritdoc />
    public DatasyncFrameworkException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <inheritdoc />
    protected DatasyncFrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
