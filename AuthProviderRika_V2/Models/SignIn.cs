using System.ComponentModel.DataAnnotations;

namespace AuthProviderRika_V2.Models;

public class SignIn
{
   
    public string Email { get; set; } = null!;

  
    public string Password { get; set; } = null!;

    
}
