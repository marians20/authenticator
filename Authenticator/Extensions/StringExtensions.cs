// <copyright file="StringExtensions.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Logging;

namespace Authenticator.Extensions;

/// <summary>
/// StringExtensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Encode value to Base64
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string EncodeBase64(this string value)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(valueBytes);
    }

    /// <summary>
    /// Decode from Base64
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string DecodeBase64(this string value)
    {
        byte[]? valueBytes;
        var paddingSize = value.Length % 3;
        if (paddingSize != 0)
        {
            var sb = new StringBuilder(value);
            sb.Append('=', paddingSize);
            var paddedValue = sb.ToString();
            valueBytes = Convert.FromBase64String(paddedValue);
        }
        else
        {
            valueBytes = Convert.FromBase64String(value);
        }

        return Encoding.UTF8.GetString(valueBytes);
    }

    /// <summary>
    /// Extracts JwtSecurityToken from JWT string
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static JwtSecurityToken GetJwtToken(this string token) => new JwtSecurityToken(token);

    /// <summary>
    /// Validates and extracts JwtSecurityToken from JWT string
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static (ClaimsPrincipal, JwtSecurityToken?) ValidateJwt(this string token)
    {
        IdentityModelEventSource.ShowPII = true;

        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
            IssuerSigningKey = GetSecurityKey(token)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, validationParameters, out var jwt);

        return (principal, jwt as JwtSecurityToken);
    }

    private static SecurityKey? GetSecurityKey(string token)
    {
        var jwtHeader = GetJwtToken(token).Header;

        return OpenIdConnectConfiguration
            .SigningKeys
            .FirstOrDefault(ky => ky.KeyId.Equals(jwtHeader.Kid) && ky.IsSupportedAlgorithm(jwtHeader.Alg));
    }

    private static OpenIdConnectConfiguration OpenIdConnectConfiguration
    {
        get
        {
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                Constants.StsDiscoveryEndpoint,
                new OpenIdConnectConfigurationRetriever());

            var config = configManager.GetConfigurationAsync().Result;
            return config;
        }
    }

}