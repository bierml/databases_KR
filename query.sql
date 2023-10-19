CREATE DATABASE TR_A_12_19_06
ON (NAME=tr1206_data,
    FILENAME = 'D:\MSSQL\DATA\tr1206_data.mdf',
    SIZE = 5 MB,
    MAXSIZE = 10 MB,
    FILEGROWTH = 20%)
LOG ON
   (NAME=tr1206_log,
    FILENAME = 'D:\MSSQL\DATA\tr1206_log.ldf',
    SIZE = 2 MB,
    MAXSIZE = 10 MB,
    FILEGROWTH = 1 MB);
GO
USE TR_A_12_19_06;
CREATE TABLE Clients(
 Clientid bigint not null identity(1,1) primary key,
 FIO varchar(100) not null,
 Birthdate date not null,
 Phonenum varchar(50) not null,
 Email varchar(50) not null,
 login varchar(50) not null,
 password varchar(50) not null
);
CREATE TABLE Filials(
 Filid bigint not null identity(1,1) primary key, 
 Name varchar(50) not null,
 Address varchar(50) not null 
);
CREATE TABLE Masters(
 Masterid bigint not null identity(1,1) primary key,
 FIO varchar(100) not null,
 Exper int not null,
 Phonenum varchar(50) not null,
 Labcdate date not null,
 Wage money not null,
 Prepdate date not null,
 Wagedate date not null,
 Fil bigint not null references Filials(Filid)
);
CREATE TABLE Servtype(
 Servnum bigint not null identity(1,1) primary key, 
 Servtype varchar(50) not null,
 Cost money not null,
 Masterid bigint not null references Masters(Masterid)
);
CREATE TABLE Servlist( 
 Servid bigint not null identity(1,1) primary key, 
 Status varchar(50) not null, 
 Servtime datetime not null, 
 Disc money not null,
 Servnum bigint not null references Servtype(Servnum),
 Client bigint not null references Clients(Clientid)
);
CREATE TABLE ManagerTable(
 id bigint not null identity(1,1) primary key,
 status int not null,
 login varchar(50) not null,
 password varchar(50) not null
);
CREATE NONCLUSTERED INDEX ServSearch
 ON Servtype(Servtype) 
 INCLUDE (Cost);
CREATE NONCLUSTERED INDEX MastSearch
 ON Masters(FIO)
 INCLUDE (Masterid,Fil);
GO
 CREATE FUNCTION FindFilMas(@fil_ bigint)
 RETURNS TABLE
 AS
 RETURN 
 (SELECT Masterid AS 'Id мастера', 
  FIO AS 'ФИО мастера',
  Exper AS 'Стаж работы',
  Phonenum AS 'Номер телефона',
  Labcdate AS 'Дата закл. труд. договора',
  Wage AS 'Размер зп', 
  Prepdate AS 'Дата предопл.', 
  Wagedate AS 'Дата зп'
  FROM Masters WHERE Fil=@fil_ GROUP BY Masterid,FIO,Exper,Phonenum,Wage,Prepdate,Wagedate,Labcdate 
 )
 GO
CREATE FUNCTION	SumServPrice(@usid bigint)
 RETURNS money
 AS
 BEGIN
  DECLARE @sumpr money
  SELECT @sumpr = SUM(Servtype.Cost) FROM Servtype INNER JOIN Servlist ON Servlist.Servnum = Servtype.Servnum 
  WHERE Servlist.Client=@usid;
  RETURN(@sumpr)
 END;
 GO
 CREATE VIEW Fullservlist AS
  SELECT Servlist.Servid AS 'Id услуги',
  Servlist.Servnum AS 'Номер услуги',
  Servlist.Client AS 'Id клиента',
  Servlist.Status AS 'Статус',
  Servlist.Servtime AS 'Время оказ. услуги',
  Servlist.Disc AS 'Скидка',
  Masters.FIO AS 'ФИО мастера',
  Clients.FIO AS 'ФИО клиента',
  Servtype.Servtype AS 'Вид услуги',
  Servtype.Cost-Servlist.Disc AS 'Цена услуги со скидкой'
  FROM Servlist LEFT JOIN Servtype ON Servlist.Servnum=Servtype.Servnum LEFT JOIN Clients ON Servlist.Client = Clients.Clientid 
  LEFT JOIN Masters ON Servtype.Masterid=Masters.Masterid
  GROUP BY Servlist.Servid,Servlist.Servnum,Servlist.Client,Servlist.Status,Servlist.Servtime,Servlist.Disc,Masters.FIO,Clients.FIO,
  Servtype.Servtype,Servtype.Cost-Servlist.Disc
 GO
