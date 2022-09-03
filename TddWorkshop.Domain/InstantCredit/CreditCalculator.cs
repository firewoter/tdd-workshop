namespace TddWorkshop.Domain.InstantCredit;

internal static class CreditCalculator
{
    private static int GetAgePoints(int age, CreditInfo creditInfo)
    {
        if (age.Between(21, 28))
        {
            return creditInfo switch
            {
                { Sum: < 1_000_000 } => 12,
                { Sum: > 3_000_000 } => 0,
                _ => 9
            };
        }

        if (age.Between(29, 59))
        {
            return 14;
        }

        return creditInfo.Deposit == Deposit.None
            ? 0
            : 8;
    }

    private static int GetCriminalRecordPoints(bool hasCriminalRecord) => hasCriminalRecord
        ? 0
        : 15;

    private static int GetEmploymentPoints(Employment employment, int age) => employment switch
    {
        Employment.Employee => 14,
        Employment.SelfEmployed => 12,
        Employment.Freelancer => 8,
        Employment.Retired when age < 70 => 5,
        Employment.Retired => 0,
        Employment.Unemployed => 0,
        _ => throw new ArgumentOutOfRangeException(nameof(employment), employment, null)
    };

    private static int GetCreditGoalPoints(CreditGoal goal) => goal switch
    {
        CreditGoal.RealEstate => 8,
        CreditGoal.ConsumerCredit => 14,
        CreditGoal.OnLending => 12,
        _ => throw new ArgumentOutOfRangeException()
    };

    private static int GetOtherCreditPoints(bool hasOtherCredits, CreditGoal creditGoal)
    {
        if (!hasOtherCredits)
        {
            return creditGoal == CreditGoal.OnLending ? 0 : 15;
        }

        //throw new InvalidOperationException("Evil Exception");

        return 0;
    }

    public static CalculateCreditResponse Calculate(CalculateCreditRequest request, bool hasCriminalRecord)
    {
        var points = 0;
        points += GetAgePoints(request.PersonalInfo.Age, request.CreditInfo);
        points += GetEmploymentPoints(request.CreditInfo.Employment, request.PersonalInfo.Age);
        points += GetCreditGoalPoints(request.CreditInfo.Goal);
        points += GetDepositPoints(request.CreditInfo.Deposit);
        points += GetOtherCreditPoints(request.CreditInfo.HasOtherCredits, request.CreditInfo.Goal);
        points += GetSumPoints(request.CreditInfo.Sum);
        points += GetCriminalRecordPoints(hasCriminalRecord);
        
        return new CalculateCreditResponse(points);
    }

    private static int GetSumPoints(decimal sum) => sum switch
    {
        <= 1_000_000 => 12,
        <= 5_000_000 => 14,
        <= 10_000_000 => 8,
        _ => 0
    };

    private static int GetDepositPoints(Deposit deposit) => deposit switch
    {
        Deposit.Guarantor => 12,
        Deposit.RealEstate => 14,
        Deposit.OldCar => 3,
        Deposit.NewCar => 8,
        Deposit.None => 0,
        _ => throw new ArgumentOutOfRangeException()
    };
}

internal static class IntExtensions
{
    public static bool Between(this int src, int left, int right) => src >= left && src <= right;
}