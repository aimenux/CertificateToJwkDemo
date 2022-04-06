namespace App;

public class CertificateConverterSettings
{
    public string KeyId { get; set; } = "1";

    public string KeyUse { get; set; } = "Sig";

    public string CertificateFile { get; set; }

    public string CertificatePassword { get; set; }
}