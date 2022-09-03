using MediatR;

namespace TddWorkshop.Domain.InstantCredit;

public class CalculateCreditHandler : IRequestHandler<CalculateCreditRequest, CalculateCreditResponse>
{

    private readonly ICriminalRecordChecker _criminalRecordChecker;

    public CalculateCreditHandler(ICriminalRecordChecker criminalRecordChecker)
    {
        _criminalRecordChecker = criminalRecordChecker;
    }

    public async Task<CalculateCreditResponse> Handle(CalculateCreditRequest request, CancellationToken cancellationToken)
    {
        var hasCriminalRecord = await _criminalRecordChecker.HasCriminalRecord(request.PersonalInfo, cancellationToken);
        
        return CreditCalculator.Calculate(request, hasCriminalRecord);
    }
}