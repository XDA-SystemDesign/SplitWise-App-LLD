using SplitWise_Business.DTOs;
using SplitWise_Business.Exceptions;
using SplitWise_Models;
using SplitWise_Services.Expense;
using SplitWise_Services.Group;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SplitWise_Business.Validations
{
    public class ExpenseValidationService: IExpenseValidationService
    {
        private IGroupService groupService;
        private IExpenseService expenseService;
        public ExpenseValidationService(IGroupService groupService, IExpenseService expenseService)
        {
            this.groupService = groupService;
            this.expenseService = expenseService;
        }

        public Expense createExpense(ExpenseRequestDTO expenseRequest)
        {
            var group = this.groupService.GetGroupByGroupId(expenseRequest.GroupId);
            Validate(expenseRequest, group);
            //createContributionsIfNotPresent(group, ref expenseRequest);
            Expense e = new Expense
            {
                //group_associated_with_this = group,
                splitType = expenseRequest.SplitType,
                userContributions = expenseRequest.userContributions,
                userExpenditures = expenseRequest.userExpenditures
            };
            return expenseService.CreateExpense(e, group);
        }

        public IEnumerable<ExpenseSummary> GetExpenseSummary(int id)
        {
            if (id <= 0) throw new ValidationException("Invalid ID Passed");
            return this.expenseService.calculateSummaryForAnExpense(id);
        }
        public bool Validate(ExpenseRequestDTO expenseRequest, Group group)
        {
            //return false;
            
            if (group is null)
            {
                throw new ValidationException("Group doesn't exists");
            }

            var total_money_spent_on_the_group = expenseRequest.userExpenditures.Sum(e => e.how_much_the_user_spent_on_the_group);
            if (total_money_spent_on_the_group <= 0) throw new ValidationException("Total money spent must be > 0");
            var areAnyVes = expenseRequest.userExpenditures.Any(e => e.how_much_the_user_spent_on_the_group <= 0);
            areAnyVes = areAnyVes  || expenseRequest.userContributions.Any(e => e.Contribution <= 0);
            if (areAnyVes) throw new ValidationException("-ve numbers can't be provided");
            var result = expenseRequest.userExpenditures.Where(spent_user => !group.users.Any(user_in_group => user_in_group.Userid == spent_user.UserId));
            if (result.Any())
            {
                throw new ValidationException("Spent user doesn't exists in the group");
            }
            switch (expenseRequest.SplitType)
            {
                case SplitWise_Constants.Enums.SplitType.UNEQUAL or SplitWise_Constants.Enums.SplitType.PERCENTAGE:
                    if (!expenseRequest.userContributions.Any()) 
                        throw new ValidationException($"Please provide the relevant data for {expenseRequest.SplitType.ToString()}");
                    if (expenseRequest.userContributions.Any(spent_for_user => !group.users.Any(user_in_group => user_in_group.Userid == spent_for_user.UserId)))
                        throw new ValidationException($"Please add the users to a group before creating the expenses");
                    if (expenseRequest.SplitType == SplitWise_Constants.Enums.SplitType.PERCENTAGE)
                    {
                        var total_percentage = expenseRequest.userContributions.Sum(e => e.Contribution);
                        if ((int)total_percentage != 100) throw new ValidationException("Total contribution is not 100%");
                    } else
                    {
                        if (expenseRequest.userContributions.Sum(e => e.Contribution) != total_money_spent_on_the_group)
                            throw new ValidationException("Total is not equal");
                    }
                    break;
                    
                case SplitWise_Constants.Enums.SplitType.EQUAL:
                    if (expenseRequest.userContributions.Any() && expenseRequest.userContributions.GroupBy(v => v.Contribution).Count() > 1)
                    {
                        throw new ValidationException($"Unequal values've been provided");
                    }
                    break;
            }
            
            return true;
        }

    }
}
