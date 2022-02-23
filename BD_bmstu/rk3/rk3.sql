-- Задание 1
DROP TABLE IF EXISTS Drivers CASCADE;
DROP TABLE IF EXISTS Fines CASCADE;
DROP TABLE IF EXISTS Cars CASCADE;
DROP TABLE IF EXISTS DC CASCADE;

CREATE TABLE IF NOT EXISTS Drivers
(
    DriverID SERIAL PRIMARY KEY,
    DriverLicense VARCHAR,
    FIO VARCHAR,
    Phone VARCHAR
);

CREATE TABLE IF NOT EXISTS Fines
(
    FineID SERIAL PRIMARY KEY,
    FineType VARCHAR,
    Amount REAL,
    FineDate DATE
);

CREATE TABLE IF NOT EXISTS Cars
(
    CarID SERIAL PRIMARY KEY,
    Model VARCHAR,
    Color VARCHAR,
    Year INT,
    RegistrationDate DATE
);

CREATE TABLE IF NOT EXISTS DC ();

ALTER TABLE DC ADD COLUMN DriverID INTEGER;
ALTER TABLE DC ADD COLUMN CarID INTEGER;

ALTER TABLE DC
   ADD CONSTRAINT fk_d_id
   FOREIGN KEY (DriverID)
   REFERENCES Drivers(DriverID);

ALTER TABLE DC
   ADD CONSTRAINT fk_c_id
   FOREIGN KEY (CarID)
   REFERENCES Cars(CarID);

ALTER TABLE Fines ADD COLUMN DriverID INTEGER;

ALTER TABLE Fines
   ADD CONSTRAINT fk_f_id
   FOREIGN KEY (DriverID)
   REFERENCES Drivers(DriverID);

INSERT INTO Drivers VALUES (1, '12937482', 'Искакова Карина Муратовна', '89857375335');
INSERT INTO Drivers VALUES (2, '84583828', 'Куликов Дмитрий Алексеевич', '89365647365');
INSERT INTO Drivers VALUES (3, '74726462', 'Никитина Ирина Александровна', '89162843567');

INSERT INTO Fines VALUES (11, 'За скорость', 5000, '2020-05-07', 1);
INSERT INTO Fines VALUES (12, 'За пяьное вождение', 200000, '2021-12-27', 2);
INSERT INTO Fines VALUES (13, 'За мат сотруднику полиции', 5000, '2019-11-14', 3);

INSERT INTO Cars VALUES (1, 'Газель', 'Красный', 2000, '2004-05-06');
INSERT INTO Cars VALUES (2, 'Иномарка', 'Черный', 2019, '2020-06-16');
INSERT INTO Cars VALUES (3, 'Седан', 'Белый', 2018, '2019-12-23');

INSERT INTO DC VALUES  (1, 1);
INSERT INTO DC VALUES  (2, 3);
INSERT INTO DC VALUES  (3, 2);

-- Для каждого водителя найти его первый штраф - табличная функция
INSERT INTO Fines VALUES (14, 'За пересечение сплошной', 5000, '2020-05-07', 1);
INSERT INTO Fines VALUES (15, 'Проезд на красный свет', 3000, '2020-05-07', 2);

DROP FUNCTION IF EXISTS First_Fine();

CREATE FUNCTION First_Fine()
RETURNS TABLE (
        DriverID INT,
        FIO VARCHAR,
        FineID INT,
        R BIGINT
        ) AS $$
begin
   RETURN QUERY (
          SELECT *
        FROM (
            SELECT Drivers.DriverID, Drivers.FIO, Fines.FineID,
                   row_number() OVER (PARTITION BY Drivers.DriverID
                                      ORDER BY Fines.FineID) AS r
            FROM Drivers JOIN Fines ON Drivers.DriverID = Fines.DriverID) AS f
        WHERE f.r = 1
   );
end;
$$
LANGUAGE 'plpgsql';

SELECT * FROM First_Fine();