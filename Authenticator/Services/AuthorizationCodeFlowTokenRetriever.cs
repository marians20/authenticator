// <copyright file="AuthorizationCodeFlowTokenRetriever.cs" company="Microsoft">
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
internal sealed class AuthorizationCodeFlowTokenRetriever : ITokenRetriever
{
    private readonly Oauth2Settings settings;
    private readonly ILogger<ClientCredentialsFlowTokenRetriever> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationCodeFlowTokenRetriever"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    public AuthorizationCodeFlowTokenRetriever(
        Oauth2Settings settings,
        ILogger<ClientCredentialsFlowTokenRetriever> logger)
    {
        this.settings = settings;
        this.logger = logger;
    }

    /// <summary>
    /// Get Token via Authorization Code Flow
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetTokenAsync()
    {
        logger.LogTrace("Authenticate via authorization code flow");
        var scopes = settings.Scope; ////.FinalScope();
        var clientApplication = PublicClientApplicationBuilder
                .Create(settings.ClientId)
                .WithAuthority(new Uri(settings.AuthUrl))
                .WithRedirectUri(settings.RedirectUrl)
                .WithTenantId(settings.TenantId)
                .WithCacheOptions(CacheOptions.EnableSharedCacheOptions)
                .Build();

        try
        {
            var accounts = await clientApplication.GetAccountsAsync().ConfigureAwait(false);

            var result = await clientApplication.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                .ExecuteAsync().ConfigureAwait(false);

            return result.AccessToken;
        }
        catch (MsalUiRequiredException)
        {
            try
            {
                var result = await clientApplication.AcquireTokenInteractive(scopes)
                    .ExecuteAsync().ConfigureAwait(false);

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            throw;
        }
    }
}
