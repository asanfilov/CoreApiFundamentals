# About
Building an API with ASP.NET Core course on Pluralsight
https://app.pluralsight.com/library/courses/building-api-aspdotnet-core

# Tools required
* Microsoft Visual Studio Community 2019
* .NET 5
* Entity Framework Core tools: `dotnet tool install --global dotnet-ef`
* SSMS with LocalDB installed. [Getting started](https://www.mssqltips.com/sqlservertip/5612/getting-started-with-sql-server-2017-express-localdb) guide
* Postman

# Getting started
* Visual Studio - Solution - Restore NuGet packages. Then build.
* Command prompt: check if MSSQLLocalDB exists and the instance is running:  
    ```
    SqlLocalDb info MSSQLLocalDB  
    SqlLocalDb start MSSQLLocalDB  
    ```
* The project uses LocalDB. See ConnectionString in src\appsettings.json
* Visual Studio - Developer Command Prompt. Run this to create a PSCodeCamp database:  
    dotnet ef database update  
    Expected output: Build started... Build succeeded. Applying migration '20180928134504_initialdb'. Done.
* To connect using SSMS: Server Name (localdb)\MSSQLLocalDB
* Visual Studio - set Debug Target to CoreCodeCamp (see src\Properties\launchSettings.json)
 * Developer Command Prompt: `dotnet run`
 * Alternatively, Start Dedbugging. This will open a browser, and if you don't want it, set `"launchBrowser": false,` in src\Properties\launchSettings.json
* Navigate to: https://localhost:5001/api/values

# Map entities to models in controller actions
* Install NuGet package Automapper.Extensions.Microsoft.Dependencyinjection
* In Startup.cs/ConfigureServices add `services.AddAutoMapper( Assembly.GetExecutingAssembly() );`
* 
