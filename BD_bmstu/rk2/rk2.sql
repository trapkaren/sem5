-- Искакова Карина ИУ7-52Б

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

ALTER TABLE Drivers ADD COLUMN FineID INTEGER;

ALTER TABLE Drivers
   ADD CONSTRAINT fk_f_id
   FOREIGN KEY (FineID)
   REFERENCES Fines(FineID);

INSERT INTO Drivers VALUES (1, '12937482', 'Искакова Карина Муратовна', '89857375335', 11);
INSERT INTO Drivers VALUES (2, '84583828', 'Куликов Дмитрий Алексеевич', '89365647365', 12);
INSERT INTO Drivers VALUES (3, '74726462', 'Никитина Ирина Александровна', '89162843567', 13);

INSERT INTO Cars VALUES (1, 'Газель', 'Красный', 2000, '2004-05-06');
INSERT INTO Cars VALUES (2, 'Иномарка', 'Черный', 2019, '2020-06-16');
INSERT INTO Cars VALUES (3, 'Седан', 'Белый', 2018, '2019-12-23');

INSERT INTO Fines VALUES (11, 'За скорость', 5000, '2020-05-07');
INSERT INTO Fines VALUES (12, 'За пяьное вождение', 200000, '2021-12-27');
INSERT INTO Fines VALUES (13, 'За мат сотруднику полиции', 5000, '2019-11-14');

INSERT INTO DC VALUES  (1, 1);
INSERT INTO DC VALUES  (2, 3);
INSERT INTO DC VALUES  (3, 2);

-- Задание 2
-- 1. group by + having
-- Вывести id водителей, у которых  штрафа > 5000
SELECT DriverID
FROM Drivers JOIN Fines ON Drivers.FineID = Fines.FineID
GROUP BY DriverID
HAVING SUM(Amount) > 5000;

-- 2. Кареллированный подзапрос
-- Вывести список водителей, у которых штраф больше чем у других водителей,
-- которых оштрафовали за скорость
SELECT DriverID
FROM Drivers JOIN Fines ON Drivers.FineID = Fines.FineID
WHERE Amount > ALL (SELECT Amount
                    FROM Fines
                    WHERE FineType = 'За скорость');

-- 3. Поисковый case
-- Получить список клиентов, у которых модел машины "Газель"
SELECT FIO,
    CASE WHEN Model = 'Газель' THEN 'Газель!!!!'
    ELSE  'Не газель :('
    END AS gazel
FROM Drivers JOIN DC ON DC.DriverID = Drivers.DriverID
             JOIN Cars ON DC.CarID = Cars.CarID;

-- Задание 3
-- Процедуру, которая ищет все таблицы с определенным  столбцом
-- имя столбца - параметр
DROP PROCEDURE IF EXISTS findTable(VARCHAR);

CREATE PROCEDURE findTable(column_name VARCHAR)
AS $$
BEGIN
    SELECT table_name FROM information_schema.columns
    WHERE information_schema.columns.COLUMN_NAME LIKE concat('%',$1,'%');
END;
$$
LANGUAGE 'plpgsql';

call findTable('client_id');

--  SELECT table_name FROM information_schema.columns t
--  WHERE t.COLUMN_NAME LIKE concat('%',$1,'%');