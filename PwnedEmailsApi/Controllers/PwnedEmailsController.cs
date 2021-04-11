using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;


namespace PwnedEmailsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PwnedEmailsController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public PwnedEmailsController(IClusterClient clusterClient) => _clusterClient = clusterClient;

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var result = await _clusterClient.GetGrain<IDomainGrain>(ExtractDomain(email))
                .CheckEmailAddress(email);
            return result ? (IActionResult) Ok($"Email address {email} has been pwned") : NotFound($"Email address {email} is not on the list of pwned email addresses");
        }

        [HttpPost("{email}")]
        public async Task<IActionResult> Post(string email)
        {
            if (!IsValidEmail(email)) return BadRequest($"Email address {email} is not valid");
            var domain = ExtractDomain(email);
            var result = await _clusterClient.GetGrain<IDomainGrain>(domain)
                .AddEmailAddress(email);

            return result
                ? (IActionResult) Created(
                    $"Email address {email} has been added to the list of pwned emails list for domain {domain}",
                    email)
                : Conflict($"Email address {email} already exists in the pwned emails list");
        }

        private static string ExtractDomain(string email) => new MailAddress(email).Host;

        private static bool IsValidEmail(string email) => new EmailAddressAttribute().IsValid(email);
    }
}