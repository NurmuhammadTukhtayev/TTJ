--PROCEDURE FOR INSERT NEW DATA
CREATE PROCEDURE SP_InsertData
@NAME VARCHAR(50),
@GENDER VARCHAR(10),
@ADDRESS VARCHAR(50),
@DNAME VARCHAR(50),
@RNUM INT,
@STARTDATE DATETIME,
@ENDDATE DATETIME,
@MONTHS INT,
@PAYED MONEY
AS
BEGIN 
    BEGIN TRY
        BEGIN TRAN
            DECLARE @SID INT = (SELECT MAX(ID) FROM Students)
            SET @SID = @SID + 1
            DECLARE @DID INT = (SELECT DID FROM Direction WHERE DName=@DNAME)
            INSERT INTO Students VALUES(@SID, @NAME, @GENDER, @ADDRESS, @DID)

            DECLARE @PID INT = (SELECT MAX(ID) FROM Payment)
            SET @PID = @PID + 1
            INSERT INTO Payment
            VALUES(@PID, @SID, @RNUM, @STARTDATE, @ENDDATE, @MONTHS, @PAYED)

            UPDATE Rooms SET IsBromed=1 WHERE RNum = @RNUM
        COMMIT TRAN
    END TRY
    
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ERRORNUM, ERROR_MESSAGE() AS ERRORMESSAGE;
        ROLLBACK TRAN
    END CATCH
END


-- something to help  ******************
SELECT [Name] AS FIO, FName AS Fakultet, DName AS [Yo`nalish], P.RNum AS Xona, 
StartDate AS [Brom qilingan kun], EndDate AS [Oxirgi muddat], [Oy miqdori]=DATEDIFF(MONTH, StartDate, EndDate),
Payed AS [To`landi], Price AS Narxi, Summa=(Price*Months), Qarzdorlik=(Price*Months)-Payed
FROM Payment P INNER JOIN Students S ON P.[SID] = S.Id
INNER JOIN Rooms R ON P.RNum = R.RNum
INNER JOIN Building B ON B.BID = R.BNum
INNER JOIN Direction D ON D.DID = S.DID
INNER JOIN Faculty F ON F.Fid = D.FID
WHERE P.EndDate<GETDATE()


-- view for to show FACULTY AND DIRECTION
CREATE VIEW V_FacDir AS
SELECT FName, DName FROM Faculty F INNER JOIN Direction D ON F.Fid=D.FID;

SELECT * FROM V_FacDir;

-- Procedure for UPDATE ROOMS 
CREATE PROC Update_Rooms
AS
BEGIN
    UPDATE Rooms SET IsBromed = 0
    WHERE RNum IN
    (
        select RNum from Students s
        inner join Payment p
        on s.Id = p.SID
        where EndDate < GETDATE()
    )
END

EXECUTE Update_Rooms;

-- view for student info
CREATE VIEW V_ShowStudent_Info AS
SELECT Name AS FIO, Gender AS Jinsi, [Address] AS Manzil, FName AS [Fakultet nomi], 
DName AS [Yo`nalishi] FROM Students S 
JOIN Direction D ON S.DID = D.DID JOIN Faculty F ON F.Fid = D.FID
JOIN Payment P ON P.SID = S.Id
WHERE EndDate > GETDATE();

SELECT * FROM V_ShowStudent_Info;

-- view for to show all rooms with their buildings
CREATE VIEW V_Building_Rooms AS
SELECT RNum AS [Xona raqami], 
Price AS Narxi, [Brom qilingan] = CASE WHEN IsBromed=1 THEN 'HA' ELSE 'Yoq' END, 
BID AS[Bino raqami], [Address] AS[Manzil] 
FROM Rooms R INNER JOIN Building B ON R.BNum = B.BID;

SELECT * FROM V_Building_Rooms;

-- view for to show payment info
CREATE VIEW V_Payment_Info AS
SELECT [Name] AS FIO, FName AS Fakultet, DName AS [Yo`nalish], P.RNum AS Xona, 
StartDate AS [Brom qilingan kun], EndDate AS [Oxirgi muddat], [Oy miqdori]=DATEDIFF(MONTH, StartDate, EndDate),
Payed AS [To`landi], Price AS Narxi, [To`langan summa]=(Price*Months), Qarzdorlik=(Price*Months)-Payed
FROM Payment P INNER JOIN Students S ON P.[SID] = S.Id
INNER JOIN Rooms R ON P.RNum = R.RNum
INNER JOIN Building B ON B.BID = R.BNum
INNER JOIN Direction D ON D.DID = S.DID
INNER JOIN Faculty F ON F.Fid = D.FID;

SELECT * FROM V_Payment_Info;

--view for to show empty rooms
CREATE VIEW V_Empty_Rooms AS
SELECT RNum AS [Xona raqami], 
Price AS Narxi, BID AS[Bino raqami], [Address] AS[Manzil] 
FROM Rooms R INNER JOIN Building B ON R.BNum = B.BID WHERE IsBromed = 0;

SELECT * FROM V_Empty_Rooms;

--view for dept
CREATE VIEW V_Dept_Info AS
SELECT [Name] AS FIO, FName AS Fakultet, DName AS [Yo`nalish], P.RNum AS Xona, 
StartDate AS [Brom qilingan kun], EndDate AS [Oxirgi muddat], [Oy miqdori]=DATEDIFF(MONTH, StartDate, EndDate),
Payed AS [To`landi], Price AS Narxi, [To`langan summa]=(Price*Months), Qarzdorlik=(Price*Months)-Payed
FROM Payment P INNER JOIN Students S ON P.[SID] = S.Id
INNER JOIN Rooms R ON P.RNum = R.RNum
INNER JOIN Building B ON B.BID = R.BNum
INNER JOIN Direction D ON D.DID = S.DID
INNER JOIN Faculty F ON F.Fid = D.FID 
WHERE (Price*Months)-Payed>0;

SELECT * FROM V_Dept_Info;

-- procedure for search students
CREATE PROC SP_Search
@Name VARCHAR(50),
@DName VARCHAR(50),
@FName VARCHAR(50)
AS
SELECT Name AS FIO, Gender AS Jinsi, [Address] AS Manzil, FName AS [Fakultet nomi], 
DName AS [yo`nalishi] FROM Students S JOIN Direction D ON S.DID = D.DID JOIN Faculty F ON F.Fid = D.FID
WHERE ([Name] LIKE '%' + @Name+ '%' OR @Name is null)
AND (DName=@DName) AND (FName=@FName);

-- archive data
CREATE VIEW V_Archive AS
SELECT [Name] AS FIO, FName AS Fakultet, DName AS [Yo`nalish], P.RNum AS Xona, 
StartDate AS [Brom qilingan kun], EndDate AS [Oxirgi muddat], [Oy miqdori]=DATEDIFF(MONTH, StartDate, EndDate),
Payed AS [To`landi], Price AS Narxi, Summa=(Price*Months), Qarzdorlik=(Price*Months)-Payed
FROM Payment P INNER JOIN Students S ON P.[SID] = S.Id
INNER JOIN Rooms R ON P.RNum = R.RNum
INNER JOIN Building B ON B.BID = R.BNum
INNER JOIN Direction D ON D.DID = S.DID
INNER JOIN Faculty F ON F.Fid = D.FID
WHERE P.EndDate<GETDATE()

select * from V_Archive