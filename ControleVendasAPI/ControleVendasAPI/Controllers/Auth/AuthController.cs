using ControleVendasAPI.DTOS;
using ControleVendasAPI.Models;
using ControleVendasAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendasAPI.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserToken> _userManager;
    private readonly SignInManager<UserToken> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<UserToken> userManager, SignInManager<UserToken> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] LoginDto dto)
    {   
        //Criando o objeto do usuario
        var user = new UserToken { UserName = dto.Email, Email = dto.Email };
        
        //Criando o usuario no banco de dados
        var newUser = await _userManager.CreateAsync(user, dto.Senha!);

        if (!newUser.Succeeded)
        {
            return BadRequest(newUser.Errors);
        }

        return Ok(new
        {
            Mensagem = "Usuario registrado com sucesso",
        });
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

        var token = _tokenService.GerarToken(user);
        var refreshToken = _tokenService.GerarRefreshToken();

        return Ok(new
        {
            Mensagem = "Usuario logado com sucesso",
            Token = token,
        });
    }
}