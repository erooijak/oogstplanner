#!/bin/bash

# Summary: 
#   This script will run the commands to create the Oogstplanner Membership and
#   regular PostgreSQL databases and tables. The "Crops" table will be seeded with
#   information provided in a CSV file.
#
# Dependencies: 
#   PostgreSQL 8+
#   postgres user
#   database administrative login by Unix socket needs to be set to md5 in pg_hba.conf
#   CSV with crop information (CropData.csv) in root
#   Migration scripts located in Oogstplanner.Web/App_Data/Migrations folder
#
# Options:
#   -a [optional] path to folder with database migrations and membership folders
#                 default: ./Oogstplanner.Web/App_Data/
#
#   -c [optional] path to crop data CSV file
#                 default: ./CropData.csv
#
#   -e [optional] environment which is used in user and database names
#                 default: test
#
#                 The script will create the [environment]_oogstplanner_database_user 
#                 which will own the the databases [environment]_oogstplanner_database, 
#                 and [environment]_oogstplanner_membership_database.
#
#   -p            gives prompt for specifying password for the database user
#                 default password: broccoli
#
# Usage:
#   Run with ./setup_database.sh [-a /path/app_data] [-c /path/cropdata] [-e env] [-p].

# Get command line options.
aflag=
cflag=
eflag=
pflag=
while getopts a:c:e:p option
do
  case $option in
    a) aflag=1
       appdata_path="$OPTARG";;
    c) cflag=1
       cropdata_path="$OPTARG";;
    e) eflag=1
       environment="$OPTARG";;
    p) pflag=1;;
    ?) printf "Usage: %s: [-a /path/app_data] [-c /path/cropdata] [-e env] [-p] \n" $0
       exit 2;;
  esac
done

# Define default variables which can be used when no flags are provided.
CURRENT_DIRECTORY=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
APPDATA_DEFAULT="$CURRENT_DIRECTORY/Oogstplanner.Web/App_Data/"
CROPDATA_DEFAULT="$CURRENT_DIRECTORY/CropData.csv"
ENVIRONMENT_DEFAULT="test"
DATABASE_USER_PASSWORD_DEFAULT="broccoli"

# Check if path to migrations was provided and if not if the default path can be used and
# if it was provided if the migrations folder exists.
if [ -z $aflag ]; then

  printf "Do you want to use the default path $APPDATA_DEFAULT as database scripts folder"
  read -p " (y/n)? " answer

  case ${answer:0:1} in
    y|Y|yes|Yes ) appdata_path=$APPDATA_DEFAULT
                  printf "\n";;
    * )           printf "Script aborted. Please specify App_Data path with -a option.\n"
                  exit 2;;
  esac

elif [ ! -d $appdata_path ]; then
  printf "Provided path to database scripts folder (path: %s) not found!\n" $appdata_path
  exit 2

fi

# Check if path to crop data CSV was provided and if not if the default path needs to be 
# used, and if it was provided if the file exists and has either a csv or txt extension.
if [ -z $cflag ]; then

  printf "Do you want to use the default path $CROPDATA_DEFAULT as CSV import"

  read -p " (y/n)? " answer
  case ${answer:0:1} in
    y|Y|yes|Yes ) cropdata_path=$CROPDATA_DEFAULT
                  printf "\n";;
    * )           printf "Script aborted. Please specify path to CSV with -c option.\n"
                  exit 2;;
  esac

elif [ ! -f $cropdata_path ]; then
  printf "Provided path to CSV (path: %s) not found.\n" $cropdata_path
  exit 2

elif [[ ! $cropdata_path =~ \.([Cc][Ss][Vv]|[Tt][Xx][Tt])$ ]]; then
  printf "%s is not a CSV or plain text file.\n" $cropdata_path
  exit 2

fi

# Check if environment was provided or use default
if [ -z $eflag ]; then

  printf "Do you want to use the default environment test as user and database prefix"

  read -p " (y/n)? " answer
  case ${answer:0:1} in
    y|Y|yes|Yes ) environment=$ENVIRONMENT_DEFAULT
                  printf "\n";;
    * )           printf "Script aborted. Please specify environment with -e option.\n"
                  exit 2;;
  esac

fi

# User and database names:
user=""$environment"_oogstplanner_database_user"
database=""$environment"_oogstplanner_database"
membership_database=""$environment"_oogstplanner_membership_database"

#Check if database user password flag was provided, if so show prompt if not use default.
if [ -z $pflag ]; then

  database_user_password=$DATABASE_USER_PASSWORD_DEFAULT
  printf "Using default password $database_user_password for $user...\n"

  else
    printf "Please specify a password for database user $user: "
    read -s pw
    printf "\n"
    database_user_password=$pw
fi

# Store postgres password in env variable so the statements do not request a password.
if [ -z $PGPASSWORD ]; then
  printf "Password for user postgres: "
  read -s pw
  printf "\n"
  export PGPASSWORD=$pw
fi

# Execute PostgreSQL statements and scripts:
printf "Creating user $user...\n"
psql -U postgres -c \
  "CREATE USER $user \
   WITH PASSWORD '""$database_user_password""'"

printf "\nCreate database $membership_database...\n"
psql -U postgres -c \
  "CREATE DATABASE $membership_database \
    OWNER $user;"

printf "\nInserting Membership tables and roles in $membership_database...\n"
psql -U postgres -d $membership_database -f \
  "$appdata_path/Membership/AddMembershipSchema.sql"

printf "\nGrant $user needed privileges on $membership_database..."
psql -U postgres -c \
  "GRANT ALL ON DATABASE $membership_database TO $user;"
psql -U postgres -d $membership_database -c \
  "GRANT ALL ON ALL SEQUENCES IN SCHEMA public TO $user; \
   GRANT ALL ON ALL TABLES IN SCHEMA public TO $user;"

printf "\nCreate database $database...\n"
psql -U postgres -c \
  "CREATE DATABASE $database \
   OWNER $user;"

printf "\nAdd all migrations to database $database...\n" # note: correct order due prefix
for file in $appdata_path/Migrations/*; do
  psql -U postgres -d $database -f "$file"
done

printf "\nProvide user $user with access...\n"
psql -U postgres -d $database -c \
  "GRANT ALL ON DATABASE $database TO $user; \
   GRANT ALL ON ALL TABLES IN SCHEMA public TO $user; \
   GRANT ALL ON ALL SEQUENCES IN SCHEMA public TO $user;"

printf "\nAdd data from CSV file into $database table \"Crops\"...\n"
psql -U postgres -d $database -c \
  "\copy \"Crops\" from $cropdata_path delimiter ',' csv;" && printf "SUCCESS\n"

