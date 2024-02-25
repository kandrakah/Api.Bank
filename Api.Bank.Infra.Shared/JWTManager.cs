using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace Api.Bank.Infra.Shared
{
    /// <summary>
    /// Classe de gerenciamento de token.
    /// </summary>
    public class JWTManager
    {
        /// <summary>
        /// Instância do serviço de logs.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Handler de tokens.
        /// </summary>
        private readonly JwtSecurityTokenHandler _handler;

        /// <summary>
        /// Nome do emissor do token.
        /// </summary>
        private readonly string _issuer;

        /// <summary>
        /// Enumeração de audiências válidas.
        /// </summary>
        private readonly IEnumerable<string> _validAudiences;

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="logger">Instância do serviço de logs.</param>
        /// <param name="issuer">Nome do emissor do token.</param>
        /// <param name="validAudiences">Enumeração de audiências válidas.</param>
        public JWTManager(ILogger logger, string issuer, IEnumerable<string> validAudiences)
        {
            this._logger = logger;
            this._handler = new JwtSecurityTokenHandler();
            this._issuer = issuer;
            this._validAudiences = validAudiences;
        }

        /// <summary>
        /// Método que verifica se o token pode ser lido.
        /// </summary>
        /// <param name="token">Token à ser validado.</param>
        /// <returns>Verdadeiro caso o token possa ser lido e falso caso contrário.</returns>
        public bool CanReadToken(string token)
        {
            var result = _handler.CanReadToken(token);
            return result;
        }

        /// <summary>
        /// Método que faz a leitura do token.
        /// </summary>
        /// <param name="token">Token à ser lido.</param>
        /// <returns><see cref="SecurityToken"/> contendo os dados do token informado.</returns>
        public SecurityToken ReadToken(string token)
        {
            var result = _handler.ReadToken(token);
            return result;
        }

        /// <summary>
        /// Método que faz a leitura do token.
        /// </summary>
        /// <param name="token">Token à ser lido.</param>
        /// <returns><see cref="JwtSecurityToken"/> contendo os dados do token informado.</returns>
        public JwtSecurityToken ReadJwtToken(string token)
        {
            var result = _handler.ReadJwtToken(token);
            return result;
        }

        /// <summary>
        /// Método que faz a criação de um token.
        /// </summary>
        /// <param name="audience">Audiência do token.</param>
        /// <param name="expiration">Validade, em anos, do token.</param>
        /// <param name="claims">Atributos do token.</param>
        /// <param name="keyParameters">Parâmetros da chave de segurança do token. Veja <see cref="RSAParameters"/></param>
        /// <param name="algorithm">Algoritmo utilizado na geração da assinatura do token. Veja <see cref="SecurityAlgorithms"/></param>
        /// <returns>Token JWT gerado.</returns>
        public string CreateToken(string audience, int expiration, IDictionary<string, object> claims, RSAParameters keyParameters, string algorithm = SecurityAlgorithms.RsaSha512Signature)
        {
            var now = DateTime.UtcNow;
            var descriptor = new SecurityTokenDescriptor();
            descriptor.Issuer = this._issuer;
            descriptor.Audience = audience;
            descriptor.NotBefore = now;
            descriptor.Expires = now.AddYears(expiration);
            descriptor.SigningCredentials = this.CreateSigningCredentials(this.GetSecurityKey(keyParameters), algorithm);
            descriptor.Claims = claims;

            var token = _handler.CreateEncodedJwt(descriptor);
            return token;
        }

        /// <summary>
        /// Método que faz a criação de um token.
        /// </summary>
        /// <param name="audience">Audiência do token.</param>
        /// <param name="expiration">Data de validade do token.</param>
        /// <param name="claims">Atributos do token.</param>
        /// <param name="keyParameters">Parâmetros da chave de segurança do token. Veja <see cref="RSAParameters"/></param>
        /// <param name="algorithm">Algoritmo utilizado na geração da assinatura do token. Veja <see cref="SecurityAlgorithms"/></param>
        /// <returns>Token JWT gerado.</returns>
        public string CreateToken(string audience, DateTime expiration, IDictionary<string, object> claims, RSAParameters keyParameters, string algorithm = SecurityAlgorithms.RsaSha512Signature)
        {
            var now = DateTime.UtcNow;
            var descriptor = new SecurityTokenDescriptor();
            descriptor.IssuedAt = now;
            descriptor.Issuer = _issuer;
            descriptor.Audience = audience;
            descriptor.NotBefore = now;
            descriptor.Expires = expiration;
            descriptor.SigningCredentials = this.CreateSigningCredentials(this.GetSecurityKey(keyParameters), algorithm);
            descriptor.Claims = claims;

            var token = _handler.CreateEncodedJwt(descriptor);
            return token;
        }

        /// <summary>
        /// Método que faz a validação do token.
        /// </summary>
        /// <param name="token">Token que será validado.</param>
        /// <param name="keyParameters">Parâmetros da chave de segurança do token. Veja <see cref="RSAParameters"/></param>
        public SecurityToken ValidateToken(string token, RSAParameters keyParameters)
        {
            return this.ValidateToken(token, this.GetValidationParameters(keyParameters));
        }

        /// <summary>
        /// Método que faz a validação do token.
        /// </summary>
        /// <param name="token">Token que será validado.</param>
        /// <param name="parameters">Parâmetros de validação de token. Veja <see cref="TokenValidationParameters"/></param>
        public SecurityToken ValidateToken(string token, TokenValidationParameters parameters)
        {
            if (!_handler.CanReadToken(token))
            {
                throw new InvalidCredentialException("O token informado não pôde ser lido!");
            }

            _handler.ReadJwtToken(token);

            try
            {
                _handler.ValidateToken(token, parameters, out SecurityToken securityToken);
                return securityToken;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("O token não pôde ser validado: {reason}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Método que gera um objeto <see cref="SecurityKey"/> com base nos parâmetros
        /// da chave de segurança.
        /// </summary>
        /// <param name="keyParameters">Parâmetros da chave de segurança. Veja <see cref="RSAParameters"/></param>
        /// <returns><see cref="SecurityKey"/> contendo os dados da chave de segurança.</returns>
        private SecurityKey GetSecurityKey(RSAParameters keyParameters)
        {
            return new RsaSecurityKey(keyParameters);
        }

        /// <summary>
        /// Método que cria as credenciais de assinatura do token.
        /// </summary>
        /// <param name="securityKey">Dados da chave de segurança. Veja <see cref="SecurityKey"/></param>
        /// <param name="algorithm">Algoritmo utilizado na geração da assinatura do token. Veja <see cref="SecurityAlgorithms"/></param>
        /// <returns>Dados das credenciais de asinatura do token. Veja <see cref="SigningCredentials"/></returns>
        private SigningCredentials CreateSigningCredentials(SecurityKey securityKey, string algorithm = SecurityAlgorithms.RsaSha512Signature)
        {
            return new SigningCredentials(securityKey, algorithm);
        }

        /// <summary>
        /// Método que obtém os parâmetros de validação de token.
        /// </summary>
        /// <param name="keyParameters">Parâmetros da chave de segurança. Veja <see cref="RSAParameters"/></param>
        /// <returns><see cref="TokenValidationParameters"/> contendo os parâmetros de validação.</returns>
        private TokenValidationParameters GetValidationParameters(RSAParameters keyParameters)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = this._issuer,
                ValidAudiences = this._validAudiences,
                IssuerSigningKey = this.GetSecurityKey(keyParameters)
            };

            return parameters;
        }
    }
}
