// <copyright file="Oauth2Settings.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Authenticator.Models;

/// <summary>
/// Oauth2Settings
/// </summary>
public sealed class Oauth2Settings
{
    /// <summary>
    /// AuthServer
    /// </summary>
    public string AuthServer { get; set; } = default!;

    /// <summary>
    /// ClientId
    /// </summary>
    public string ClientId { get; set; } = default!;

    /// <summary>
    /// ClientSecret
    /// </summary>
    public string ClientSecret { get; set; } = default!;

    /// <summary>
    /// TenantId
    /// </summary>
    public string TenantId { get; set; } = default!;

    /// <summary>
    /// Scope
    /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
    public string[] Scope { get; set; } = default!;
#pragma warning restore CA1819 // Properties should not return arrays

    /// <summary>
    /// RedirectUrl
    /// </summary>
    public string RedirectUrl { get; set; } = default!;

    /// <summary>
    /// Authority
    /// </summary>
    public string Authority => $"{AuthServer}/{TenantId}/oauth2/v2.0";

    /// <summary>
    /// Gets the Auth Url
    /// </summary>
    public string AuthUrl => $"{Authority}/authorize";

    /// <summary>
    /// Gets the Access Token Url
    /// </summary>
    public string AccessTokenUrl => $"{Authority}/token";

    /// <summary>
    /// Gets the Scope
    /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
    public string[] FinalScope() => Scope.Select(s => $"api://{ClientId}/{s}").ToArray();
#pragma warning restore CA1819 // Properties should not return arrays
}