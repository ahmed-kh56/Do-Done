using DoDone.Authentication;
using DoDone.Application.Authentication.Commands.ConfirmEmail;
using DoDone.Application.Authentication.Commands.CreateToken;
using DoDone.Application.Authentication.Commands.PasswordReset;
using DoDone.Application.Authentication.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoDone.Controllers;
[Route("[controller]")]
[AllowAnonymous]
public class AuthenticationController: ApiController
{


    private readonly ISender _mediator;

    public AuthenticationController(ISender sender)
    {
        _mediator = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = request.ToCommand();


        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(authResult.FromAuthenticationResult()),
            Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);

        var authResult = await _mediator.Send(query);

        return authResult.Match(
            authResult => Ok(authResult.FromAuthenticationResult()),
            Problem);
    }
    [HttpPost("Tokens")]
    public async Task<IActionResult> CreateToken(CreateTokenRequest request)
    {
        var command = new CreateTokenCommand(request.Email, request.Type);

        var Result = await _mediator.Send(command);

        return Result.Match(
            Result => Ok(),
            Problem);


    }
    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
    {
        var command = new ConfirmEmailCommand(request.Email, request.Token,request.UserId);

        var Result = await _mediator.Send(command);

        return Result.Match(
            (Result) => Ok(),
            Problem);
    }
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var command = new ResetPasswordCommand(request.Email,request.Token,request.NewPassword);
        
        var Result = await _mediator.Send(command);

        return Result.Match(
            Result => Ok(Result.FromAuthenticationResult()),
            Problem);


    }


}