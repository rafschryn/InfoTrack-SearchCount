# InfoTrack-SearchCount
This Application can be used to 'scrap' Google and Bing searches without using their APIs, which is against their Terms of Services and your IP might get blocked.
Use this application at your own risks.

If Google or Bing decide to change their DOM, this Application would need to be updated.

## Design
### API
The API is using .Net 8 and Entity Framework to communicate with a SQL Express database.

The connection string (`Server=localhost\\SQLEXPRESS;Database=InfoTrack-SearchCount;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False`) can be found in the appsettings.json.
The database connection is not crypted to facilitate the setup.

It has 4 layers: Controllers, Handlers/Services, Repositories and Contexts.

Dependency injections are used across the API.

Each method is covered by Unit Tests (xUnit), and each endpoint is covered by Integration Tests.

Requests are validated and a GlobalExceptionHandler intercepts and Logs custom and native Exceptions.

Data that could vary per environment sit in the appsettings.json (number of results to scrap),
and data that could change over time (Search Engine regex) sit in a easily accessible Constant file (*SearchCount.Shared/Constants.cs*)

### Front-end
The Front-End is using a classic Angular framework, with components and services.

Each method is covered by Unit Tests (Jasmine).

Bootstrap has been used for a few components (namely the navigation bar and the toasters).

An ErrorHandler keeps track and handle and Exceptions returned by the API.

## Setup
### API
 - Make sure you have **SQL Express** and **.Net 8.0** installed
 - Open the **InfoTrack-SearchCount.sln** in Visual Studio or similar
 - In the Console, enter '**Update-Database**' to initialise the Database
 - <ins>Optional</ins>: to fully test the application, the database can be populated with dummy data.
 Drag and Drop **SearchCount.Contexts/Scripts/SearchCountHistorySeed.sql** into your Microsoft SQL Server Management Studio then 'Execute'
 - Use SearchCount.API as the Startup project and use 'http' as the Launch Profile
![enter image description here](https://github.com/rafschryn/InfoTrack-SearchCount/assets/159721556/cc7dc77d-0551-4b46-bf8d-32e43af389be)

### Front-end

- Make sure you have **npm** and **Node.js** installed (https://docs.npmjs.com/downloading-and-installing-node-js-and-npm)
 - Open the **SearchCount.UI** folder in Visual Studio Code or similar
 - Run '**npm i**' and '**npm update**'
 - <ins>Optional</ins>: run **ng test** to run the Unit Tests
 - Run **ng serve** to start the Front-End
 - Access the Application using **localhost:4200**

## Hot to use
### Search Counts
![image](https://github.com/rafschryn/InfoTrack-SearchCount/assets/159721556/782b25bf-d412-435e-aca6-d9b084564f65)

Enter a Search term and the URL you would like the results to contain, select the search engine (Google or Bing),
and submit.
In this screenshot, only the first search result is for infotrack.co.uk (the second and third are for infotrack.com)
**Note 1**: paid placements/sponsored websites that appear at the top of a normal search are not included in this Result Placement, as their placements are not a result of their popularity or relevance. 
**Note 2**: Google returns 100 results as per the description of the task. Bing only returns roughly 30 results and would need to go to the second webpage to display more results, which the Application does not do.

### History
![enter image description here](https://github.com/rafschryn/InfoTrack-SearchCount/assets/159721556/b376f8f4-21c0-4072-b515-b9b18a383021)

Here is displayed the whole history of the Searches.
This table is filterable on the Search Term, the URL, the Search Engine, and the Date of Execution.
For the latter, a date range can be selected to display only the searches executed during a date period.
**Note**: Knowing the small amount of items in the table at any given time, I opted for a filtering functionality on the front-end side. This also allowed the Application to filter the table as the user types.
If the expected amount of items grows, and a more complex filtering functionality or Reporting is expected, I would advise for a filtering functionality in the back-end.

## Improvements
Here's a list of a few improvements this Application would welcome:

 - Sorting functionality for the History table
 - Ability, from the front-end, to delete records
 - Authentication system
 - Use of Fluent Validation instead of returning an Exception on validation failure
 - Weekly trend analysis for a given Search Term, URL and Search Engine
 - UI/UX improvement for visually impaired people
 - Sending the Logs to a database so they can be analysed
 - Pagination system for the History table
 - More Integration tests to test edge cases

## Task description
[InfoTrack Project Waterloo.pdf](https://github.com/rafschryn/InfoTrack-SearchCount/files/14234087/InfoTrack.Project.Waterloo.pdf)
