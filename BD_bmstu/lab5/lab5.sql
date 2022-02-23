-- ЛАБ. 5
-----------------------------------------------------
-- 1. Создать JSON из имеющейся таблицы
COPY
(
	SELECT row_to_json(clients_data)
    FROM
    (
        SELECT *
        FROM clients
    ) clients_data
) TO '/Users/trapkaren/Desktop/BD_bmstu/lab5/clients.json';

COPY
(
	SELECT row_to_json(tarologists_data)
    FROM
    (
        SELECT *
        FROM tarologists
    ) tarologists_data
) TO '/Users/trapkaren/Desktop/BD_bmstu/lab5/tarologists.json';

-- 2. Создать таблицу из JSON файла (она идентична уже созданной в 1 ЛР)
DROP PROCEDURE IF EXISTS tableBasedOnJSON();

CREATE OR REPLACE PROCEDURE tableBasedOnJSON()
AS
$$
BEGIN
    CREATE TABLE IF NOT EXISTS ClientsJSON (
    id_client SERIAL PRIMARY KEY,
    client_name CHARACTER VARYING(64) NOT NULL,
    birthday DATE,
    sex SEX,
    balance MONEY,
    -- ограничение на дату (минимум 17 лет)
    CONSTRAINT rest CHECK(age(CURRENT_DATE, birthday)>='17 years'::interval)
    );

    DELETE FROM ClientsJSON;

    CREATE TABLE IF NOT EXISTS ClientsJSONtmp (
        clientsInfo jsonb
    );

   -- DELETE FROM ClientsJSONtmp;

    COPY ClientsJSONtmp
    FROM '/Users/trapkaren/Desktop/BD_bmstu/lab5/clients.json';

    INSERT INTO ClientsJSON(id_client, client_name, birthday,
                sex, balance)
        SELECT (clientsInfo -> 'id_client')::INT AS id_client,
            (clientsInfo ->> 'client_name') AS client_name,
            (clientsInfo ->> 'birthday')::DATE AS birthday,
            (clientsInfo ->> 'sex')::SEX AS sex,
            (clientsInfo ->> 'balance')::MONEY AS balance
        FROM ClientsJSONtmp;

    --DROP TABLE ClientsJSONtmp;
END;
$$
LANGUAGE PLPGSQL;

DROP TABLE IF EXISTS clientsjson;

CALL tableBasedOnJSON();

-- 3. создат таблицу "пасспорт клиента" с JSON-данными внутри

DROP TABLE IF EXISTS clientPassport;

CREATE TABLE clientPassport (
    id_client INT NOT NULL,
    client_name JSON NOT NULL
);

INSERT INTO clientPassport
VALUES (19374, '{"forename": "Карина", "surname": "Искакова"}'),
       (19265, '{"forename": "Дмитрий", "surname": "Куликов"}'),
       (14901, '{"forename": "Анастасия", "surname": "Боренко"}');

-- 4. Выполнить следующие действия:
-- 4.1. Извлечь JSON фрагмент из JSON документа
-- Создала мага и извлекла его уровень
DROP FUNCTION IF EXISTS extractJSON();

CREATE OR REPLACE FUNCTION extractJSON()
    RETURNS TABLE(level JSON, type TEXT)
AS
$$
BEGIN
    CREATE TABLE IF NOT EXISTS magician
    (
        magician_info JSON
    );
    --DELETE FROM magician;
    COPY magician
    FROM '/Users/trapkaren/Desktop/BD_bmstu/lab5/magician.json';


    RETURN query
        SELECT magician_info #> '{level}' #> '{dangerous}' as "dangerous", json_typeof(magician_info #> '{level}' #> '{dangerous}') AS "type"
        FROM magician;

    DROP TABLE magician;
END;
$$
LANGUAGE PLPGSQL;

SELECT * FROM extractJSON();

-- 4.2.
-- Извлечь значения конкретных узлов или атрибутов JSON документа (magician)
DROP FUNCTION IF EXISTS extractExactValue();

CREATE OR REPLACE FUNCTION extractExactValue()
    RETURNS TABLE(level TEXT, type TEXT)
AS
$$
BEGIN
    CREATE TABLE IF NOT EXISTS magician
    (
        magician_info JSON
    );
    --DELETE FROM magician;
    COPY magician
    FROM '/Users/trapkaren/Desktop/BD_bmstu/lab5/magician.json';

    RETURN query
        SELECT magician_info->>'name' AS "magician's name", json_typeof(magician_info->'name') AS "type"
        FROM magician;

    DROP TABLE magician;
END;
$$
LANGUAGE PLPGSQL;

SELECT * FROM extractExactValue();

-- 4.3. Выполнить проверку существования узла или атрибута
DROP PROCEDURE IF EXISTS is_snippet_exist();

CREATE OR REPLACE PROCEDURE is_snippet_exist()
AS
$$
DECLARE
    object_tmp TEXT;
BEGIN
    object_tmp = '';
    CREATE TABLE IF NOT EXISTS magician (
        magician_info JSON
    );
    --DELETE FROM magician;
    COPY magician
    FROM '/Users/trapkaren/Desktop/BD_bmstu/lab5/magician.json';

    SELECT magician_info #>> '{dangerous}'
    INTO object_tmp
    FROM magician;

    IF object_tmp IS NULL THEN raise notice 'Nothing found:(';
    ELSE raise notice 'Search result: %', object_tmp;
    END IF;

    DROP TABLE magician;
END
$$
LANGUAGE PLPGSQL;

call is_snippet_exist();

-- 4.4. Изменить JSON документ
DROP PROCEDURE IF EXISTS edit_json_file();

CREATE OR REPLACE PROCEDURE edit_json_file()
AS
$$
BEGIN
    CREATE TABLE IF NOT EXISTS magician (
        magician_info JSON
    );
    --DELETE FROM magician;
    COPY magician
    FROM '/Users/trapkaren/Desktop/BD_bmstu/lab5/magician.json';

    UPDATE magician
    SET magician_info = '{"name":"Karina","level":{"dangerous": "Big", "skill": "nothing"}}';

    COPY (
	    SELECT magician_info
        FROM magician
    ) TO '/Users/trapkaren/Desktop/BD_bmstu/lab5/magician.json';

    DROP TABLE magician;
END
$$
LANGUAGE PLPGSQL;

CALL edit_json_file();

-- 4.5. Разделить JSON документ на несколько строк по узлам
DROP PROCEDURE IF EXISTS split_json_file();

CREATE OR REPLACE PROCEDURE split_json_file()
    LANGUAGE PLPGSQL
AS
$$
DECLARE
    object_tmp TEXT;
    cursor_json_object CURSOR FOR
    SELECT magician_info
    FROM magician;
BEGIN
    CREATE TABLE IF NOT EXISTS magician (
        magician_info JSONB
    );
    --DELETE FROM magician;
    COPY magician
    FROM '/Users/trapkaren/Desktop/BD_bmstu/lab5/magician.json';

    SELECT jsonb_pretty(magician_info)
    INTO object_tmp
    FROM magician;

    raise notice '%', object_tmp;

    COPY (
	    SELECT jsonb_pretty(magician_info)
        FROM magician
    ) TO '/Users/trapkaren/Desktop/BD_bmstu/lab5/magician_second.json';

    DROP TABLE magician;
END
$$;

CALL split_json_file();

