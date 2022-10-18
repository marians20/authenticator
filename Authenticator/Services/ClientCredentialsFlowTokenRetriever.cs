// <copyright file="ClientCredentialsFlowTokenRetriever.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

using Authenticator.Models;
using Authenticator.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace Authenticator.Services;

/// <summary>
/// Jwt Retriever via client credentials flow
/// </summary>
internal sealed class ClientCredentialsFlowTokenRetriever : ITokenRetriever
{
    private readonly Oauth2Settings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientCredentialsFlowTokenRetriever"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    public ClientCredentialsFlowTokenRetriever(
        Oauth2Settings settings)
    {
        this.settings = settings;
    }

    /// <summary>
    /// Get Token via Client Credentials Flow
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetTokenAsync()
    {
        try
        {
            var clientApplication = ConfidentialClientApplicationBuilder
                .Create(settings.ClientId)
                .WithClientSecret(settings.ClientSecret)
                .WithAuthority(new Uri(settings.AuthUrl))
                .Build();

            var scopes = settings.Scope;
            var authenticationResult = await clientApplication.AcquireTokenForClient(scopes)
                .ExecuteAsync().ConfigureAwait(false);

            return authenticationResult.AccessToken;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}