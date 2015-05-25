#!/bin/bash

# Summary: 
#   This script will remove users, calendars and farming actions of anonymous users that 
#   are more than one week old.
#
# Dependencies: 
#   PostgreSQL 8+
#   postgres user
#
# Options:
#   -d            name of the database
#
# Usage:
#   Run with ./clean-database.sh [-d database_name].

# Get command line options.
dflag=
while getopts d: option
do
  case $option in
    d) dflag=1
       database="$OPTARG";;
    ?) printf "Usage: %s: [-d database_name] \n" $0
       exit 2;;
  esac
done

# Store postgres password in env variable so the statements do not request a password.
if [ -z $PGPASSWORD ]; then
  printf "Password for user postgres: "
  read -s pw
  printf "\n"
  export PGPASSWORD=$pw
fi

psql -U postgres -d $database -c \
  "DELETE FROM \"Users\"  \
   WHERE \"AuthenticationStatus\" = 0 AND \
         \"CreationDate\" < now()::date - 7;"

