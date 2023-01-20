CREATE TABLE Cards (
    id_card INTEGER PRIMARY KEY NOT NULL,
    fk_id_responsable INTEGER NOT NULL,
    description VARCHAR(100) NOT NULL,
    color VARCHAR(100) NULL,
    priority VARCHAR(100) NOT NULL,
    goalDate DATE NULL,
    creationDate DATE NOT NULL,
    position INTEGER NOT NULL
);

CREATE TABLE Persons (
    id_person INTEGER NOT NULL PRIMARY KEY,
	name VARCHAR(100) NOT NULL
);