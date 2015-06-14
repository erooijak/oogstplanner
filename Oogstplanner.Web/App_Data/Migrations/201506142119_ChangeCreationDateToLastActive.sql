/* * * * * * * * * * * * * * * * * * * * * * *
 *
 * Migration:		ChangeCreationDateToLastActive
 *
 * Date and time:	6/14/2015 9:19:38 PM
 *
 * * * * * * * * * * * * * * * * * * * * * * */

ALTER TABLE "public"."Users" RENAME COLUMN "CreationDate" TO "LastActive";

// Hand made
