using SplitWise_Models;
using SplitWise_Services.Expense;
using SplitWise_Services.Settlement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SplitWise_Services.Calculation
{
    public class CalculationService : ICalculationService
    {
        private ISettlementService settlementService;
        private IExpenseService expenseService;
        public CalculationService(ISettlementService settlementService, IExpenseService expenseService)
        {
            this.settlementService = settlementService;
            this.expenseService = expenseService;

        }

        private UserSummary mergeSummaries(UserSummary a, UserSummary b)
        {
            var output = new UserSummary { amount = a.amount + b.amount, user = a.user, summaries = new List<Summary>() };
            var commonSummaries = a.summaries.Select(s => s.OtherUser.Userid).Intersect(b.summaries.Select(s => s.OtherUser.Userid));
            var onlyASummaries = a.summaries.Select(s => s.OtherUser.Userid).Except(b.summaries.Select(s => s.OtherUser.Userid));
            var onlyBSummaries = b.summaries.Select(s => s.OtherUser.Userid).Except(a.summaries.Select(s => s.OtherUser.Userid));
            output.summaries.AddRange(a.summaries.Where(s => onlyASummaries.Any(aSummary => aSummary == s.OtherUser.Userid)));
            output.summaries.AddRange(b.summaries.Where(s => onlyBSummaries.Any(bSummary => bSummary == s.OtherUser.Userid)));

            foreach (var commonUser in commonSummaries)
            {
                var summay_from_a = a.summaries.FirstOrDefault(summary => summary.OtherUser.Userid == commonUser);
                var summay_from_b = b.summaries.FirstOrDefault(summary => summary.OtherUser.Userid == commonUser);
                output.summaries.Add(new Summary { OtherUser = summay_from_a.OtherUser, money_to_transfer = summay_from_a.money_to_transfer + summay_from_b.money_to_transfer });
            }
            return output;
        }
        private Dictionary<SplitWise_Models.User, UserSummary> merge(Dictionary<SplitWise_Models.User, UserSummary> a, Dictionary<SplitWise_Models.User, UserSummary> b)
        {
            var output = new Dictionary<SplitWise_Models.User, UserSummary>();

            foreach (var v in a)
            {
                var (user, summary) = v;
                if (b.ContainsKey(user))
                {
                    output[user] = mergeSummaries(summary, b[user]);
                } else output[user] = summary;
            }

            foreach (var v in b)
            {
                var (user, summary) = v;
                if (!a.ContainsKey(user)) output[user] = summary;
            }
            return output;
        }

        private void updateSummariesWithSettlements(SplitWise_Models.Settlement settlement, Dictionary<SplitWise_Models.User, UserSummary> summary)
        {
            var (from_user, to_user, amount) = (settlement.FromUser, settlement.ToUser, settlement.Amount_Settled);

            var summary_from_user = summary[from_user];
            var summary_to_user = summary[to_user];
            if (summary_from_user.summaries.Any(summary => summary.OtherUser == to_user))
            {
                var summary_from_from_user = summary_from_user.summaries.FirstOrDefault(summary => summary.OtherUser == to_user);
                var summary_from_to_user = summary_to_user.summaries.FirstOrDefault(summary => summary.OtherUser == from_user);

                summary_from_from_user.money_to_transfer += amount;
                summary_from_to_user.money_to_transfer -= amount;
                summary_from_user.amount += amount;
                summary_to_user.amount -= amount;
            }
        }

        public Dictionary<SplitWise_Models.User, UserSummary> calculateSummaryForAGroup(SplitWise_Models.Group group, SplitWise_Models.User user)
        {
            var allExpenses_related_to_the_user = expenseService.GetAllExpensesRelatedToUser(user);
            var allSettlements_related_to_the_user = settlementService.GetAllSettlementsRelatedToUser(user, group);

            Dictionary<SplitWise_Models.User, UserSummary> merged_dictionary = null;

            foreach (var expense in allExpenses_related_to_the_user)
            {
                var summary_of_the_expense = calculateSummaryBasedOnExpense(expense);
                if (merged_dictionary is null) merged_dictionary = summary_of_the_expense;
                else merged_dictionary = merge(merged_dictionary, summary_of_the_expense);
            }
            foreach (var settlement in allSettlements_related_to_the_user)
            {
                updateSummariesWithSettlements(settlement, merged_dictionary);
            }
            return merged_dictionary;
        }

        private Dictionary<SplitWise_Models.User, UserSummary> calculateSummaryBasedOnExpense(SplitWise_Models.Expense expense)
        {
            SortedList<int, double> total_paid = new SortedList<int, double>();
            SortedList<int, double> total_to_pay = new SortedList<int, double>();
            foreach (var expended in expense.userExpenditures)
            {
                var (userId, user_paid) = (expended.UserId, expended.how_much_the_user_spent_on_the_group);
                total_paid[userId] = user_paid;
            }
            var group = expense.group_associated_with_this;
            foreach (var contribution in expense.userContributions)
            {
                var (userId, user_to_pay) = (contribution.UserId, contribution.Contribution);
                if (total_paid.ContainsKey(userId))
                {
                    var min_val = Math.Min(total_paid[userId], user_to_pay);
                    if (min_val == total_paid[userId])
                    {
                        total_paid.Remove(userId);
                        user_to_pay -= min_val;
                    }
                    else
                    {
                        total_paid[userId] -= min_val;
                        user_to_pay = 0;
                    }
                }
                if (user_to_pay != 0)
                {
                    total_to_pay[userId] = user_to_pay;
                }
            }
            var (m, n) = (total_paid.Count, total_to_pay.Count);
            var (i, j) = (0, 0);

            var (paid_users, to_pay_users) = (total_paid.Keys, total_to_pay.Keys);
            Dictionary<SplitWise_Models.User, UserSummary> summaries = new Dictionary<SplitWise_Models.User, UserSummary>();

            foreach (var v in total_paid)
            {
                var (userId, total_to_recieve) = v;
                var user = group.users.FirstOrDefault(u => u.Userid == userId);
                summaries[user] = new UserSummary { amount = total_to_recieve, user = user, summaries = new List<Summary>() };
            }

            foreach (var v in total_to_pay)
            {
                var (userId, total_to_be_paid) = v;
                var user = group.users.FirstOrDefault(u => u.Userid == userId);
                summaries[user] = new UserSummary { amount = -total_to_be_paid, user = user, summaries = new List<Summary>() };
            }


            while (i < m && j < n)
            {

                var (already_paid_user, to_pay_user) = (group.users.FirstOrDefault(u => u.Userid == paid_users[i]),
                    group.users.FirstOrDefault(u => u.Userid == to_pay_users[j]));
                var (amount_to_recieve, amount_to_pay) = (total_paid[already_paid_user.Userid], total_to_pay[to_pay_user.Userid]);
                if (amount_to_recieve == 0)
                {
                    i++;
                    continue;
                }
                if (amount_to_pay == 0)
                {
                    j++; continue;
                }
                var min_val = Math.Min(amount_to_pay, amount_to_recieve);
                
                summaries[already_paid_user].summaries.Add(new Summary { OtherUser = to_pay_user, money_to_transfer = min_val });
                summaries[to_pay_user].summaries.Add(new Summary { OtherUser = already_paid_user, money_to_transfer = -min_val });
                if (min_val == amount_to_recieve)
                {
                    total_paid[already_paid_user.Userid] = 0;
                    i++;
                }
                else
                {
                    total_paid[already_paid_user.Userid] -= min_val;
                    j++;
                }
            }
            
            return summaries;
        }

        
        //public GroupSummary calculateSummaryForAGroup(SplitWise_Models.Group group, SplitWise_Models.User user)
        //{
        //    var allExpenses_related_to_the_user = expenseService.GetAllExpensesRelatedToUser(user);
        //    var allSettlements_related_to_the_user = settlementService.GetAllSettlementsRelatedToUser(user, group);

        //    SortedList<int, double> total_paid = new SortedList<int, double>();
        //    SortedList<int, double> total_to_pay = new SortedList<int, double>();
        //    foreach (var expense in allExpenses_related_to_the_user)
        //    {
        //        foreach (var expended in expense.userExpenditures)
        //        {
        //            var (userId, user_paid) = (expended.UserId, expended.how_much_the_user_spent_on_the_group);
        //            if (total_paid.ContainsKey(userId))
        //            {
        //                total_paid[userId] += user_paid;
        //            }
        //            else
        //            {
        //                total_paid[userId] = user_paid;
        //            }
        //        }
        //    }

        //    foreach (var expense in allExpenses_related_to_the_user)
        //    {
        //        foreach (var contribution in expense.userContributions)
        //        {
        //            var (userId, user_to_pay) = (contribution.UserId, contribution.Contribution);
        //            if (total_paid.ContainsKey(userId))
        //            {
        //                var min_val = Math.Min(total_paid[userId], user_to_pay);
        //                if (min_val == total_paid[userId])
        //                {
        //                    total_paid.Remove(userId);
        //                    user_to_pay -= min_val;
        //                }
        //                else
        //                {
        //                    total_paid[userId] -= min_val;
        //                    user_to_pay = 0;
        //                }
        //            }
        //            else if (user_to_pay != 0)
        //            {
        //                if (total_to_pay.ContainsKey(userId))
        //                    total_to_pay[userId] += user_to_pay;
        //                else total_to_pay[userId] = user_to_pay;
        //            }
        //        }
        //    }

        //    foreach (var settlement in allSettlements_related_to_the_user)
        //    {
        //        try
        //        {
        //            total_paid[settlement.ToUser.Userid] -= settlement.Amount_Settled;
        //            total_to_pay[settlement.FromUser.Userid] -= settlement.Amount_Settled;
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }

        //    var (m, n) = (total_paid.Count, total_to_pay.Count);
        //    var (i, j) = (0, 0);

        //    var (paid_users, to_pay_users) = (total_paid.Keys, total_to_pay.Keys);
        //    Dictionary<SplitWise_Models.User, UserSummary> summaries = new Dictionary<SplitWise_Models.User, UserSummary>();
        //    while (i < m && j < n)
        //    {

        //    }
        //}
    }
}
