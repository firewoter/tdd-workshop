using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using TddWorkshop.Domain.InstantCredit;
using TddWorkshop.Domain.Tests;
using TddWorkshop.Web.IntegrationTests.Base;
using Xunit;

namespace TddWorkshop.Web.IntegrationTests;

public class IntegrationTests: IClassFixture<TddWorkshopWebApplicationFactory>
{
    private readonly TddWorkshopWebApplicationFactory _factory;

    public IntegrationTests(TddWorkshopWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Theory, ClassData(typeof(CreditCalculatorTestData))]
    public async Task Calculate_CreditApproved_PointsCalculatedCorrectly(CalculateCreditRequest request, 
        bool hasCriminalRecord, int points)
    {
        _factory.CriminalCheckerMock
            .Setup(x => x.HasCriminalRecord(It.IsAny<PersonalInfo>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(hasCriminalRecord);
        
        var res = await _factory
            .HttpClient
            .PostAsync("Calculator/Calculate", JsonContent.Create(request));
        
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        var content = await res.Content.ReadFromJsonAsync<CalculateCreditResponse>();
        
        Assert.Equal(points, content?.Points);
        //_factory.VerifyHasCriminalRecordOnce();
    }

    [Fact(Skip = "Implement on Step 4")]
    public async Task Calculate_WrongInput_ValidationFailed()
    {
        throw new NotImplementedException();
    }
}