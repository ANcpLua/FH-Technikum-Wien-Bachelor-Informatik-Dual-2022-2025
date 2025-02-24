CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
                                                       "MigrationId" character varying(150) NOT NULL,
                                                       "ProductVersion" character varying(32) NOT NULL,
                                                       CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Tours" (
                         "Id" uuid NOT NULL,
                         "Name" character varying(200) NOT NULL,
                         "Description" character varying(500) NOT NULL,
                         "From" character varying(100) NOT NULL,
                         "To" character varying(100) NOT NULL,
                         "ImagePath" character varying(10000) NULL,
                         "RouteInformation" character varying(30000) NULL,
                         "Distance" numeric(18,2) NULL,
                         "EstimatedTime" double precision NULL,
                         "TransportType" character varying(50) NOT NULL,
                         CONSTRAINT "PK_Tours" PRIMARY KEY ("Id")
);

CREATE TABLE "TourLogs" (
                            "Id" uuid NOT NULL,
                            "DateTime" timestamp with time zone NOT NULL,
                            "Comment" character varying(500) NOT NULL,
                            "Difficulty" double precision NULL,
                            "TotalDistance" numeric(18,2) NULL,
                            "TotalTime" double precision NULL,
                            "Rating" double precision NULL,
                            "TourPersistenceId" uuid NOT NULL,
                            CONSTRAINT "PK_TourLogs" PRIMARY KEY ("Id"),
                            CONSTRAINT "FK_TourLogs_Tours_TourPersistenceId" FOREIGN KEY ("TourPersistenceId") REFERENCES "Tours" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_TourLogs_TourPersistenceId" ON "TourLogs" ("TourPersistenceId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250216175508_Initial', '8.0.0');

COMMIT;