==> ABOUT:

- Built with ASP.NET MVC, MS SQL, Bootstrap - Allows the wholesalers to manage farm produce inventory.


<br />
==> INITIAL SET-UP & SEEDING:

- Once in visual studio, Select Tools->Nuget Package Manager->Package Manager Console.

- Once in the console run:

		- Add-Migration initial

		- Update-Database
  
- The commands above will create the databases and add some pre-populated data.

- Pre-populated Data is found in ->Models->DBContext of the application file.

- When running unit tests, 'Test_Mode' variable in controllers must be set to true.
<br /><br />