CREATE PROCEDURE AddClient(@FIO varchar(100),@Birthdate date,@Phonenum varchar(50),@Email varchar(50),@login varchar(50), @password varchar(50))
 AS
 INSERT INTO Clients(FIO,Birthdate,Phonenum,Email,login,password)
 VALUES(@FIO,@Birthdate,@Phonenum,@Email,@login,@password)
 GO
 CREATE PROCEDURE UpdClient(@Clientid bigint, @FIO varchar(100),@Phonenum varchar(50),@Email varchar(50),@login varchar(50),@password varchar(50))
  AS
  UPDATE Clients
  SET FIO=@FIO,Phonenum=@Phonenum,Email=@Email,login=@login,password=@password
  WHERE Clientid=@Clientid
 GO
 CREATE PROCEDURE DelClient(@Clientid bigint)
  AS
  DELETE FROM Clients
  WHERE Clientid=@Clientid
 GO
CREATE PROCEDURE AddMaster(@FIO varchar(100),@Exper int,@Phonenum varchar(50),@Labcdate date,@Wage money,@Prepdate date,@Wagedate date,@Fil bigint)
 AS
 INSERT INTO Masters(FIO,Exper,Phonenum,Labcdate,Wage,Prepdate,Wagedate,Fil)
 VALUES(@FIO,@Exper,@Phonenum,@Labcdate,@Wage,@Prepdate,@Wagedate,@Fil)
GO
CREATE PROCEDURE UpdMaster(@Masterid bigint,@FIO varchar(100),@Exper int,@Phonenum varchar(50),@Wage money,@Prepdate date,@Wagedate date,@Fil bigint)
 AS
 UPDATE Masters
 SET FIO=@FIO,Exper=@Exper,Phonenum=@Phonenum,Wage=@Wage,Prepdate=@Prepdate,Wagedate=@Wagedate,Fil=@Fil
 WHERE Masterid=@Masterid
GO
CREATE PROCEDURE DelMaster(@Masterid bigint)
 AS
 DELETE FROM Masters
 WHERE Masterid=@Masterid
GO
CREATE PROCEDURE AddServ(@Status varchar(50),@Servtime datetime,@Disc money,@Servnum bigint,@Client bigint)
 AS
 INSERT INTO Servlist(Status,Servtime,Disc,Servnum,Client)
 VALUES(@Status,@Servtime,@Disc,@Servnum,@Client)
GO
CREATE PROCEDURE UpdServ(@Servid bigint,@Status varchar(50),@Servtime datetime,@Disc money,@Servnum bigint,@Client bigint)
 AS
 UPDATE Servlist
 SET Status=@Status,Servtime=@Servtime,Disc=@Disc,Servnum=@Servnum,Client=@Client
 WHERE Servid=@Servid
GO
CREATE PROCEDURE DelServ(@Servid bigint) 
 AS
 DELETE FROM Servlist
 WHERE Servid=@Servid
GO 
CREATE PROCEDURE AddServtype(@Servtype varchar(50),@Cost money,@Masterid bigint)
 AS
 INSERT INTO Servtype(Servtype,Cost,Masterid)
 VALUES(@Servtype,@Cost,@Masterid)
GO
CREATE PROCEDURE UpdServtype(@Servnum bigint,@Servtype varchar(50),@Cost money,@Masterid bigint)
 AS
 UPDATE Servtype
 SET Servtype=@Servtype,Cost=@Cost,Masterid=@Masterid
 WHERE Servnum=@Servnum
GO
CREATE PROCEDURE DelServtype(@Servnum bigint)
 AS
 DELETE FROM Servtype
 WHERE Servnum=@Servnum
