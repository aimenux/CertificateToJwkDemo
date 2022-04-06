using App;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Options;

namespace Benchs;

[Config(typeof(BenchConfig))]
public class CertificateConverterBench
{
    private static CertificateConverterWayOne _wayOne;
    private static CertificateConverterWayTwo _wayTwo;

    [GlobalSetup]
    public void Setup()
    {
        var options = Options.Create(GetSettings());
        _wayOne = new CertificateConverterWayOne(options);
        _wayTwo = new CertificateConverterWayTwo(options);
    }

    [Benchmark]
    [BenchmarkCategory(nameof(BenchCategory.WayOne))]
    public string UsingWayOne()
    {
        return _wayOne.FromCertificateToJwk();
    }

    [Benchmark]
    [BenchmarkCategory(nameof(BenchCategory.WayTwo))]
    public string UsingWayTwo()
    {
        return _wayTwo.FromCertificateToJwk();
    }

    private static CertificateConverterSettings GetSettings()
    {
        return new CertificateConverterSettings
        {
            CertificateFile = "Store/my-certificate.pfx",
            CertificatePassword = "my-certificate-pass"
        };
    }
}