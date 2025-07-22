DROP TABLE IF EXISTS __efmigrationshistory;
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory"
(
    migrationid character varying(150) NOT NULL,
    productversion character varying(32) NOT NULL,
    CONSTRAINT pk___efmigrationshistory PRIMARY KEY (migrationid)
    );
DELETE FROM "__EFMigrationsHistory";
INSERT INTO "__EFMigrationsHistory" VALUES ('20240628132207_Initial', '8.0.13');
