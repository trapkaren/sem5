from playhouse.db_url import connect
import peewee as pw

db = connect("postgresext://trapkaren:tCH@4b5H@localhost:5432/postgres")

class BaseModel(pw.Model):
   class Meta:
       database = db

class Drivers(BaseModel):
    DriverID = pw.PrimaryKeyField()
    DriverLicense = pw.CharField()
    FIO = pw.CharField()
    Phone = pw.CharField()

class Fines(BaseModel):
    FineID = pw.PrimaryKeyField()
    FineType = pw.CharField()
    Amount = pw.FloatField()
    FineDate = pw.DateField()
    DriverID = pw.ForeignKeyField(Drivers, on_delete="cascade")

class Cars(BaseModel):
    CarID = pw.PrimaryKeyField()
    Model = pw.CharField()
    Color = pw.CharField()
    Year = pw.IntegerField()
    RegistrationDate = pw.DateField()

class DC(BaseModel):
    DriverID = pw.ForeignKeyField(Drivers, on_delete="cascade")
    CarID = pw.ForeignKeyField(Cars, on_delete="cascade")

def task_1():
   cursor = db.execute_sql("SELECT FineType, Phone, Model\
   FROM Fines JOIN Drivers on Fines.DriverID = Drivers.DriverID JOIN DC ON DC.DriverID = Drivers.DriverID JOIN Cars ON DC.CarID = Cars.CarID")

   for row in cursor.fetchall():
       print(row)

   cursor = Fines.select(Fines.FineType, Drivers.Phone, Cars.Model).join(Drivers).join(DC).join(Cars)

   for row in cursor:
       print(row)

def task_2():
    cursor = db.execute_sql("SELECT DriverID\
    FROM Fines JOIN Drivers on Fines.DriverID = Drivers.DriverID\
    WHERE DATE_PART('year', FineDate) = 2021")

    for row in cursor.fetchall():
        print(row)

    cursor = Fines.select(Drivers.DriverID).join(Drivers).where(Fines.FineDate.year == 2021)

    for row in cursor:
        print(row)

def task_3():
    cursor = db.execute_sql("SELECT CarID\
                            FROM Cars JOIN DC ON DC.CarID = Cars.CarID JOIN Drivers ON DC.DriverID = Drivers.DriverID\
                            GROUP BY Cars.CarID\
                            HAVING count(Drivers.DriverID) <= 3")

    for row in cursor.fetchall():
        print(row)

    cursor = Cars.select(Cars.CarID).join(DC).join(Drivers).group_by(Cars.CarID).having(pw.fn.Count(Drivers.DriverID) <= 3)

    for row in cursor:
        print(row)

def main():
   task_1()
   task_2()
   task_3()


if __name__ == "__main__":
   main()