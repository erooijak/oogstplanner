This folder is excluded by our .gitignore file.

For production create a role production_oogstplanner_database_user:
CREATE USER production_oogstplanner_database_user WITH PASSWORD 'aubergine';

Change the regular Migrations by replacing "test_oogstplanner_database" with "production_oogstplanner_database" (everywhere except ConnectionStrings)
and replacing "test_oogstplanner_database_user" with "production_oogstplanner_database_user" in the postgresql setup and migration scripts.

Finally, make sure the connection string specified in OogstplannerContext.cs has name "ProductionOogstplannerDatabaseConnection".


COPY TO SERVER

scp -r /home/erooijak/zaaikalender/Oogstplanner/* root@85.17.176.210:/home/erooijak/oogstplanner.nl/httpdocs/
scp /home/erooijak/zaaikalender/CropData.csv root@85.17.176.210:/home/erooijak/oogstplanner.nl/

cd /home/erooijak/oogstplanner.nl/httpdocs
fastcgi-mono-server4 /applications=/:/home/erooijak/oogstplanner.nl/httpdocs/ /socket=tcp:192.168.2.223:9000 /logfile=/var/log/mono/fastcgi.log & 