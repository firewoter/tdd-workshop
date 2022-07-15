using System.Threading;
using AutoFixture.Xunit2;
using TddWorkshop.Domain.InstantCredit;
using TddWorkshop.Domain.Tests;
using TddWorkshop.Domain.Tests.Extensions;
using TddWorkshop.Web.E2ETests.Base;
using TddWorkshop.Web.E2ETests.Pages;
using Xunit;

namespace TddWorkshop.Web.E2ETests;

public class E2eTests: IClassFixture<WebDriverFixture>
{
    private readonly WebDriverFixture _fixture;

    public E2eTests(WebDriverFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void CreditCalculatorForm_SendMaximumForm_GetMinimalInterestRate()
    {
        var request = CreditCalculatorTestData.Maximum;
        _fixture.Refresh();
        var page = _fixture.CreatePage<FormPage>();
        var result = page.Submit(request);

        Assert.Equal("12.5", result.InterestRate.Text);
    }
}