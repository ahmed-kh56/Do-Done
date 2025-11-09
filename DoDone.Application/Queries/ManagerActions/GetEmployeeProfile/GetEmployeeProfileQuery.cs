using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Dtos.UserDtos;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace DoDone.Application.Queries.ManagerActions.GetEmployeeProfile;

[Authorize(Role = "Manager")]
public record GetEmployeeProfileQuery(Guid UserId):IRequest<ErrorOr<EmployeeProfileResult>>;

public class GetEmployeeProfileQueryValidator:AbstractValidator<GetEmployeeProfileQuery>
{
    public GetEmployeeProfileQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
