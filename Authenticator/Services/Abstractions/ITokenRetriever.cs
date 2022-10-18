// <copyright file="ITokenRetriever.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Authenticator.Services.Abstractions;

/// <summary>
/// ITokenRetriever
/// </summary>
public interface ITokenRetriever
{
    /// <summary>
    /// GetTokenAsync
    /// </summary>
    /// <returns></returns>
    Task<string> GetTokenAsync();
}