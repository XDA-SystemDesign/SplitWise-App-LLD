# Splitwise

- Installation:
  - Please install .NET 5 SDK (which also has runtime as well)
    - If possible please install VS2019 community edition as it'll automatically have .NET 5 SDK installed.
    - if not,
      - please manually install .NET 5 SDK (from this link: https://dotnet.microsoft.com/download/dotnet/5.0)
        - go inside the Folder "SplitWise-App" and run 
          - ```dotnet build SplitWise-App.csproj``` -> to build the startup
        - After this please run, 
          - ```dotnet run SplitWise-App.csproj``` -> to run the app.
            - you'll get some end points in the terminal (cmd) and you can access them in the browser
              - Also, you'll have OPEN API Specification document that has the relveant endpoints and how to contact them. You can access this by ```link/swagger/index.html``` in the browser. where ```link``` for example is: ```http://localhost:5001``` this link the end point that's been produced in the run
    - if you've visual studio installed, please open the app and right click on the ```SplitWise-App``` project and click on Debug. It starts a new instance in the browser and it's opens up Swagger UI (OPEN API)

- Some functionalities ignored due to time constraints:
    - No single expenses allowed i.e.. expenses are tightly coupled with groups (didn't check for this, I can't confirm it will work)
    - Didn't add validations during settlements (For example: If user A owes user B by 500, but user A pays 600 to user B). I couldn't write validations due to time constraints
    - Only 3 Splits Types are considered (Eq, Unequal, percentage)
    - Some validations might be missing because of time constraints (in all the endpoints)
    - We can't add an existing user to a group (couldn't do this due to time constraints)
      - But we can create Users during creation of a group

- Note: Some files are obselete but are still present. Some codecleanup must happen.

- Changes to the models:
    - Expense:
      - Expense has the group it's associated with it (as discussed)
      - Expense also has the User Expenditures List that's basically the initial payment done by a set of users (total = 5000 and A pays 2000, B pays 3000 for the team). These will be there in expenditure
      - Contribution -> This contribution is about how much each person contributes to the total payment done above: Decided by split criteria (SplitType enum): 
          - EQUAL = 1
          - UNEQUAL = 2
          - PERCENTAGE = 3
        - This contribution is about how much each person contributes to (note: if equal we divide by the entire group length and share it with all) 
      ```cs
        public class Expense
        {
            public int ExpenseId { get; set; }
            public Group group_associated_with_this { get; set; }
            public SplitType splitType { get; set; }
            public List<UserExpenditure> userExpenditures { get; set; }
            public List<UserContribution> userContributions { get; set; }
        }
      ```
      - Here the contribution is basically the split detail that we've discussed. And with out an expense, a split detail doesn't exists, so, I've coupled it with Expense
    - ExpenseSummary:
      - Prints the summary according to the expense for all the users in it (it will be a subset of the group or equal to the group)
    - Calculations:
      - In the design discussion, while I've told you that these will be stored in the DB, After I thought for sometime, I felt its not worth to write it in the DB. Example: if we need to calculate age, we don't store the age of the person (if we do that, then we need to run a cron job to update the DB every day which is not at all worth). So, calculations are gonna be done independently
      - This happens by Sorting via UserId and then Calculating how much each needs to pay
      - In the summary,
        - if there's a -ve amount, then that means the user must pay the corresponding amount
        ```json
        {
            "user": {
                "userid": 6,
                "name": "F"
            },
            "amount": -5200,
            "summaries": [
                {
                    "otherUser": {
                        "userid": 4,
                        "name": "D"
                    },
                    "money_to_transfer": -2700
                },
                {
                    "otherUser": {
                        "userid": 5,
                        "name": "E"
                    },
                    "money_to_transfer": -2500
                }
            ]
        }
        ```
        - In the above example:
            - User ID 6 (F) owes 5200 to all the others
              - he owes 2700 to User Id 4 (D)
              - he also owes 2500 to user ID 5 (E)
        - If it's +ve, that means he needs to get the money 
    - Settlements:
      - Settlements are inside a group (these don't have any relation with Expense class) as in Splitwise, Settlements happen at the group level. So, implemented that
