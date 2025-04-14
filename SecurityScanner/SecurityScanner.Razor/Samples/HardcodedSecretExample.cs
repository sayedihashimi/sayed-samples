namespace SecurityScanner.Razor.Samples;
public class HardcodedSecretExample
{
    // SECURITY ISSUE: Hardcoded secret
    private const string SecretApiKey = "sk_test_1234567890";
}
