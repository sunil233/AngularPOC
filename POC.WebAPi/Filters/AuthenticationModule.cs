using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace POCServices.Filters
{
    /// <summary>
    /// This class is used to generate and validate JSON Web Token (JWT).
    /// JWT is the approach of securely transmitting data across communication channel. 
    /// For authentication and authorization, it uses the technique of passing digitally signed tokens. 
    /// JWT comprises of three parts: Header, Payloads and Signature.
    /// </summary>
    public class AuthenticationModule
    {
        private const string securityKey = "GQDstc21ewfffffffffffFiwDffVvVBrk";
        private const string baseapi = "http://localhost:57894/";
        private SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

        /// <summary>
        /// The Method is used to generate token for user
        /// </summary>
        /// <param name="dbuser">UserSecurity</param>
        /// <returns>JWT token</returns>
        public string GenerateTokenForUser(UserSecurity dbuser)
        {

            var now = DateTime.UtcNow;
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, dbuser.UserName),
                new Claim(ClaimTypes.NameIdentifier, dbuser.UserInfoId),
                new Claim ("userdata", JsonConvert.SerializeObject(dbuser)),

            }, "acysclaims");

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = baseapi,
                Issuer = "self",
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);
            return signedAndEncodedToken;

        }

        /// <summary>
        /// Using the same key used for signing token, user payload is generated back
        /// </summary>
        /// <param name="authToken">string</param>
        /// <returns>payload</returns>
        public JwtSecurityToken GenerateUserClaimFromJWT(string authToken)
        {

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[] { baseapi },
                ValidIssuers = new string[] { "self" },
                IssuerSigningKey = signingKey
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(authToken, tokenValidationParameters, out validatedToken);

            return validatedToken as JwtSecurityToken;

        }

        public JWTAuthenticationIdentity PopulateUserIdentity(JwtSecurityToken userPayloadToken)
        {
            string name = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "unique_name").Value;
            string userId = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "nameid").Value;
            return new JWTAuthenticationIdentity(name) { UserId = Convert.ToInt32(userId), UserName = name };
        }
    }

    public class UserSecurity
    {
        public string UserName { get; set; }
        public string UserInfoId { get; set; }
    }
}