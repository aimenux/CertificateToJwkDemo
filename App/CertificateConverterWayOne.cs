using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace App;

public class CertificateConverterWayOne : ICertificateConverter
{
    private readonly IOptions<CertificateConverterSettings> _options;

    public CertificateConverterWayOne(IOptions<CertificateConverterSettings> options)
    {
        _options = options;
    }

    public string FromCertificateToJwk()
    {
        var certificateFile = _options.Value.CertificateFile;
        var certificatePassword = _options.Value.CertificatePassword;
        using var certificate = new X509Certificate2(certificateFile, certificatePassword);
        using var rsaKey = certificate.GetRSAPublicKey();
        if (rsaKey is null)
        {
            throw new InvalidOperationException("Unsupported certificate: public key is not RSA");
        }

        var securityAlgorithm = certificate.GetSecurityAlgorithm();
        var securityKey = new X509SecurityKey(certificate, _options.Value.KeyId);
        var jsonWebKey = JsonWebKeyConverter.ConvertFromX509SecurityKey(securityKey, true);
        jsonWebKey.Use ??= _options.Value.KeyUse;
        jsonWebKey.Alg ??= securityAlgorithm;

        var jwk = new
        {
            Kty = jsonWebKey.Kty,
            Use = jsonWebKey.Use,
            Alg = jsonWebKey.Alg,
            Kid = jsonWebKey.Kid,
            N = jsonWebKey.N,
            E = jsonWebKey.E
        };

        var json = JsonSerializer.Serialize(jwk);
        return json;
    }
}