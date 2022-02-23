
-- ЛАБ. 3
-----------------------------------------------------
-- 1. Скалярная функция
-- Получить средний баланс для пользователей с нужным полом
DROP FUNCTION IF EXISTS AverageBalance(sex);

CREATE FUNCTION AverageBalance(tmpSex SEX)
RETURNS NUMERIC AS $$
BEGIN
    return(
            SELECT AVG(balance::NUMERIC)
            FROM clients
            WHERE clients.sex = tmpSex
        );
END;
$$
LANGUAGE 'plpgsql';

SELECT AverageBalance('Ж');

-- 2. Подставляемая табличная функция
-- Получить тарологов из нужного места (деревни, например)
DROP FUNCTION IF EXISTS GetTarologistsfromPlace(CHARACTER VARYING(100));

CREATE FUNCTION GetTarologistsfromPlace(CHARACTER VARYING(100))
RETURNS SETOF tarologists
AS $$
BEGIN
	RETURN QUERY (
   		SELECT *
		FROM tarologists
		WHERE address LIKE concat('%',$1,'%')
	);
END;
$$
LANGUAGE 'plpgsql';

SELECT * FROM GetTarologistsfromPlace('д.');

-- 3. Многооператорная табличная функция
-- То же самое, но со именами столбцов
DROP FUNCTION IF EXISTS GetTableTarologistsfromPlace(CHARACTER VARYING(100));

CREATE FUNCTION GetTableTarologistsfromPlace(text CHARACTER VARYING(100))
RETURNS TABLE (
        id_tarologist INT,
        tarologist_name CHARACTER VARYING(64),
        address CHARACTER VARYING(100),
        phone_number VARCHAR(20),
        level SMALLINT
              ) AS $$
begin
	RETURN QUERY (
   		SELECT *
		FROM tarologists
		WHERE tarologists.address LIKE concat('%',text,'%')
	);
end;
$$
LANGUAGE 'plpgsql';

SELECT * FROM GetTableTarologistsfromPlace('д.');

-- 4. Рекурсивная функция или функция с рекурсивной ОТВ
-- Вывод всех подчиненных
DROP FUNCTION IF EXISTS recCompany(need_id INT);

CREATE FUNCTION recCompany(need_id INT)
RETURNS TABLE (id_chef INT, id_subordinate INT)
AS $$
BEGIN
    RETURN QUERY
    SELECT (recCompany(A.id_subordinate)).*
    FROM company A WHERE A.id_chief = need_id;

    RETURN QUERY
    SELECT B.id_chief, B.id_subordinate
    FROM company B
    WHERE B.id_chief = need_id;
END;
$$
LANGUAGE 'plpgsql';

SELECT *
FROM recCompany(4000);

-- 5. Хранимая процедура без параметров или с параметрами
-- Новый адрес для таролога с опр. id
DROP PROCEDURE IF EXISTS changeAddressTarologists(CHARACTER VARYING, INT);

CREATE PROCEDURE changeAddressTarologists(text CHARACTER VARYING(100), need_id INT)
AS $$
BEGIN
    UPDATE tarologists
    SET address = text
    WHERE id_tarologist = need_id;
END;
$$
LANGUAGE 'plpgsql';

CALL changeAddressTarologists('Москва, Циолковского 7, 130', 1);

SELECT *
FROM tarologists
WHERE id_tarologist = 1;


-- 6. Рекурсивная хранимая процедура или хранимая процедура с
-- рекурсивным ОТВ
-- Изменить цену на услуги начиная с определенного id
DROP FUNCTION IF EXISTS recChangePrice(start_id INT);

CREATE FUNCTION recChangePrice(start_id INT)
RETURNS void
AS $$
BEGIN
    UPDATE services
    SET price = price::numeric + 1000
    WHERE id_service = start_id;

    IF (start_id < 16) THEN
        PERFORM * FROM recChangePrice(start_id + 1);
    END IF;
END;
$$
language 'plpgsql';

SELECT *
FROM recChangePrice(1);

SELECT *
FROM services;

-- 7. Хранимая процедура с курсором
-- Изменить адрес на Москву, где start_id <= id <= end_id
DROP PROCEDURE IF EXISTS changeAddressFromTo(start_id INT, end_id INT, need_address CHARACTER VARYING);

