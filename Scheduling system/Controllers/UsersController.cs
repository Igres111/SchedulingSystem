using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Queries.GetJobs;

namespace SchedulingSystem.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns the job assigned to the authenticated user.
    /// </summary>
    /// <response code="200">Job details retrieved successfully.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    [Authorize]
    [HttpGet("job")]
    [ProducesResponseType(typeof(UserJobResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserJob(CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new GetJobsQuery(), cancellationToken);

        return Ok(response);
    }
}
