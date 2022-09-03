using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using TddWorkshop.Domain.InstantCredit;
using TddWorkshop.Web.Controllers;

namespace TddWorkshop.Web.IntegrationTests.Base;

public class TddWorkshopWebApplicationFactory: WebApplicationFactory<HomeController>
{
    private HttpClient? _httpClient;
    
    private int _index = 1;

    public HttpClient HttpClient => _httpClient ??= CreateClient();

    public Mock<ICriminalRecordChecker> CriminalCheckerMock { get; } = new();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor =
                new ServiceDescriptor(
                    typeof(ICriminalRecordChecker),
                    _ => CriminalCheckerMock.Object,
                    ServiceLifetime.Singleton);

            services.Replace(descriptor);
        });
    }

    public void VerifyHasCriminalRecordOnce()
    {
        CriminalCheckerMock.Verify(x => x.HasCriminalRecord(
                It.IsAny<PersonalInfo>(), 
                It.IsAny<CancellationToken>()),
            Times.Exactly(_index++));
    }
}