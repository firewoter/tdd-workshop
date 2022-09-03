namespace TddWorkshop.Domain.InstantCredit;

public class CriminalRecordChecker : ICriminalRecordChecker
{
    public Task<bool> HasCriminalRecord(PersonalInfo record, CancellationToken cancellationToken) => 
        Task.FromResult(record.FirstName == "John" && record.LastName == "Snow");
}