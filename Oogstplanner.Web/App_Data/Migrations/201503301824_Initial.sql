/* * * * * * * * * * * * * * * * * * * * * * *
 *
 * Migration:		Initial
 *
 * Date and time:	3/30/2015 6:24:02 PM
 *
 * * * * * * * * * * * * * * * * * * * * * * */

CREATE SCHEMA IF NOT EXISTS "public";
CREATE TABLE "public"."Calendars" (
    "CalendarId" serial NOT NULL,
    "UserId" integer NOT NULL,
    CONSTRAINT "PK_public.Calendars" PRIMARY KEY ("CalendarId")
);
CREATE INDEX "IX_public.Calendars_UserId" ON "public"."Calendars"("UserId");
CREATE TABLE "public"."FarmingActions" (
    "Id" serial NOT NULL,
    "Month" integer NOT NULL,
    "Action" integer NOT NULL,
    "CropCount" integer NOT NULL,
    "Calendar_CalendarId" integer,
    "Crop_Id" integer,
    CONSTRAINT "PK_public.FarmingActions" PRIMARY KEY ("Id")
);
CREATE INDEX "IX_public.FarmingActions_Calendar_CalendarId" ON "public"."FarmingActions"("Calendar_CalendarId");
CREATE INDEX "IX_public.FarmingActions_Crop_Id" ON "public"."FarmingActions"("Crop_Id");
CREATE TABLE "public"."Crops" (
    "Id" serial NOT NULL,
    "Name" text,
    "Race" text,
    "Category" text,
    "GrowingTime" integer NOT NULL,
    "AreaPerCrop" float8,
    "AreaPerBag" float8,
    "PricePerBag" decimal,
    "SowingMonths" integer NOT NULL,
    "HarvestingMonths" integer NOT NULL,
    CONSTRAINT "PK_public.Crops" PRIMARY KEY ("Id")
);
CREATE TABLE "public"."Users" (
    "UserId" serial NOT NULL,
    "Name" text,
    "FullName" text,
    "Email" text,
    "Enabled" boolean NOT NULL,
    CONSTRAINT "PK_public.Users" PRIMARY KEY ("UserId")
);
CREATE TABLE "public"."PasswordResetTokens" (
    "Id" serial NOT NULL,
    "Email" text,
    "Token" text,
    "TimeStamp" timestamp NOT NULL,
    CONSTRAINT "PK_public.PasswordResetTokens" PRIMARY KEY ("Id")
);
ALTER TABLE "public"."Calendars" ADD CONSTRAINT "FK_public.Calendars_public.Users_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."Users" ("UserId") ON DELETE CASCADE;
ALTER TABLE "public"."FarmingActions" ADD CONSTRAINT "FK_public.FarmingActions_public.Calendars_Calendar_CalendarId" FOREIGN KEY ("Calendar_CalendarId") REFERENCES "public"."Calendars" ("CalendarId");
ALTER TABLE "public"."FarmingActions" ADD CONSTRAINT "FK_public.FarmingActions_public.Crops_Crop_Id" FOREIGN KEY ("Crop_Id") REFERENCES "public"."Crops" ("Id");