CREATE PROCEDURE changeAddressFromTo(start_id INT,
                                     end_id INT,
                                     need_address CHARACTER VARYING(100))
AS $$
    DECLARE
        row RECORD;
        cur CURSOR FOR
        SELECT *
        FROM tarologists
        WHERE id_tarologist >= start_id AND id_tarologist <= end_id;
    BEGIN
        OPEN cur;
        LOOP
            FETCH cur INTO row;
            EXIT WHEN NOT FOUND;
            UPDATE tarologists t
            SET address = need_address
            WHERE t.id_tarologist = row.id_tarologist;
        END LOOP;
        CLOSE CUR;
    END;
$$
language 'plpgsql';

CALL changeAddressFromTo(1, 10, 'Москва, Циолковского 7, 130');

SELECT *
FROM tarologists
WHERE id_tarologist >= 1 AND id_tarologist <= 10;

-- 8. Хранимая процедура доступа к метаданным
-- Посчитать кол-во всех пользовательских триггеров
DROP PROCEDURE IF EXISTS dropUserTriggers(inout cnt INT);

CREATE PROCEDURE dropUserTriggers(inout cnt INT)
AS $$
    DECLARE
        row RECORD;
        cur CURSOR FOR
        SELECT trigger_name, event_object_table
        FROM information_schema.triggers
        WHERE trigger_schema = 'public';
    BEGIN
        OPEN cur;
        LOOP
            FETCH cur INTO row;
            EXIT WHEN NOT FOUND;
            EXECUTE format('DROP TRIGGER IF EXISTS %s ON %s',
            quote_ident(row.trigger_name), row.event_object_table);
            cnt := cnt + 1;
        END LOOP;
        CLOSE cur;
    END;
$$
language 'plpgsql';

CALL dropUserTriggers(0);

-- 9. Триггер AFTER
-- Вывод на кэран в виде ошибки напдписи
-- 'Э, нельзя удалять босса'
-- Если будет удаление строки
DROP FUNCTION IF EXISTS afterDelete() CASCADE;

CREATE FUNCTION afterDelete()
RETURNS TRIGGER
AS $$
    BEGIN
        RAISE EXCEPTION 'Э, нельзя удалять босса';
    END;
$$
language 'plpgsql';

CREATE TRIGGER companyDeleteSmth
    AFTER DELETE
    ON company
    FOR EACH ROW
    EXECUTE PROCEDURE afterDelete();

DELETE FROM company
WHERE id_chief = 4000;

-- SELECT *
-- FROM company
-- WHERE id_chief = 4000;

-- INSERT INTO company VALUES (4000, 4001);

-- 10. Триггер INSTEAD OF
-- Если вызывается удаление, заменить его вставкой
-- новой строки + вывод надписи
DROP FUNCTION IF EXISTS insteadOfDelete2() CASCADE;

DROP VIEW IF EXISTS company_view;

CREATE VIEW company_view AS
SELECT *
FROM company;

CREATE FUNCTION insteadOfDelete2()
RETURNS TRIGGER
AS $$
    BEGIN
        INSERT INTO company VALUES (1, 4003);
        RAISE USING MESSAGE = 'Мы добавили нового босса, знакомьтесь, 1';
    END;
$$
language 'plpgsql';

CREATE TRIGGER companyDeleteSmth2
    INSTEAD OF DELETE
    ON company_view
    FOR EACH ROW
    EXECUTE PROCEDURE insteadOfDelete2();

DELETE FROM company_view
WHERE id_chief = 4000;

-- ЗАЩИТА ЛАБ. 3
-----------------------------------------------------
-- процедура весь список услуг таролога по id или имени
DROP FUNCTION IF EXISTS getAllServices(tmp_id INT);

CREATE FUNCTION getAllServices(tmp_id INT)
RETURNS TABLE (id_tarologists INT, service_name CHARACTER VARYING(64))
AS $$
BEGIN
    RETURN QUERY (
    SELECT tmp_id, services.service_name
    FROM services JOIN ts on services.id_service = ts.id_service
                  JOIN tarologists ON ts.id_tarologist = tarologists.id_tarologist
    WHERE tarologists.id_tarologist = tmp_id
    );
END;
$$
LANGUAGE 'plpgsql';

SELECT * FROM getAllServices(4);
