using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace IntegracaoDadosUsuario.WS {
    public class TokenManager {
        private const String SECRET = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static String GenerateToken(String username, Int32? expireMinutes = null) {
            var symmetricKey = Convert.FromBase64String(SECRET);
            var tokenHandler = new JwtSecurityTokenHandler();

            DateTime? expireToken = null;
            if (expireMinutes.HasValue && expireMinutes > 0) {
                expireToken = DateTime.UtcNow.AddMinutes(expireMinutes.Value);
            } else {
                var tempoExpiracaoToken = ConfigurationManager.TokenMinutosExpirar;
                if (tempoExpiracaoToken > 0) {
                    expireToken = DateTime.UtcNow.AddMinutes(tempoExpiracaoToken);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = expireToken,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public static ClaimsPrincipal GetPrincipal(String token) {
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null) {
                    return null;
                }

                var symmetricKey = Convert.FromBase64String(SECRET);

                var validationParameters = new TokenValidationParameters {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            } catch (Exception) {
                //should write log
                return null;
            }
        }

        public static Boolean TokenValid(String token) {
            var principal = GetPrincipal(token);
            var tokenValido = principal != null;

            return tokenValido;
        }
    }
}