using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Queries.Projects.GetEmployeesByProjectId
{
    public class GetEmployeesByProjectIdQueryHandler(IPURRepository _purRepository) : IRequestHandler<GetEmployeesByProjectIdQuery, ErrorOr<IEnumerable<ProjectEmployeeProfileResult>>>
    {
        public async Task<ErrorOr<IEnumerable<ProjectEmployeeProfileResult>>> Handle(GetEmployeesByProjectIdQuery request, CancellationToken cancellationToken)
        {


            var employees = await _purRepository.GetEmployeesByProjectId(request.ProjectId);
            return employees.ToList();


        }
    }
}
