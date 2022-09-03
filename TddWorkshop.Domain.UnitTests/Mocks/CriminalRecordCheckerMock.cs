using System.Threading;
using System.Threading.Tasks;
using TddWorkshop.Domain.InstantCredit;

namespace TddWorkshop.Domain.Tests.Mocks;

public class CriminalRecordCheckerMock : ICriminalRecordChecker
{
    private readonly bool _hasRecord;
    
    public CriminalRecordCheckerMock(bool hasRecord)
    {
        _hasRecord = hasRecord;
    }
    public Task<bool> HasCriminalRecord(PersonalInfo record, CancellationToken cancellationToken)
    {
        return Task.FromResult(_hasRecord);
    }
}