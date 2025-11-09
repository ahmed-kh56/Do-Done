using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Domain.Projects;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Commands.Projects.StartProject;
[Authorize(Role ="Manager")]
public record StartProjectCommand(string Name,bool IsStarted,DateTime? StartDate):IRequest<ErrorOr<Project>>;
