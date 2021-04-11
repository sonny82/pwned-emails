using System;
using GrainInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Orleans;
using PwnedEmailsApi.Controllers;

namespace PwnedEmailsTests.Controllers
{
    public class PwnedEmailsControllerTestBase : IDisposable
    {
        internal const string Domain = "gmail.com";
        internal const string Email = "test@gmail.com";
        internal Mock<IClusterClient> ClusterClient;
        internal Mock<IDomainGrain> MockGrain;
        internal ControllerContext ControllerContext;
        internal PwnedEmailsController Controller;

        public PwnedEmailsControllerTestBase()
        {
            ClusterClient = new Mock<IClusterClient>();
            MockGrain = new Mock<IDomainGrain>();

            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            Controller = new PwnedEmailsController(ClusterClient.Object)
            {
                ControllerContext = ControllerContext
            };

            ClusterClient.Setup(c => c.GetGrain<IDomainGrain>(Domain, null)).Returns(MockGrain.Object);
        }

        public void Dispose()
        {
          GC.SuppressFinalize(this);
        }
    }
}