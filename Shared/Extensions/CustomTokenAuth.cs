using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using Shared.Services;

namespace Shared.Extensions
{
   public static class CustomTokenAuth
   {
      public static void AddCustomTokenAuth(this IServiceCollection service, CustomTokenOptions tokenOptions)
      {
         service.AddAuthentication(opt =>
        {
           opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
        {
           opt.TokenValidationParameters = new TokenValidationParameters
           {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ClockSkew = TimeSpan.Zero,
              ValidIssuer = tokenOptions.Issuer,
              ValidAudience = tokenOptions.Audience[0],
              IssuerSigningKey = SignService.GetSigningKey(tokenOptions.SecurityKey)
           };
        });
      }
   }
}