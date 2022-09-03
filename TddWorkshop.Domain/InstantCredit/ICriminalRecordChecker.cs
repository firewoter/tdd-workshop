namespace TddWorkshop.Domain.InstantCredit;

public interface ICriminalRecordChecker
{
    Task<bool> HasCriminalRecord(PersonalInfo record, CancellationToken cancellationToken);
}