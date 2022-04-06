using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace App;

public class CertificateConverterWayTwo : ICertificateConverter
{
    private readonly IOptions<CertificateConverterSettings> _options;

    public CertificateConverterWayTwo(IOptions<CertificateConverterSettings> options)
    {
        _options = options;
    }

    public string FromCertificateToJwk()
    {
        var certificateFile = _options.Value.CertificateFile;
        var certificatePassword = _options.Value.CertificatePassword;
        var certificate = new X509Certificate2(certificateFile, certificatePassword);
        var rsaKey = certificate.GetRSAPublicKey();
        if (rsaKey is null)
        {
            throw new InvalidOperationException("Unsupported certificate: public key is not RSA");
        }

        var securityAlgorithm = certificate.GetSecurityAlgorithm();
        var param = rsaKey.ExportParameters(false);
        var modulus = Base64UrlEncoder.Encode(param.Modulus);
        var exp = Base64UrlEncoder.Encode(param.Exponent);

        var jwk = new
        {
            Kty = JsonWebAlgorithmsKeyTypes.RSA,
            Use = _options.Value.KeyUse,
            Alg = securityAlgorithm,
            Kid = _options.Value.KeyId,
            N = modulus,
            E = exp
        };

        var json = JsonSerializer.Serialize(jwk);
        return json;
    }
}