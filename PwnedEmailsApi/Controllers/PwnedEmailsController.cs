using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.AspNetCore.Http;
using Orleans;
using PwnedEmailsApi.Utils;


namespace PwnedEmailsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PwnedEmailsController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private readonly string InvalidEmailMessage = "Email address is not valid";

        public PwnedEmailsController(IClusterClient clusterClient) => _clusterClient = clusterClient;

        /// <summary>
        /// Checks if the provided email is on the list of pwned emails 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///  GET /api/PwnedEmails/john.doe@gmail.com
        /// </remarks>
        /// <param name="email"></param>
        /// <returns>
        /// </returns>
        /// <response code="200">If email has been found on the pwned emails list</response>
        /// <response code="404">If email is not on the pwned emails list</response>
        /// <response code="400">If email is not valid</response>
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(ResponseBody), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string email)
        {
            if (!ControllerHelper.IsValidEmail(email))
            {
                return BadRequest(ControllerHelper.CreateProblemDetails(StatusCodes.Status400BadRequest,
                    InvalidEmailMessage, HttpContext.Request.Path));
            }

            var result = await _clusterClient.GetGrain<IDomainGrain>(ControllerHelper.ExtractDomain(email))
                .CheckEmailAddress(email);
            return result
                ? (IActionResult) Ok(ControllerHelper.CreateResponseBody(StatusCodes.Status200OK,
                    $"Email address {email} has been pwned"))
                : NotFound(ControllerHelper.CreateResponseBody(StatusCodes.Status404NotFound,
                    $"Email address {email} was not found on the list of pwned email addresses"));
        }

        /// <summary>
        /// Adds email to the pwned emails list
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///  POST /api/PwnedEmails/john.doe@gmail.com
        /// </remarks>
        /// <param name="email"></param>
        /// <returns>
        /// </returns>
        /// <response code="201">If email has been successfully added to the pwned emails list</response>
        /// <response code="409">If email already exists on the pwned emails list</response>
        /// <response code="400">If email is not valid</response>
        [HttpPost("{email}")]
        [ProducesResponseType(typeof(ResponseBody), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(string email)
        {
            if (!ControllerHelper.IsValidEmail(email))
            {
                return BadRequest(ControllerHelper.CreateProblemDetails(StatusCodes.Status400BadRequest,
                    InvalidEmailMessage, HttpContext.Request.Path));
            }

            var domain = ControllerHelper.ExtractDomain(email);
            var result = await _clusterClient.GetGrain<IDomainGrain>(domain)
                .AddEmailAddress(email);

            return result
                ? (IActionResult) Created(new Uri(HttpContext.Request.Path.ToString(), UriKind.Relative),
                    ControllerHelper.CreateResponseBody(StatusCodes.Status201Created,
                        $"Email address {email} has been added to the pwned emails list"))
                : Conflict(ControllerHelper.CreateProblemDetails(StatusCodes.Status409Conflict,
                    $"Email address {email} already exists in the pwned emails list", HttpContext.Request.Path));
        }
    }
}