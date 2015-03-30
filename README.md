## Synopsis

ASP.NET MVC 4 harvesting planner app running on [Mono](http://www.mono-project.com) using a [PostgreSQL](http://www.postgresql.org/) database.

## Installation

 1. Install an IDE as [*Xamarin Studio*, *MonoDevelop*](http://www.monodevelop.com/download/) and if necessary the [*Mono runtime*](http://www.mono-project.com/download/). For Windows user install [*Mono tools*](http://www.mono-project.com/archived/gettingstartedwithmonotools/) for Visual studio.
 2. Clone the repository with git clone https://www.github.com/erooijak/zaaikalender.
 3. Open the solution and get the packages with NuGet. *Note: Do not update to a later version of MVC since the app depends on MVC4.*

 4. Install a PostgreSQL database and create a user, test database and schema and grant the user access:  

    `sudo apt-get install postgresql-9.3`  
    `sudo su - postgres`  
    `psql`  

     `-- Create a test user with password broccoli (encrypted) which is used in the connection string.`   
    `CREATE ROLE zktest LOGIN ENCRYPTED PASSWORD 'md5638a57daa56afced2a664def8fa3d93d' NOSUPERUSER INHERITNOCREATEDB NOCREATEROLE NOREPLICATION;`  
    `CREATE DATABASE "ZkTestDatabase" OWNER zktest;`    

Then follow the database setup and run the migrations on the ZkTestDatabase and ZkTestUsersDatabase (for Membership table only) in [App_Data/Migrations](https://github.com/erooijak/zaaikalender/tree/master/Zk/App_Data/Migrations).

Now the tables are created grant the user access and create test data:

    `GRANT ALL ON DATABASE "ZkTestDatabase" TO zktest;`  
    `GRANT ALL ON ALL TABLES IN SCHEMA public TO zktest;`  
    `GRANT ALL ON ALL SEQUENCES IN SCHEMA public TO zktest;`

    COPY "Crops" FROM '/zaaikalender/CropData.csv' DELIMITER ',' CSV;

    -- Note: if calendars throws an exception, first create a user via logging into the site
    COPY "Calendars" FROM '/zaaikalender/CalendarData.csv' DELIMITER ',' CSV;
    COPY "FarmingActions" FROM '/zaaikalender/FarmingActionsTestData.csv' DELIMITER ',' CSV;

Finally, remove the _ prefix from the Zk/_ConnectionStrings.config file to include it in the project so that the application can access the database with the user zktest created earlier.

To enable lost password e-mailing remove the _ prefix from the Zk/_Email.config file and add your own SMTP server (ensure you have [imported certificates](stackoverflow.com/questions/9801224/smtpclient-with-gmail#9803922) if using gmail).

## Tests

Run with NUnit.

On Ubuntu derivatives with MonoDevelop as IDE install package with `sudo apt-get install monodevelop-nunit`.
Once you have it, in MonoDevelop click on "View -> Pads -> Unit Tests".

## Git

First time clone: `git clone https://www.github.com/erooijak/zaaikalender` or pull request.

For collaborators:

 1. Get latest version: git pull origin master.
 2. See changes: git status.
 3. Add changes: git add -A (add all with -A flag, or specify specific file)
 4. Commit changes: git commit -m "Descriptive message of what you did"
 5. Push to GitHub: git push origin master (make sure solution builds).
 6. Repeat.

## Contributors

Elisa, Jeroen and Erwin.

## License

The MIT License (MIT)

Copyright (c) 2015 Zaaikalender DevOps team

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
