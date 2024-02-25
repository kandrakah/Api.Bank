using Api.Bank.Domain.Models;
using Api.Bank.Infra.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Api.Bank.Application.Attributes
{
    /// <inheritdoc/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private const string DEV_KEY = "<RSAKeyValue><Modulus>2xgCHlintQW82/wAUbWDzNedfcNjT2AnZOvF0TkJM6V6yltUAsZlFVxF+4/cK6A9NgkLieykdAkcbJIdJ4O/Y6zluiji8U2Re3D4GIBLnmljfeiRzHRzsKSENfbiJjoRM+fQjVJ5eu5fkDqTpkQZR8dLPfk5MXrrXtt6hLP0kQhJxdohKk5yqg8a/VotH58J03iggn4GOPRXJTzgOxUv11RBuWMVST8BbDnW9fNhjUKAo54VBvmgM/OGY+ONzY+7ZrScajEYmEPqtEBbvAV07RcOnFUDFOotdoS/4ujuwwpoAxp4DJhNFEW+Qq9pmrtA3Ksvthtjex2CplTEn8/k/Q==</Modulus><Exponent>AQAB</Exponent><P>61GdhQw56YPEKn93UPUJRwimT0hNOLBGsuNtzpR3o406eNri7VXN3TGuUmCijgCiDA/7qoIykYYRyV46OcYZProPKOCjNK7F9ZcDg61gL04ZK4aPMAQ/z2xbzPTQv4m6fwH3WUfcNRxmngX1TNIUro+WEobVSCr2Sar2VteShW8=</P><Q>7llZiIw+Dy1mhqfC+zEo4V3MMm5M0GjElCj5+5afA2ltnLhJxi3zakWciYPK2IfXeMwcZ+wS8w4ipofNlaOXdJw0kJn5Axe/Yqsn3HQ7r7JNzoCOnoDYkjEw+Zbr2kzqzljWGTDEoJ5sLdk209ORCyQYWi1fb8wObNbaS59+flM=</Q><DP>5nBsIwhQBf8FMODWRS9QQIGV63nQNT1aXoAc3fnCFyWMJyGiq+wkxwGJxh3f0dOI3Osh9l29i4iRDnL57e2ydxzZD5y79jFYpiGWTXKRCIIgX+FLgygGHOfuFg2ABrGnMg0I3iNi35fKXeFT0EDCBBeUTNxBLIT13jpPNcexMn0=</DP><DQ>wpGHyDMLuiIKruOmBJ41y/tg5M4rwHm192iX4Ows9IEM7MF33+LE8TzQM2W1ohsKJ18f1SnesjEY0i7MAVqhGZ0mP+ChaQfqwOYNTSZJTvzUDT3err9pi7XCUA0GjZb6muXkKH4qFSAeGTNXUYZEwf5+LqcZYE8Ie4rMeq1Bemc=</DQ><InverseQ>ofnbXKLEFwS6nCvniMa5AbPeh0+j8GMQiVXeHsplb5VntNgWJzwjv5HoaaHU/Z9oaOl02cI7Gxq1qoyIqqAhevkIdUiEQs1u5zABxwoN2HqDkj0oAgXt9WRO9BGk9+ICuvSKoMgCSUH76UrypVYS7B6ci2v7oV+joHYo1X37kak=</InverseQ><D>UkQRh5OCZVZiM7fL5uJhOuk7GZ3kDEnugNaG0Ki568qW8KSaRvB3xC2BmZPK+BaS5VdAHendVjSVl67Fg7DUjHi/ScdQb4rQtakVfx2tRy4LTZumIz8WsZ0Uh67L+86R5wuEI4x0UNRwzZcxGZ04YNtURVDvSMT+2/9NYcawbkQck/vvvTLrWalqlBwkPYPEoUtLBQP2Z6T7hld8kUTh4HrFmROSYrVyvaV+YHYOax2MbL7E+tGmDbWSnG+GbbmxLXs1uwAL6rXEB1IZp0E+KkFzeRPhiJ8eC7oQxcq/mIPeHpPNJGInTk769KK3Ik+TELDYCD606ETnYk67BNGngQ==</D></RSAKeyValue>";
        /// <inheritdoc/>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var audiences = new List<string>()
            {
                "development"
            };
            var manager = new JWTManager(NullLogger.Instance, "developer", audiences);
            var token = context.HttpContext.Request.Headers["Authorization"];

            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(DEV_KEY);

                try
                {
                    manager.ValidateToken(token, rsa.ExportParameters(true));
                }
                catch
                {
                    var data = new StatusCodeModel()
                    {
                        Status = 401,
                        Title = "Não autorizado"
                    };
                    context.Result = new UnauthorizedObjectResult(data);
                }
            }

        }
    }
}
