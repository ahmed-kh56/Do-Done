using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Queries.ManagerActions.GetEmployees
{
    public class GetEmployeesQueryHandler(IUsersRepository _userRepository) : IRequestHandler<GetEmployeesQuery, ErrorOr<IEnumerable<EmployeeProfileResult>>>
    {
        public async Task<ErrorOr<IEnumerable<EmployeeProfileResult>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {

                var employees = await _userRepository.GetEmployees(request.BageNum,request.BageSize);
                return employees.ToList();
        }
    }
}
