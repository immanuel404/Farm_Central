=================================

** FARM CENTRAL WEB-APP **

================================


<br />
==> ABOUT:

- Web-app for the management of farm produce for produce wholesaler,
allowing farmers to detail produce available for sale. Built with ASP.NET-MVC, SQL, & Bootstrap.

- Features: CRUD operations, Data filtering, Authentication, Cookies, & Unit Tests.


<br />
==> TECHNOLOGIES:

- ASP.NET Core-V6 Web-App (Model-View-Controller).

- Database: Microsoft SQL Server Management Studio (MSSMS).

- Visual Studio IDE will be needed to run this application.

- MSSMS will be needed to link to the local database.

- The website can linked to a SQL Database Azure Cloud.


<br />
==> INITIAL SET-UP & DATA POPULATION:

- Once in visual studio, Select Tools->Nuget Package Manager->Package Manager Console.

- Once in the console run:

		- Add-Migration initial

		- Update-Database

- The commands above will create the databases and add some pre-populated data.

- Pre-populated Data is found in ->Models->DBContext of the application file.

- When running unit tests, 'Test_Mode' variable in controllers must be set to true.

<br /><br />
