# Oogstplanner

ASP.NET MVC 4 harvesting planner app running on [Mono](http://www.mono-project.com) using a [PostgreSQL](http://www.postgresql.org/) database.

## Installation

 1. Install an IDE (e.g, [*Xamarin Studio*, *MonoDevelop*](http://www.monodevelop.com/download/)) and the [*Mono 4.x runtime*](http://www.mono-project.com/download/). For Windows user install [*Mono Tools*](http://www.mono-project.com/archived/gettingstartedwithmonotools/) for Visual Studio.
 2. Clone the repository `git clone https://www.github.com/erooijak/oogstplanner`.
 3. Open the solution and get the packages with NuGet (for MonoDevelop install the [NuGet add-in](http://community.sharpdevelop.net/blogs/mattward/archive/2013/01/07/MonoDevelopNuGetAddin.aspx)).  *Note: Do not update to a later version of MVC since the app depends on MVC4.*
 4. Install the PostgreSQL database
    `sudo apt-get install postgresql-9.3`  
 5. Run [setup-database.sh](https://raw.githubusercontent.com/erooijak/oogstplanner/master/setup-database.sh).
 6. Remove the _ prefix from the Oogstplanner.Web/_ConnectionStrings.config file to include it in the project so that the application can access the database with the user test_oogstplanner_database_user created in the setup-database script.

To enable lost password e-mailing remove the _ prefix from the Oogstplanner.Web/_Email.config file and add your own SMTP server (ensure you have [imported certificates](https:/www.stackoverflow.com/questions/9801224/smtpclient-with-gmail#9803922) if using gmail).

## Architecture

The application uses the Model-View-Controller architecture powered by ASP.NET MVC with a generic repository and unit of work pattern.

![MVC Architecture](https://raw.githubusercontent.com/erooijak/oogstplanner/master/architecture.jpg)  
*Layers of Model-View-Controller architecture with Task (Unit of Work) pattern (image [source](https://merroun.wordpress.com/2012/03/28/mvvm-mvp-and-mvc-software-patterns-againts-3-layered-architecture/))*

HTTP requests to certain URLs (configured in [RouteConfig.cs](https://github.com/erooijak/oogstplanner/blob/master/Oogstplanner.Web/App_Start/RouteConfig.cs)) which are initiated by JavaScript events in [Oogstplanner.Web/Scripts](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Web/Scripts) or browser navigation are mapped to action methods on the controllers ([Oogstplanner.Web/Controllers](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Web/Controllers)). The controllers use services ([Oogstplanner.Services](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Services)) which use the data access layer ([Oogstplanner.Data](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Data)) to perform tasks on the data represented by the data model ([Oogstplanner.Models](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Models)). The Unit of Work ([OogstplannerUnitOfWork](https://github.com/erooijak/oogstplanner/blob/master/Oogstplanner.Data/OogstplannerUnitOfWork.cs)) represents these tasks and coordinates the work of repositories using a single [OogstplannerContext](https://github.com/erooijak/oogstplanner/blob/master/Oogstplanner.Data/OogstplannerContext.cs). The repositories being classes that provide methods on the context, and the context being an abstraction layer which queries the database. Based on the feedback of the services the controller update the Views ([Oogstplanner.Web/Views](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Web/Views)) which are rendered in the browser as HTML.

The Inversion of Control container [Autofac](http://autofac.org/) is used in [IocConfig.cs](https://github.com/erooijak/oogstplanner/blob/master/Oogstplanner.Web/App_Start/IocConfig.cs) to register all dependencies (services, repositories, database context and unit of work), which are provided to classes via constructor injection.

## TypeScript

The project uses TypeScript. For MonoDevelop see this [TypeScript add-in](http://addins.monodevelop.com/Project/Index/128). 

## Tests

[Oogstplanner.Tests](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Tests) is written with the help of the unit testing framework [NUnit](http://www.nunit.org/) and the mocking framework [Moq](https://github.com/Moq/moq4).

On a distro with MonoDevelop as IDE install package with `sudo apt-get install monodevelop-nunit` or your package manager of choice.

Once installed, in MonoDevelop click on "View -> Pads -> Unit Tests > Run All".

## Database migrations

The application uses [Entity Framework 6](https://github.com/aspnet/EntityFramework) (EF) as data access technology. EF Code First database migrations on Windows are done using PowerShell commands in the Package Manager Console. On Linux there is no such thing as PowerShell (yet). So how to create EF migrations when the data model changes on Linux?

Since the EF migrations commands are "just thin wrappers over an underlying API" (quote [source](http://stackoverflow.com/questions/20374783/enable-entity-framework-migrations-in-mono#20382226)), it is possible to hook into the API directly. In [Oogstplanner.Migrations](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Migrations) the Add-Migration and Update-Database commands are implemented in a console project with entrance point being [_MigrationsTool.cs](https://raw.githubusercontent.com/erooijak/oogstplanner/master/Oogstplanner.Migrations/_MigrationsTool.cs).

To handle EF migrations with PostgreSQL this project makes use of the [PostgreSqlMigrationSqlGenerator project](https://github.com/darionato/PostgreSqlMigrationSqlGenerator) of Dario Malfatti.

It is somewhat of a cumbersome process, but migrations can be created as follows:

1. Set Oogstplanner.Migrations as startup project.
2. Specify `ADD_MIGRATION` in the step variable and choose a `MIGRATION_NAME`.
3. Run the code (F5).
4. Three migration files with the specified name are created and need to be added to the project (right click on project, "Add > Add Files from Folder...").
5. Set the build action of the resource file (.resx) extension to EmbeddedResource (right click on file, "Build Action > EmbeddedResource").
6. Specify `CREATE_SCRIPT` to create a database script in `App_Data/Migrations`.
7. Run the code (F5).
8. Right click on `App/Data/Migrations` and add file ("Add > Add Files from Folder...").
9. The PostgreSqlMigrationSqlGenerator for some reason does not add semicolons after each line. Add them manually.
10. Prefix the migration with the current year, date, and time (e.g., "201519061942_") (this is necessary so the [setup-database.sh](https://raw.githubusercontent.com/erooijak/oogstplanner/master/setup-database.sh) script triggers the migrations in the right order).
11. Now the SQL script is created and can be run on the database manually or via the [setup-database.sh](https://raw.githubusercontent.com/erooijak/oogstplanner/master/setup-database.sh) script.

See http://romiller.com/2012/02/09/running-scripting-migrations-from-code/ for more information on running scripting migrations from code.

## Deployment

Create a release build, copy the DLLs, configuration files and web content ([Views](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Web/Views) and [Content](https://github.com/erooijak/oogstplanner/tree/master/Oogstplanner.Web/Content) folder) to the remote host.

See http://www.philliphaydon.com/2013/06/setting-up-mono-on-nginx/ for an excellent explanation on setting up Mono on nginx. Follow that explanation or use another web server of your choice. In the case of nginx start the app with:

    fastcgi-mono-server4 /applications=/:/path/to/applicationroot/ /socket=tcp:ip:9000 /logfile=/var/log/mono/fastcgi.log & 

## Future

Currently the system is in the minimal viable product (MVP) stage. There are a lot of things that can be done to improve the application. Here are a few:

- [ ] Convert somewhat awkward JavaScripts to [AngularJS](https://angularjs.org/).
- [ ] Provide clickable tree view of available crops under the crops search button.
- [ ] Crop searching on category or preferred harvesting or sowing month.
- [ ] Make calendar span multiple years (i.e., seeds sowed in one year can be harvested in a later year or vice versa).
- [ ] Multiple calendars for one user.
- [ ] Editing of user profile.
- [ ] Choosing profile image.
- [ ] Comments on user profile.
- [ ] Improved mobile support (especially on sowing/harvesting page).
- [ ] Improved CSS styling.
- [ ] Logging of errors.
- [ ] Data and language of other countries.
- [ ] Integration with external seeds webshop.
- [ ] Add area for blog posts and articles.
- [ ] Export sowing calendar to PDF, Google Calendar and/or Excel.
- [ ] Only show sowing or harvesting calendar.
- [ ] Refactor code (e.g., split up FarmingAction class in sowing and harvesting part).
- [ ] Improve SEO.
- [ ] Provide effects (e.g., shaking of login modal dialog on error, loading symbol while loading).
- [ ] Integrate with external APIs (e.g., weather forecast).
- [ ] Integration with food recipes, calories and/or vitamins.

Suggestions and/or pull requests are very welcome.

## Contributors

Elisa, Jeroen, Anna, Stefan and Erwin.

## License

The MIT License (MIT)

Copyright (c) 2015 Oogstplanner DevOps team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE
