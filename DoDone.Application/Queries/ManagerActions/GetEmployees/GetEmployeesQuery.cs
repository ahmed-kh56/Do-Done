using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Dtos.UserDtos;
using ErrorOr;
using MediatR;


namespace DoDone.Application.Queries.ManagerActions.GetEmployees
{
    [Authorize(Role ="Manager")]
    public record GetEmployeesQuery(int BageNum,int BageSize) : IRequest<ErrorOr<IEnumerable<EmployeeProfileResult>>>;
}
