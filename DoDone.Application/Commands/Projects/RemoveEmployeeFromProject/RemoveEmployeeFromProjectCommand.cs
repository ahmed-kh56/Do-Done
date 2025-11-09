using ErrorOr;
using MediatR;

namespace DoDone.Application.Commands.Projects.RemoveEmployeeFromProject;

public record RemoveEmployeeFromProjectCommand(Guid purId) : IRequest<ErrorOr<Success>>;
