-- 1) Инструкция SELECT, использующая предикат сравнения
-- Вывести имена клиентов, которым выпала 20-ая карта (Солнце)
SELECT client_name
FROM clients JOIN predictions on clients.id_client = predictions.id_client
WHERE id_card = 20;

-- 2) Инструкция SELECT, использующая предикат BETWEEN.
-- Вывести названия услуг в ценовом диапозоне от 2000 до 8000
SELECT service_name
FROM services
WHERE price::numeric BETWEEN 2000 AND 8000;

-- Вывести список клиентов, чье день рождения между 1990-01-01 и 2003-09-31
SELECT client_name
FROM clients
WHERE birthday BETWEEN '1990-01-01' AND '2003-01-31';

-- 3) Инструкция SELECT, использующая предикат LIKE.
-- Получить список услуг в описании которых присутствует предложение 'Совет от карт'
SELECT service_name
FROM services WHERE description LIKE '%Совет от карт%';

-- Получить список тарологов, оказывающих услуги, в описании
-- которых присутствует предложение 'Совет от карт'
SELECT tarologist_name
FROM tarologists JOIN ts on tarologists.id_tarologist = ts.id_tarologist
JOIN services on ts.id_service = services.id_service
WHERE description LIKE '%Совет от карт%';

-- 4) Инструкция SELECT, использующая предикат IN с вложенным подзапросом
-- Получить имена и пол клиентов, посещавших тарологов из Ростова
SELECT client_name, sex
FROM clients
WHERE clients.id_tarologist IN (SELECT id_tarologist
                                FROM tarologists
                                WHERE address LIKE '%Ростов%');

-- 5) Инструкция SELECT, использующая предикат EXISTS с вложенным
-- подзапросом.
-- Вывести фразу, если существвуют лиенты у которых баланс денег между 10000 и 20000
SELECT 'exist users where balance between'
WHERE EXISTS
(
	SELECT id_client
	from clients
	WHERE balance::numeric BETWEEN 10000 AND 20000
);

-- 6) Инструкция SELECT, использующая предикат сравнения с квантором.
-- Вывести список клиентов, у которых баланс больше чем у других клиентов,
-- которые брали улсуги у тарологов с id от 100 до 300
SELECT client_name
FROM clients
WHERE balance > ALL (SELECT balance
                     FROM clients
                     WHERE clients.id_tarologist BETWEEN 100 AND 300);

-- 7) Инструкция SELECT, использующая агрегатные функции в выражениях
-- столбцов.
-- Вывести средний баланс у клиентов, которые брали услуги  у тарологов
-- с id от 100 до 500
SELECT AVG(balance::numeric)
FROM clients
WHERE clients.id_tarologist BETWEEN 100 AND 500;

-- 8) Инструкция SELECT, использующая скалярные подзапросы в выражениях
-- столбцов.
-- Вывести все данные о клиентах, у которых баланс больше среднего
-- баланса у всех клиентов
SELECT *
FROM clients
WHERE balance::numeric > (SELECT AVG(balance::numeric) FROM clients);

-- 9) Инструкция SELECT, использующая простое выражение CASE.
-- Получить список клиентов женского пола
SELECT id_client, client_name,
    CASE sex
        WHEN 'Ж' THEN 'Женский пол'
        ELSE  'Другой'
    END AS female_sex
FROM clients;

-- 10) Инструкция SELECT, использующая поисковое выражение CASE.
-- Получить список клиентов, которым больше 30
SELECT client_name, birthday,
    CASE WHEN DATE_PART('year', current_date) - DATE_PART('year', birthday) > 30 THEN 'Больше'
    ELSE  'Меньше'
    END AS more_30
FROM clients;

-- 11. Создание новой временной локальной таблицы из результирующего набора
-- данных инструкции SELECT.
-- Создать временную таблицу самых богатых клиентов (баланс > 15000)
CREATE TEMP TABLE
rich_clients AS
SELECT client_name, balance
from clients
WHERE balance::numeric > 15000;

SELECT *
FROM rich_clients;

DROP TABLE rich_clients;

-- 12) Инструкция SELECT, использующая вложенные коррелированные
-- подзапросы в качестве производных таблиц в предложении FROM.
-- Получить список тарологов, оказыающих услугу под номером 5
SELECT tarologist_name, id_tarologist
FROM tarologists t
WHERE 5 IN
    (
        SELECT id_service
        FROM ts
        WHERE t.id_tarologist = ts.id_tarologist
        );
-- вложенный подзапрос не может быть обработан прежде, чем будет
-- обрабатываться внешний подзапрос

-- 13) Инструкция SELECT, использующая вложенные подзапросы с уровнем
-- вложенности 3.
-- Вывести список карт, выпавших клиентам, чей баланс > среднего баланса
-- клиентов, которые были у таролога из Ростова
SELECT predictions.id_card
FROM predictions
WHERE predictions.id_client IN
(
    SELECT id_client
    FROM clients
    WHERE balance::numeric >
          (
              SELECT AVG(balance::numeric)
              FROM clients
              WHERE clients.id_tarologist IN
                    (
                        SELECT id_tarologist
                        FROM tarologists
                        WHERE address LIKE '%Ростов%')
              )
      );

-- 14) Инструкция SELECT, консолидирующая данные с помощью предложения
-- GROUP BY, но без предложения HAVING.
-- Вывести дни рождения клиентов женского пола
SELECT birthday
FROM clients
WHERE sex = 'Ж'
GROUP BY birthday
ORDER BY  birthday

-- 15) Инструкция SELECT, консолидирующая данные с помощью предложения
-- GROUP BY и предложения HAVING.
-- Вывести id клиентов, у которых баланс < 8000
SELECT id_client
FROM clients
GROUP BY id_client
HAVING SUM(balance::numeric) < 8000;

-- 16) Однострочная инструкция INSERT, выполняющая вставку в таблицу одной
-- строки значений.
-- Вставка клиента
INSERT INTO clients VALUES (1001, 'Карина', '2001-01-07', 'Ж', '20000', 1);

-- 17) Многострочная инструкция INSERT, выполняющая вставку в таблицу
-- результирующего набора данных вложенного подзапроса.
--Вставить клиентов из таблицы clients женского пола
INSERT INTO clients(id_client, client_name, birthday, sex, balance)
SELECT id_client + 1000 as id, client_name, birthday, sex, balance
FROM clients
WHERE sex = 'Ж';

-- 18) Простая инструкция UPDATE.
-- Сделать баланс = 30000 у клиентов с id > 1000
UPDATE clients
SET balance = 30000
WHERE id_client > 1000;

-- 19) Инструкция UPDATE со скалярным подзапросом в предложении SET.
-- Поставить максиммальную дату др клиентам с id > 1000
UPDATE clients
SET birthday =
(
    SELECT MAX(birthday)
    FROM clients
)
where id_client > 1000;

-- 20) Простая инструкция DELETE.
-- Удалить клиента с id = 1356
DELETE FROM clients
WHERE id_client = 1356;

-- 21) Инструкция DELETE с вложенным коррелированным подзапросом в
-- предложении WHERE.
-- Удалить клиентов с id > 1000
DELETE FROM clients
WHERE id_client IN
(
    SELECT id_client
    FROM clients
    WHERE id_client > 1000
);

-- 22) Инструкция SELECT, использующая простое обобщенное табличное
-- выражение
--


-- 23) 