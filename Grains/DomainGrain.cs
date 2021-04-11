using System;
using System.Globalization;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace Grains
{
    public class DomainGrain : Grain, IDomainGrain
    {
        private readonly ILogger<DomainGrain> _logger;
        private readonly IPersistentState<DomainGrainState> _domainState;

        public DomainGrain([PersistentState("domain")] IPersistentState<DomainGrainState> domainState,
            ILogger<DomainGrain> logger)
        {
            _logger = logger;
            _domainState = domainState;
        }

        public override async Task OnActivateAsync()
        {
            RegisterTimer(WriteStateAsync, null, TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(5));
            await base.OnActivateAsync();
            _logger.Trace(
                $"{DateTime.Now.ToString(CultureInfo.CurrentCulture)}: Grain {IdentityString} has been activated");
        }

        public Task<bool> AddEmailAddress(string email)
        {
            _logger.Trace(
                $"{DateTime.Now.ToString(CultureInfo.CurrentCulture)}: Adding email address {email} to the list of pwned emails");
            return Task.FromResult(_domainState.State.EmailAddresses.Add(email));
        }

        public Task<bool> CheckEmailAddress(string email)
        {
            _logger.Trace(
                $"{DateTime.Now.ToString(CultureInfo.CurrentCulture)}: Checking if email address {email} is on the list of pwned emails");
            return _domainState.State.EmailAddresses.Contains(email) ? Task.FromResult(true) : Task.FromResult(false);
        }

        private async Task WriteStateAsync(object input)
        {
            _logger.Trace(
                $"{DateTime.Now.ToString(CultureInfo.CurrentCulture)}: Timer activated: writing state for grain {IdentityString} asynchronously...");
            await _domainState.WriteStateAsync();
        }
    }
}