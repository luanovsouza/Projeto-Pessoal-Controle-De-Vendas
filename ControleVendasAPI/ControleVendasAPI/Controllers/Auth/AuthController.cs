using System.IdentityModel.Tokens.Jwt;
using ControleVendasAPI.DTOS;
using ControleVendasAPI.Models;
using ControleVendasAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(UserManager<UserToken> userManager, SignInManager<UserToken> signInManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }


    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);

        try
        {
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        message = $"Role '{roleName}' criada com sucesso!"
                    });
                }

                return BadRequest($"Ocorreu um erro ao criar a role '{roleName}'!");
            }

            return BadRequest(new
            {
                ErroMessage = $"Role '{roleName}' já existe!"
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    [HttpPost("CreateUserToRole")]
    public async Task<IActionResult> CreateUserToRole(string userName, string roleName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user is null)
        {
            return NotFound(new
            {
                ErroMessage = "Usuario não encontrado!"
            });
        }
        
        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (result.Succeeded)
        {
            return Ok(new
            {
                Message = $"Role '{roleName}' atribuida ao usuario '{userName}' com sucesso!"
            });
        }

        return BadRequest(new
        {
            ErroMessage = $"Ocorreu um erro ao atribuir a role '{roleName}' ao usuario '{userName}'!"
        });
    }
    
    [HttpPost("register")]
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


    [HttpPost("login")]
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

        //Criando o Refresh token no meu banco de dados
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            Mensagem = "Usuario logado com sucesso",
            Token = token
        });
    }



    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenUserDto tokenUserDto)
    {
        if (tokenUserDto is null)
            return BadRequest("Invalid client request");

        var acessToken = tokenUserDto.Token; //Pegando o token de acesso do cliente
        var refreshToken = tokenUserDto.RefreshToken; //Pegando o refresh token do cliente

        var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!);
        //Chamando o método para obter as claims do token de acesso expirado, para validar o refresh token
        //e gerar um novo token de acesso

        if (principal is null)
            return BadRequest();

        var username = principal.Identity!.Name;
        // Buscando o nome do usuario nas claims

        var user = await _userManager.FindByNameAsync(username!);
        // Buscando o usuario no banco de dados

        if (user is null || user.RefreshToken != refreshToken ||
            user.Expiracao <= DateTime.UtcNow)
            return BadRequest("Invalid client request");

        var newAcessToken = _tokenService.GerarToken(user);
        var newRefreshToken = _tokenService.GerarRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            Token = newAcessToken,
            RefreshToken = newRefreshToken
        });


    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByEmailAsync(username);

        if (user is null)
            return BadRequest("Usuario não encontrado!");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }
}