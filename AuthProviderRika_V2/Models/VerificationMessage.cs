namespace AuthProviderRika_V2.Models;

public class VerificationMessage
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
}
