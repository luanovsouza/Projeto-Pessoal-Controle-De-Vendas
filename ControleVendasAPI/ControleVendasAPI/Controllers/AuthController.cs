using ControleVendasAPI.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] LoginDto dto)
    {   
        //Criando o objeto do usuario
        var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
        
        //Criando o usuario no banco de dados
        var newUser = await _userManager.CreateAsync(user, dto.Senha!);

        if (!newUser.Succeeded)
        {
            return BadRequest(newUser.Errors);
        }

        return Ok("Usuario registrado com sucesso!");
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email!);

        if (user is null)
        {
            return BadRequest("Email Incorreto!");
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Senha!, false).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            return BadRequest($"Senha incorreta!");
        }

        return Ok("Usuario logado com sucesso, divitar-se!");
    }
}