[![.NET](https://github.com/aimenux/CertificateToJwkDemo/actions/workflows/ci.yml/badge.svg)](https://github.com/aimenux/CertificateToJwkDemo/actions/workflows/ci.yml)

# CertificateToJwkDemo
```
Using various ways to generate jwk from pfx certificate file
```

In this demo, i m using two ways in order to generate jwk from pfx (or p12) certificate file
>
:one: `CertificateConverterWayOne` : use less code thanks to the class [JsonWebKeyConverter](https://docs.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.jsonwebkeyconverter)
>
:two: `CertificateConverterWayTwo` : use a little more code based on class [Base64UrlEncoder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.base64urlencoder)
>

In order to generate self signed certificate locally, type this command in your terminal :
>
> **dotnet dev-certs https -ep [path-to-certificate]/[certificate-name].pfx -p [certificate-password]**
>

```
|      Method | Categories |     Mean |    Error |   StdDev |      Min |      Max | Rank | Allocated |
|------------ |----------- |---------:|---------:|---------:|---------:|---------:|-----:|----------:|
| UsingWayOne |     WayOne | 52.40 ms | 4.332 ms | 12.77 ms | 30.59 ms | 81.95 ms |    1 |     12 KB |
|             |            |          |          |          |          |          |      |           |
| UsingWayTwo |     WayTwo | 47.36 ms | 4.650 ms | 13.71 ms | 29.88 ms | 70.02 ms |    1 |      9 KB |
```

>
**`Tools`** : vs22, net 6.0, xunit, fluent-assertion, benchmark-dotnet
