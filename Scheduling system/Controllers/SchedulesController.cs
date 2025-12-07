using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchedulingSystem.Application.Commands.CreateSchedule;
using SchedulingSystem.Application.Commands.UpdateScheduleStatus;
using SchedulingSystem.Application.Dtos.Requests;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Queries.GetSchedules;
using System.Security.Claims;

namespace SchedulingSystem.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchedulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Updates the status of a schedule (Admin only).
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /api/schedules/{id}/status
    ///     {
    ///         "status": "Approved"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Schedule status updated.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">User is not authorized to update schedules.</response>
    /// <response code="404">Schedule not found.</response>
    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(ScheduleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateScheduleStatus(Guid id, [FromBody] UpdateScheduleStatusRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new UpdateScheduleStatusCommand(id, request.Status), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new schedule for the authenticated user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/schedules
    ///     {
    ///         "jobId": "00000000-0000-0000-0000-000000000000",
    ///         "date": "2025-12-31"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Schedule created successfully.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
        {
            return Unauthorized("Invalid or missing user ID in the token.");
        }

        var command = new CreateScheduleCommand(request.JobId, Guid.Parse(userIdClaim), request.Date);

        await _mediator.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Returns paginated schedules for the authenticated user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/schedules?period=week&pageNumber=1&pageSize=3
    ///
    /// </remarks>
    /// <response code="200">Schedules retrieved successfully.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ScheduleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSchedules([FromQuery] GetSchedulesRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetSchedulesQuery(
               request.Period,
               request.PageNumber,
               request.PageSize);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
