using SplitWise_Business.DTOs;

namespace SplitWise_Business.Validations
{
    public interface ISettlementValidationService
    {
        void createASettlementRecord(SettlementDTO settlement);
    }
}