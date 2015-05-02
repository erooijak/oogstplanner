/* * * * * * * * * * * * * * * * * * * * * * *
 *
 * Migration:		AddAuthenticationStatus
 *
 * Date and time:	4/26/2015 8:11:52 PM
 *
 * * * * * * * * * * * * * * * * * * * * * * */

ALTER TABLE "public"."Users" ADD "AuthenticationStatus" integer NOT NULL DEFAULT 1;
ALTER TABLE "public"."Users" ADD "CreationDate" timestamp NOT NULL DEFAULT '1900-01-01T00:00:00.000';
ALTER TABLE "public"."Users" DROP COLUMN "Enabled";
