// <copyright file="ObjectExtensions.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Authenticator.Extensions;

/// <summary>
/// ObjectExtensions
/// </summary>
internal static class ObjectExtensions
{
    /// <summary>
    /// Serialize object to Json
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToJson(this object value) => JsonSerializer.Serialize(value, new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    });

    /// <summary>
    /// Deserialize string as T
    /// </summary>
    /// <typeparam name="T">result type</typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? Deserialize<T>(this string value) => JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });
}