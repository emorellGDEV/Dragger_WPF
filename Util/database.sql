CREATE TABLE Cards (
    id_card INTEGER PRIMARY KEY,
    fk_id_responsable INTEGER NOT NULL,
    description VARCHAR(100),
    color VARCHAR(100) NULL,
    priority VARCHAR(100) NOT NULL,
    goalDate DateTime NULL,
    creationDate DateTime NULL,
    position INTEGER NOT NULL
);

CREATE TABLE Persons (
    id_person INTEGER PRIMARY KEY,
	name VARCHAR(100)
);