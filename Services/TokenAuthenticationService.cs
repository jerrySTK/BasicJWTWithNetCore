using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTApi.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWTApi.Services
{
    public interface ITokenAuthenticationService
    {
        bool IsAuthenticated(TokenRequest tokenRequest,out string token);
    }
    public class TokenAuthenticationService : ITokenAuthenticationService
    {

        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IUserManagementService userManagementService,IOptions<TokenManagement> tokenManagement)
        {
            _userManagementService = userManagementService;
            _tokenManagement = tokenManagement.Value;
        }

        public bool IsAuthenticated(TokenRequest tokenRequest, out string token)
        {

            token = string.Empty;
            if (!_userManagementService.IsValidUser(tokenRequest.Username,tokenRequest.Password)) return false;

            var claim = new[] {
                new Claim(ClaimTypes.Name,tokenRequest.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer,
                                                _tokenManagement.Audience,
                                                claim,
                                                expires:DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                                                signingCredentials:credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return true;
        }
    }
}