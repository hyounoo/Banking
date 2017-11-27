# Banking

Code First Migrations in EF to seed the database with test data.
From the Tools menu, select Library Package Manager, then select Package Manager Console. In the Package Manager Console window, enter the following command:
Enable-Migrations
This method will be called after migrating to the latest version.

You can use the DbSet<T>.AddOrUpdate() helper extension method 
to avoid creating duplicate seed data. E.g.

In the Package Manager Console window, type the following commands after changing seed data :

Add-Migration Initial

Update-Database

On Database update error: refer below URL
https://stackoverflow.com/a/15832184
