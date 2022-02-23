-- ЛАБ. 1
-----------------------------------------------------
DROP TABLE IF EXISTS tarologists CASCADE;
DROP TABLE IF EXISTS clients CASCADE;
DROP TABLE IF EXISTS predictions CASCADE;
DROP TABLE IF EXISTS services CASCADE;
DROP TABLE IF EXISTS ts CASCADE;
DROP TABLE IF EXISTS cards CASCADE;

DROP TYPE IF EXISTS SEX;

CREATE TYPE SEX AS ENUM ('М', 'Ж', 'Иное');

CREATE TABLE IF NOT EXISTS tarologists
(
    id_tarologist SERIAL PRIMARY KEY,
    tarologist_name CHARACTER VARYING(64) NOT NULL,
    address CHARACTER VARYING(100),
    phone_number VARCHAR(20)
);

CREATE TABLE IF NOT EXISTS services
(
    id_service SERIAL PRIMARY KEY,
    service_name CHARACTER VARYING(64) NOT NULL,
    description CHARACTER VARYING(500),
    price MONEY
);

CREATE TABLE IF NOT EXISTS ts ();

ALTER TABLE ts ADD COLUMN id_tarologist INTEGER;
ALTER TABLE ts ADD COLUMN id_service INTEGER;

ALTER TABLE ts
   ADD CONSTRAINT fk_id_t
   FOREIGN KEY (id_tarologist)
   REFERENCES tarologists(id_tarologist);

ALTER TABLE ts
   ADD CONSTRAINT fk_id_s
   FOREIGN KEY (id_service)
   REFERENCES services(id_service);

CREATE TABLE IF NOT EXISTS clients
(
    id_client SERIAL PRIMARY KEY,
    client_name CHARACTER VARYING(64) NOT NULL,
    birthday DATE,
    sex SEX,
    balance MONEY,
    -- ограничение на дату (минимум 17 лет)
    CONSTRAINT rest CHECK(age(CURRENT_DATE, birthday)>='17 years'::interval)
);

ALTER TABLE clients ADD COLUMN id_tarologist INTEGER;

ALTER TABLE clients
   ADD CONSTRAINT fk_id_t
   FOREIGN KEY (id_tarologist)
   REFERENCES tarologists(id_tarologist);

CREATE TABLE IF NOT EXISTS cards
(
    id_card SERIAL PRIMARY KEY,
    number NUMERIC,
    card_name CHARACTER VARYING(64) NOT NULL,
    suit CHARACTER VARYING(20) NOT NULL,
    fortune_telling1 CHARACTER VARYING(300),
    fortune_telling2 CHARACTER VARYING(300),
    fortune_telling3 CHARACTER VARYING(300),
    fortune_telling4 CHARACTER VARYING(300)
);

CREATE TABLE IF NOT EXISTS predictions ();

ALTER TABLE predictions ADD COLUMN id_card INTEGER;
ALTER TABLE predictions ADD COLUMN id_client INTEGER;

ALTER TABLE predictions
   ADD CONSTRAINT fk_id_c
   FOREIGN KEY (id_card)
   REFERENCES cards(id_card);

ALTER TABLE predictions
   ADD CONSTRAINT fk_id_cl
   FOREIGN KEY (id_client)
   REFERENCES clients(id_client);

-- Для защиты ЛАБ.1
ALTER TABLE tarologists ADD COLUMN level SMALLINT;

copy tarologists(id_tarologist, tarologist_name, address, phone_number, level) from '/Users/trapkaren/Desktop/BD_bmstu/lab1/tarologists.csv'
delimiter ',' csv;

copy clients(id_client, client_name, birthday, sex, balance, id_tarologist) from '/Users/trapkaren/Desktop/BD_bmstu/lab1/clients.csv'
delimiter ',' csv;

copy cards(id_card, number, card_name, suit, fortune_telling1, fortune_telling2, fortune_telling3, fortune_telling4) from '/Users/trapkaren/Desktop/BD_bmstu/lab1/tarot.csv'
delimiter ',' csv;

copy predictions(id_card, id_client) from '/Users/trapkaren/Desktop/BD_bmstu/lab1/predictions.csv'
delimiter ',' csv;

copy services(id_service, service_name, description, price) from '/Users/trapkaren/Desktop/BD_bmstu/lab1/services.csv'
delimiter ',' csv;

copy ts(id_tarologist, id_service) from '/Users/trapkaren/Desktop/BD_bmstu/lab1/ts.csv'
delimiter ',' csv;

-- Если надо удалить столбццы
-- ALTER table cards drop column fortune_telling2;
-- ALTER table cards drop column fortune_telling3;
-- ALTER table cards drop column fortune_telling4;

-- ALTER table cards rename column fortune_telling1 to fortune_telling;

-- ЗАЩИТА ЛАБ.1
-----------------------------------------------------

-- К тарологам добавить уровень знаний
DROP TABLE IF EXISTS level_knowledges CASCADE;

CREATE TABLE IF NOT EXISTS level_knowledges
(
    level SMALLINT
);

ALTER TABLE level_knowledges ADD COLUMN id_service INTEGER;

ALTER TABLE level_knowledges
   ADD CONSTRAINT fk_id_s
   FOREIGN KEY (id_service)
   REFERENCES services(id_service);

COPY level_knowledges(level, id_service) FROM '/Users/trapkaren/Desktop/BD_bmstu/lab1/level_knowledges.csv'
DELIMITER ',' CSV ;
