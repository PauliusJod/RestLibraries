using System.ComponentModel.DataAnnotations;

namespace RestLibraries.Auth
{
    public record RegisterUserDto([Required] string UserName, [EmailAddress][Required] string Email, [Required] string Password);
    public record LoginDto(string UserName, string Password);
    public record LogoutDto(string UserName);
    public record UserDto(string Id, string UserName, string Email);

    public record SuccessfullLoginDto(string AccessToken);

    public record SuccessfulLoggedOutDto(string AccessToken);
}