# Financial Tracker Backend

The server-side application for my Financial Tracker app.

Challenge: Design a web service using .NET Framework.

Context: For the final project in my Web Services class, I decided to design the backend for an application that tracks a user's expenses and incomes, providing a running balance and the ability to specify date ranges to retrieve financial info at specific points in time. I created a .NET Core web API application using a SQL Server. In order to provide authentication/authorization, I used the .NET Identity framework.

Action: I creating a User class having properties for first and last name. Initially I used an int for the Id but later switched to using a Guid in order to easily extend IdentityUser from .NET Identity. I also created Expense and Income models containing a user's respective incomes and expenses. These three classes would be the only tables I would create (not including Identity tables) in the SQL Server database. A fourth class I created was a FinancialSummary class that would perform the logic for maintaining balances, holding lists of both Income and Expense. This class would not have its own table in the database, instead it would be a complex class that would only retrieve Incomes and expenses to perform logic on since the calculated values do not need to be stored. So in my DbContext within the OnModelCreating method, a User 'owns' (instead of has) one FinancialSummary, and a FinancialSummary owns many Incomes and Expenses.

In my UserController class, I specified several action methods ranging from basic CRUD operations to retrieving incomes and expenses based on date ranges as well as adding and editing them.

Result: The biggest challenge for this application was understanding and implementing .NET Identity. I watched several videos and researched the ability to create a custom User that inherits from IdentityUser in order to utilize all the identity tables that hold users' email, password hash, API key, bearer tokens, and many other authentication-related fields. What ulitimately made things much easier was to just remove the int Id I was using in my User class in order to just use the inherited Guid that exists in IdentityUser. After making this adjustment and specifying the appropriate configurations in Program.cs, the app ran as intended.

Later on, I added a Transaction model to be able to conveniently display Incomes and Expenses in the same collection. I also added a separate CustomIdentityController to handle specific lockout functionality in the event a user enters the wrong password more than 5 times.

Reflection: I really enjoyed designing this API and thinking hard about what was necessary for inclusion in the database. Even though I spent days getting my custom User to properly inherit from IdentityUser, the reward came when I was able to maintain all the functionality I created in my UserController and have .NET Identity work on its own in the background.

## Screenshot of Swagger UI showing Identity endpoints

![image](https://github.com/claytonius30/finance_backend/assets/116747177/5ca6aca8-6681-4d9a-83c2-6490077d7d17)


## Screenshot showing some User endpoints

![image](https://github.com/claytonius30/finance_backend/assets/116747177/cb0c2bef-a359-4b5d-8b60-7e88905968a2)

