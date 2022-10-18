// See https://aka.ms/new-console-template for more information

using System.Security.Claims;
using Authenticator.Extensions;
using Authenticator.Models;
using Authenticator.Services;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder().AddUserSecrets(typeof(Program).Assembly).Build();
var oauth2Settings = configuration.GetSection("RiseFeedbackSdk:OAuth2").Get<Oauth2Settings>();
var auth = new ClientCredentialsFlowTokenRetriever(oauth2Settings);
var token = await auth.GetTokenAsync();
var (principal, jwtToken) = token.ValidateJwt();

Console.WriteLine(token);
Console.WriteLine(string.Join(Environment.NewLine, principal.Claims.Select(claim => $"{claim.Type} => {claim.Value}")));

var roles = principal.Claims.Where(claim => claim.Type.Equals(ClaimTypes.Role));
Console.WriteLine(string.Join(Environment.NewLine, roles.Select(r => r.Value)));

////Console.WriteLine(jwtToken?.ToJson());
////Console.WriteLine(jwtToken?.EncodedPayload.DecodeBase64());

