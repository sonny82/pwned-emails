using System;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces;
using PwnedEmailsTests.Cluster;
using PwnedEmailsTests.Fakes;
using Xunit;

namespace PwnedEmailsTests.Grains
{
    [Collection(nameof(ClusterCollection))]
    public class DomainGrainTest
    {
        private readonly ClusterFixture _fixture;
        private const string Domain = "domain";
        private const string Email = "email1@gmail.com";

        public DomainGrainTest(ClusterFixture fixture) => this._fixture = fixture;

        private string GenerateDomainName() => Domain + Guid.NewGuid();

        [Fact]
        public async Task ShouldRegisterTimerOnActivation()
        {
            var grain = _fixture.Cluster.GrainFactory.GetGrain<IDomainGrain>(GenerateDomainName());
            await grain.CheckEmailAddress(Email);

            var timerEntries = _fixture.GetTimers(grain).ToList();
            Assert.Single(timerEntries);

            FakeTimerEntry timer = timerEntries.First();
            Assert.Equal(TimeSpan.FromMinutes(5), timer.DuePeriod);
            Assert.Equal(TimeSpan.FromMinutes(0), timer.DueTime);
        }

        [Fact]
        public async Task TestCheckEmailAddress_ShouldReturnFalseIfEmailIsNotOnTheList()
        {
            var grain = _fixture.Cluster.GrainFactory.GetGrain<IDomainGrain>(GenerateDomainName());
            Assert.False(await grain.CheckEmailAddress(Email));
        }

        [Fact]
        public async Task TestCheckEmailAddress_ShouldReturnTrueIfEmailIsOnTheList()
        {
            var grain = _fixture.Cluster.GrainFactory.GetGrain<IDomainGrain>(GenerateDomainName());
            Assert.False(await grain.CheckEmailAddress(Email));
            Assert.True(await grain.AddEmailAddress(Email));
            Assert.True(await grain.CheckEmailAddress(Email));
        }

        [Fact]
        public async Task TestAddEmailAddress_ShouldReturnTrueIfEmailIsAddedToTheList()
        {
            var grain = _fixture.Cluster.GrainFactory.GetGrain<IDomainGrain>(GenerateDomainName());
            Assert.False(await grain.CheckEmailAddress(Email));
            Assert.True(await grain.AddEmailAddress(Email));
        }

        [Fact]
        public async Task TestAddEmailAddress_ShouldReturnFalseIfEmailIsAlreadyOnTheList()
        {
            var grain = _fixture.Cluster.GrainFactory.GetGrain<IDomainGrain>(GenerateDomainName());
            Assert.False(await grain.CheckEmailAddress(Email));
            Assert.True(await grain.AddEmailAddress(Email));
            Assert.False(await grain.AddEmailAddress(Email));
        }
    }
}