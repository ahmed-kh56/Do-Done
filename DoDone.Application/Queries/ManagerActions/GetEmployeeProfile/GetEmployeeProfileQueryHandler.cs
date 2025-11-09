using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Queries.ManagerActions.GetEmployeeProfile;

public class GetEmployeeProfileQueryHandler
    (IUsersRepository _usersRepository)
    : IRequestHandler<GetEmployeeProfileQuery, ErrorOr<EmployeeProfileResult>>
{
    public async Task<ErrorOr<EmployeeProfileResult>> Handle(GetEmployeeProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _usersRepository.GetEmployeeProfileById(request.UserId);

        if (result == null)
        {
            return Error.NotFound(description: "Employee profile not found");
        }
        return result;

    }
}