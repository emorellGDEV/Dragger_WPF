CREATE TABLE IF NOT EXISTS Cards (
    id_card INTEGER NOT NULL; PK
    fk_id_responsable INTEGER NOT NULL;
    description VARCHAR(100) NOT NULL;
    color VARCHAR(100) NOT NULL;
    priority VARCHAR(100) NOT NULL;
    goalDate DATE NULL;
    creationDte DATE NOT NULL;
);

CREATE TABLE IF NOT EXISTS Persons (
    id INTEGER NOT NULL PRIMARY KEY; PK
	name VARCHAR(100) NOT NULL;
);