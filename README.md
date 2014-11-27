## Synopsis

MVC 4 sowing calendar app.

## Installation on Mono

 1. Install an IDE as [*Xamarin Studio*, *MonoDevelop*](http://www.monodevelop.com/download/) and if necessary the [*Mono runtime*](http://www.mono-project.com/download/).
 2. Clone the repository with `git clone https://www.github.com/erooijak/zaaikalender`.
 3. Open the solution and get the packages with NuGet. <br>Note: Do not update to a later version of MVC since the app depends on MVC4.

4. Install a PostgreSQL database and create a user, test database and schema and grant the user access:
    `sudo adduser zktest -- Give user zktest password "broccoli"`  
    `sudo su - postgres`  
    `createdb ZkTestDatabase`  
    `psql ZkTestDatabase`  
    
    `GRANT ALL PRIVILEGES ON DATABASE "ZkTestDatabase" TO zktest;`  
    `GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO zktest;`  
    `GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO zktest;`

    `Press CTRL-D twice to leave the psql terminal and login.`

Now set `Zk.Migrations` as the startup project and select `var step = Database.Update_Database`. This will update the database to the latest migration.

For Visual Studio users just open the project and have it converted and take similar steps as above.

## Tests

Run with NUnit.

## Git

First time clone: `git clone https://www.github.com/erooijak/zaaikalender`.

For collaborators:

 1. Get latest version: `git pull origin master`.
 2. See changes: `git status`.
 3. Add changes: `git add -A` (add all with -A flag, or specify specific file)
 4. Commit changes: `git commit -m "Descriptive message of what you did"`
 5. Push to GitHub: `git push origin master` (make sure solution builds).
 6. Repeat.

## Contributors

Elisa, Jeroen and Erwin.

## License

The MIT License (MIT)

Copyright (c) 2014 Zaaikalender DevOps team

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
