## Synopsis

ASP.NET MVC 4 harvesting planner app running on [Mono](http://www.mono-project.com) using a [PostgreSQL](http://www.postgresql.org/) database.

## Installation

 1. Install an IDE as [*Xamarin Studio*, *MonoDevelop*](http://www.monodevelop.com/download/) and if necessary the [*Mono runtime*](http://www.mono-project.com/download/). For Windows user install [*Mono Tools*](http://www.mono-project.com/archived/gettingstartedwithmonotools/) for Visual Studio.
 2. Clone the repository with `git clone https://www.github.com/erooijak/oogstplanner`.
 3. Open the solution and get the packages with NuGet. *Note: Do not update to a later version of MVC since the app depends on MVC4.*
 4. Install the PostgreSQL database

    `sudo apt-get install postgresql-9.3`  

 5. Run [setup_database.sh](https://raw.githubusercontent.com/erooijak/oogstplanner/master/setup_database.sh).
 6. Remove the _ prefix from the Oogstplanner.Web/_ConnectionStrings.config file to include it in the project so that the application can access the database with the user test_oogstplanner_database_user created earlier.

To enable lost password e-mailing remove the _ prefix from the Oogstplanner.Web/_Email.config file and add your own SMTP server (ensure you have [imported certificates](https:/www.stackoverflow.com/questions/9801224/smtpclient-with-gmail#9803922) if using gmail).

## Setting up Mono and Nginx

See http://www.philliphaydon.com/2013/06/setting-up-mono-on-nginx/.

    fastcgi-mono-server4 /applications=/:/path/to/applicationroot/ /socket=tcp:ip:9000 /logfile=/var/log/mono/fastcgi.log & 
    
## Tests

Run with NUnit.

On Ubuntu derivatives with MonoDevelop as IDE install package with `sudo apt-get install monodevelop-nunit`.
Once you have it, in MonoDevelop click on "View -> Pads -> Unit Tests".

## Git

First time clone: `git clone https://www.github.com/erooijak/oogstplanner` or pull request.

## Contributors

Elisa, Jeroen and Erwin.

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
