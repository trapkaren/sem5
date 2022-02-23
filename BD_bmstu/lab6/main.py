import psycopg2

'''Список тарооогов с id от и до'''
def scalarSQLRequest(cursor):
    first_in = input('Введите нижнюю границу id: ')
    second_in = input('Введите верхнюю границу id: ')
    cursor.execute('''
                    SELECT *
                    FROM tarologists
                    WHERE id_tarologist BETWEEN %s and %s;
                ''', (first_in, second_in))
    for row in cursor.fetchall():
        for i in range(len(row)):
            print(row[i], end=' ')
        print()

'''Получить список тарологов, оказывающих услуги, в описании
которых присутствует предложение Совет от карт'''
def multiplesJoinsSQLRequest(cursor):
    cursor.execute('''SELECT tarologist_name
                      FROM tarologists JOIN ts on tarologists.id_tarologist = ts.id_tarologist
                                    JOIN services on ts.id_service = services.id_service
                      WHERE description LIKE '%Совет от карт%';''')
    for row in cursor.fetchall():
        for i in range(len(row)):
            print(row[i], end=' ')
        print()

'''Min, max и сумма цен услуг по уровню тарологов'''
def cteSQLRequest(cursor):
    cursor.execute('''SELECT tarologists.id_tarologist, tarologists.level, services.price,
                             MAX(price) OVER (PARTITION BY level) as tmp1,
                             MIN(price) OVER (PARTITION BY level) as tmp2,
                             SUM(price) OVER (PARTITION BY level) as tmp3
                      FROM services JOIN ts on services.id_service = ts.id_service
                      JOIN tarologists on ts.id_tarologist = tarologists.id_tarologist;''')
    for row in cursor.fetchall():
        for i in range(len(row)):
            print(row[i], end=' ')
        print()

'''Список всех существующих таблиц'''
def metadataSQLRequest(cursor):
    cursor.execute('SELECT tablename ' +
                   'FROM pg_tables ' +
                   'WHERE schemaname = %(type)s', {"type":"public"})
    records = cursor.fetchall()
    print('\n')
    print('Список таблиц "public":')
    for row in records:
        print(row[0])

'''Cредний баланс для пользователей с женским полом'''
def callSQLScalarFunction(cursor):
    print('Cредний баланс для пользователей с женским полом: ')
    cursor.execute('SELECT * ' +
    'FROM AverageBalance(%(sex)s)', {"sex":"Ж"})
    for row in cursor.fetchall():
        for i in range(len(row)):
            print(row[i], end=' ')
        print()

'''Информация о тарологах из Химок'''
def callSQLTableFunction(cursor):
    cursor.execute('SELECT * ' +
    'FROM GetTableTarologistsfromPlace(%(text)s)', {"text":"Химки"})
    print('\n')
    print('Информация о тарологах Химок:')
    for row in cursor.fetchall():
        for i in range(len(row)):
            print(row[i], end=' ')
        print()

'''У таролога с айди  430 поменять адрес'''
def callSQLScalarProcedure(cursor):
    cursor.execute('''
    CALL changeAddressTarologists(%(text)s, %(need_id)s);
    SELECT * FROM Tarologists WHERE id_tarologist = %(need_id)s;
    ''', {'text' : 'Москва, Циолковского 7, 130', 'need_id' : '430'})
    for row in cursor.fetchall():
        for i in range(len(row)):
            print(row[i], end=' ')
        print()

def systemSQLFunction(cursor):
    cursor.execute('SELECT version()')
    record = cursor.fetchall()
    print('Версия PostgreSQL:')
    print(record[0][0])

def createSQLTable(cursor, conn):
    cursor.execute('CREATE TABLE IF NOT EXISTS clientPassport ' +
    '(id_client INT NOT NULL, client_name TEXT NOT NULL)')
    conn.commit()

def fillSQLTable(cursor, conn):
    text1 = "17284727"
    text2 = "Кариночка"
    cursor.execute("INSERT INTO clientPassport (id_client) VALUES (%s)",
    text1)
    conn.commit()

def menu(cursor, conn):
    while True:
        print('\n1 - Выполнить скалярный запрос;')
        print('2 - Выполнить запрос с несколькими соединениями (JOIN);')
        print('3 - Выполнить запрос с ОТВ (CTE) и оконными функциями;')
        print('4 - Выполнить запрос к метаданным;')
        print('5 - Вызвать скалярную функцию (написанную в третьей лабораторной работе);')
        print('6 - Вызвать многооператорную или табличную функцию (написанную в третьей лабораторной работе);')
        print('7 - Вызвать хранимую процедуру (написанную в третьей лабораторной работе);')
        print('8 - Вызвать системную функцию или процедуру;')
        print('9 - Создать таблицу в базе данных, соответствующую тематике БД;')
        print('10 - Выполнить вставку данных в созданную таблицу с использованием инструкции INSERT или COPY.')
        print('\n0 - Выход\n')

        operNum = int(input('Введите номер операции: '))
        if operNum == 0:
            return
        elif operNum == 1:
            scalarSQLRequest(cursor)
        elif operNum == 2:
            multiplesJoinsSQLRequest(cursor)
        elif operNum == 3:
            cteSQLRequest(cursor)
        elif operNum == 4:
            metadataSQLRequest(cursor)
        elif operNum == 5:
            callSQLScalarFunction(cursor)
        elif operNum == 6:
            callSQLTableFunction(cursor)
        elif operNum == 7:
            callSQLScalarProcedure(cursor)
        elif operNum == 8:
            systemSQLFunction(cursor)
        elif operNum == 9:
            createSQLTable(cursor, conn)
        elif operNum == 10:
            fillSQLTable(cursor, conn)
        else:
            return


conn = psycopg2.connect(dbname='postgres', user='trapkaren',
                        password='mypassword', host='localhost')
cursor = conn.cursor()

menu(cursor, conn)

cursor.close()
conn.close()