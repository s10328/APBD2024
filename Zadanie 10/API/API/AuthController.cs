namespace API;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new IdentityUser { UserName = model.Username };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User registered successfully" });
    }
}

public class RegisterModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginModel model)
{
    var user = await _userManager.FindByNameAsync(model.Username);
    if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        return Unauthorized(new { message = "Invalid credentials" });

    var token = GenerateJwtToken(user);

    // Zapisz Refresh Token w bazie danych

    return Ok(new { token });
}

private string GenerateJwtToken(IdentityUser user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id) }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
[HttpPost("refresh-token")]
public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
{
    var principal = GetPrincipalFromExpiredToken(model.Token);
    var userId = principal.Claims.FirstOrDefault(c => c.Type == "id").Value;

    // Weryfikacja i odświeżenie tokenu

    var newToken = GenerateJwtToken(await _userManager.FindByIdAsync(userId));

    return Ok(new { token = newToken });
}

private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
{
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
        ValidateLifetime = false
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
    var jwtSecurityToken = securityToken as JwtSecurityToken;

    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        throw new SecurityTokenException("Invalid token");

    return principal;
}

public class RefreshTokenModel
{
    public string Token { get; set; }
}
