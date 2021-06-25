using SplitWise_Models;
using System.Collections.Generic;

namespace SplitWise_Services.Calculation
{
    public interface ICalculationService
    {
        Dictionary<SplitWise_Models.User, UserSummary> calculateSummaryForAGroup(SplitWise_Models.Group group, SplitWise_Models.User user);
    }
}