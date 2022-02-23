-- ЛАБ. 4
-----------------------------------------------------
CREATE EXTENSION plpython3u;

-- 1. Определяемую пользователем скалярную функцию CLR
-- Получение названия услуги по id
DROP FUNCTION IF EXISTS getSeviceNameById(id_service int);

CREATE OR REPLACE FUNCTION getSeviceNameById(id_service int) RETURNS varchar
AS $$
srv = plpy.execute("select * from services")
for services in srv:
    if services['id_service'] == id_service:
        return services['service_name']
return 'None'
$$ LANGUAGE plpython3u;

SELECT * FROM getSeviceNameById(1);

SELECT * FROM services sr WHERE id_service = 1;

-- 2. Пользовательскую агрегатную функцию CLR
-- Сколько тарологов проживают в определенном городе (для примера: Москва)
DROP FUNCTION IF EXISTS GetNumOfTLivingAt(address varchar);

CREATE OR REPLACE FUNCTION GetNumOfTLivingAt(address varchar) returns numeric
as $$
tr = plpy.execute("select * from tarologists")
summ = 0
for l in tr:
    if address in l['address']:
        summ += 1
return summ
$$ language plpython3u;

SELECT * FROM GetNumOfTLivingAt('Москва')

SELECT COUNT(id_tarologist)
FROM tarologists
WHERE address LIKE '%Москва%';

-- 3.Определяемую пользователем табличную функцию CLR
-- Возвращает всех клиентов с введенным полом
DROP FUNCTION IF EXISTS getClientsBySex(sex sex);

CREATE OR REPLACE FUNCTION getClientsBySex(sex sex)
RETURNS TABLE (id_client int, client_name varchar, sex sex)
AS $$
ppl = plpy.execute("select * from clients")
res = []
for c in ppl:
    if c['sex'] == sex:
        res.append([c['id_client'], c['client_name'], c['sex']])
return res
$$ language plpython3u;

SELECT * FROM getClientsBySex('Ж');

-- 4. Определяемый пользователем тип данных CLR.
-- Тип престижа клиентов
DROP TYPE IF EXISTS prestige CASCADE;

CREATE TYPE prestige AS (
  client_name CHARACTER VARYING(64),
  sex VARCHAR,
  balance MONEY
);

-- Вывод параметров клиента по id
DROP FUNCTION IF EXISTS getPrestigeById(id integer);

CREATE OR REPLACE FUNCTION getPrestigeById(id integer) RETURNS prestige
as $$
plan = plpy.prepare("select client_name, sex, balance from clients where id_client = $1", ["int"])
cr = plpy.execute(plan, [id])
return (cr[0]['client_name'], cr[0]['sex'], cr[0]['balance'])
$$ language plpython3u;

SELECT * FROM getPrestigeById(120);

SELECT *
FROM clients
WHERE id_client = 120;

-- 5. Хранимую процедуру CLR,
-- Добавляет нового таролога
-- Новый адрес для таролога с опр. id
DROP PROCEDURE IF EXISTS changeAddressT(VARCHAR, INTEGER);

CREATE PROCEDURE changeAddressT(address VARCHAR, id_tarologist INTEGER) AS
$$
plan = plpy.prepare("update tarologists set address = $1 where id_tarologist = $2;", ["varchar", "int"])
plpy.execute(plan, [address, id_tarologist])
$$
LANGUAGE plpython3u;

CALL changeAddressT('Москва, Циолковского 7, 130', 755);

SELECT *
FROM tarologists
WHERE id_tarologist = 755;

-- 6. Триггер CLR
-- Надпись при удалении босса
DROP FUNCTION IF EXISTS afterDeleteBoss() CASCADE;

CREATE FUNCTION afterDeleteBoss()
RETURNS TRIGGER AS
$$
    if TD['event'] == 'DELETE':
        plpy.notice(f"Э, нельзя удалять босса")
$$
LANGUAGE plpython3u;

CREATE TRIGGER companyDeleteBoss
    AFTER DELETE
    ON company
    FOR EACH ROW
    EXECUTE PROCEDURE afterDeleteBoss();

DELETE FROM company
WHERE id_chief = 4003;

