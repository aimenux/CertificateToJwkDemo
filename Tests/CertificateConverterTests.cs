using App;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Tests
{
    public class CertificateConverterTests
    {
        private const string Jwk = "{\"Kty\":\"RSA\",\"Use\":\"Sig\",\"Alg\":\"RS256\",\"Kid\":\"1\",\"N\":\"rappCi51pun3a2EUTWIG9NW9LQyWyTcN3LlATYvlrUOauSPHTlqiKicHTk0OAFGCDWRVdIe68ObgS-mBqErAJwAl5dapIvK3JrMYQcW87s4N85VdXmt9JXkCVNff-uP3NSbVQpSNYCRdDTjMwPs4axEY14t3nvS72ibYlw55cfKIlK-1_1oP6cmkhb7FNDx8rqMX8GUFGKCkhwDN66cZwMRW2Kn70T7Z4YyQG3TgJ7bphh7uujo_w4EVeCvX-ftxFuHu3kcqCUiQI8lU_R9p5vpy42utWtG-BT5laKHkj5EJJqG8dpols1mLxlZJBg7A57DVcXnn77A9MHLsMlLU2Q\",\"E\":\"AQAB\"}";

        [Fact]
        public void GivenWayOneThenShouldConvertCertificateToJwk()
        {
            // arrange
            var options = Options.Create(new CertificateConverterSettings
            {
                CertificateFile = "Store/my-certificate.pfx",
                CertificatePassword = "my-certificate-pass"
            });

            var converter = new CertificateConverterWayOne(options);

            // act
            var jwk = converter.FromCertificateToJwk();

            // assert
            jwk.Should().NotBeNullOrEmpty();
            jwk.Should().Be(Jwk);
        }

        [Fact]
        public void GivenWayTwoThenShouldConvertCertificateToJwk()
        {
            // arrange
            var options = Options.Create(new CertificateConverterSettings
            {
                CertificateFile = "Store/my-certificate.pfx",
                CertificatePassword = "my-certificate-pass"
            });

            var converter = new CertificateConverterWayTwo(options);

            // act
            var jwk = converter.FromCertificateToJwk();

            // assert
            jwk.Should().NotBeNullOrEmpty();
            jwk.Should().Be(Jwk);
        }
    }
}