using MediatR;
using SchedulingSystem.Application.Dtos.Responses;

namespace SchedulingSystem.Application.Queries.GetJobs;

public record GetJobsQuery() : IRequest<List<UserJobResponse>>;
