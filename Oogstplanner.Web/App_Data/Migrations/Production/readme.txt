This folder is excluded by our .gitignore file.

For production create a role zkprod:
CREATE USER zkprod WITH PASSWORD 'aubergine';

Change the regular Migrations by replacing "ZkTestDatabase" with "ZkProductionDatabase" (everywhere except ConnectionStrings)
and replacing "zktest" with "zkprod" in the postgresql setup and migration scripts.

Finally, make sure the connection string specified in ZkContext.cs has name "ZkProductionDatabaseConnection".


COPY TO SERVER

scp -r /home/erooijak/zaaikalender/Zk/* root@85.17.176.210:/home/erooijak/oogstplanner.nl/httpdocs/
scp /home/erooijak/zaaikalender/CropData.csv root@85.17.176.210:/home/erooijak/oogstplanner.nl/

cd /home/erooijak/oogstplanner.nl/httpdocs
fastcgi-mono-server4 /applications=/:/home/erooijak/oogstplanner.nl/httpdocs/ /socket=tcp:192.168.2.223:9000 /logfile=/var/log/mono/fastcgi.log & 