GO
CREATE PROCEDURE AddFil(@Name varchar(50),@Address varchar(50))
 AS
 INSERT INTO Filials(Name,Address)
 VALUES(@Name,@Address)
GO
CREATE PROCEDURE UpdFil(@Filid bigint,@Name varchar(50),@Address varchar(50))
 AS
 UPDATE Filials
 SET Name=@Name,Address=@Address
 WHERE Filid=@Filid
GO 
CREATE PROCEDURE DelFil(@Filid bigint)
 AS 
 DELETE FROM Filials
 WHERE Filid=@Filid
GO 
CREATE PROCEDURE AddMan(@status int,@login varchar(50),@password varchar(50))
 AS
 INSERT INTO ManagerTable(status,login,password)
 VALUES(@status,@login,@password)
GO
CREATE PROCEDURE UpdMan(@id bigint,@status int,@login varchar(50),@password varchar(50))
 AS
 UPDATE ManagerTable
 SET status=@status,login=@login,password=@password
 WHERE id=@id
GO
CREATE PROCEDURE DelMan(@id bigint) 
 AS
 DELETE FROM ManagerTable
 WHERE id=@id
GO
CREATE VIEW ClientsList
WITH SCHEMABINDING
AS
SELECT Clientid,FIO,Birthdate,Phonenum,Email,login,password FROM dbo.Clients
GROUP BY Clientid,FIO,Birthdate,Phonenum,Email,login,password
GO
CREATE VIEW MastersList
WITH SCHEMABINDING
AS
SELECT Masterid,FIO,Exper,Phonenum,Labcdate,Wage,Prepdate,Wagedate,Fil FROM dbo.Masters
GROUP BY Masterid,FIO,Exper,Phonenum,Labcdate,Wage,Prepdate,Wagedate,Fil
GO
CREATE VIEW FilList
WITH SCHEMABINDING
AS
SELECT Filid,Name,Address FROM dbo.Filials
GROUP BY Filid,Name,Address
GO
CREATE VIEW ServtypeList
WITH SCHEMABINDING
AS
SELECT Servnum,Servtype,Cost,Masterid FROM dbo.Servtype
GROUP BY Servnum,Servtype,Cost,Masterid
GO
CREATE VIEW ManagerTableList
WITH SCHEMABINDING
AS
SELECT id,status,login,password FROM dbo.ManagerTable
GROUP BY id,status,login,password
GO 
EXEC sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
EXEC sp_configure 'clr enabled', 1;
GO 
RECONFIGURE;
GO
EXEC sp_configure 'clr strict security', 0;
GO
RECONFIGURE;
GO
CREATE ASSEMBLY MyAssembly
FROM 'D:\mylib1.dll'
WITH permission_set = safe;
GO
CREATE FUNCTION CheckN(@str nvarchar(50))
RETURNS bit
AS EXTERNAL NAME MyAssembly.[mylib.CLR_funct].check_ph;
GO
CREATE FUNCTION CheckE(@str nvarchar(50))
RETURNS bit
AS EXTERNAL NAME MyAssembly.[mylib.CLR_funct].check_email;
GO
CREATE TRIGGER Trig_on_Clients ON Clients
AFTER DELETE
AS
BEGIN
 DELETE FROM Servlist WHERE Servlist.Client = (SELECT Clientid FROM DELETED);
END
GO
CREATE TRIGGER Trig_on_Filials ON Filials 
AFTER DELETE 
AS
BEGIN
 DELETE FROM Masters WHERE Masters.Fil = (SELECT Filid FROM DELETED);
END
GO
CREATE TRIGGER Trig_on_Masters ON Masters 
AFTER DELETE
AS
BEGIN 
 DELETE FROM Servtype WHERE Servtype.Masterid = (SELECT Masterid FROM DELETED);
END
GO
CREATE TRIGGER Trig_on_Servtype ON Servtype 
AFTER DELETE 
AS 
BEGIN
 DELETE FROM Servlist WHERE Servlist.Servnum = (SELECT Servnum FROM DELETED);
END 
