using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviePlatform.Domain.Common;

namespace MoviePlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
	protected readonly ISender Sender;

	protected ApiController(ISender sender)
	{
		Sender = sender;
	}

	protected Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

	protected IActionResult HandleFailure(Result result)
	{
		if (result.IsSuccess)
		{
			throw new InvalidOperationException("Cannot handle a successful result as a failure.");
		}

		int statusCode = result.Errors[0].Type switch
		{
			ErrorType.Failure => StatusCodes.Status422UnprocessableEntity,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
			ErrorType.Forbidden => StatusCodes.Status403Forbidden,
			_ => StatusCodes.Status500InternalServerError
		};

		return Problem(
			statusCode: statusCode,
			title: result.Errors[0].Code,
			detail: result.Errors[0].Description,
			extensions: new Dictionary<string, object?>
			{
				["errors"] = result.Errors.Select(error => new { error.Code, error.Description }).ToList()
			});
	}
}
