SELECT * FROM Students;
SELECT * FROM Faculty;
SELECT * FROM Direction;

Declare @n nvarchar(10)
set @n = null
SELECT Name, Gender, [Address], FName, DName 
FROM Students S
JOIN Direction D ON S.DID = D.DID
JOIN Faculty F ON F.Fid = D.FID
WHERE ([Name] LIKE '%salim%' OR 'salim'='')

SELECT Name, Gender, [Address], FName, DName 
            FROM Students S
            JOIN Direction D ON S.DID = D.DID
            JOIN Faculty F ON F.Fid = D.FID
            WHERE ([Name] LIKE '%salim%' OR 'SALIM'='')


SELECT * FROM Faculty;

SELECT * FROM Rooms;
SELECT * FROM Building;

SELECT RNum AS [Xona raqami], Price AS Narxi, [Brom qilingan] = CASE WHEN IsBromed=1 THEN 'HA' ELSE 'Yo"q' END,
BID AS [Bino raqami], Rooms AS [Xonalar soni], [Address] AS [Manzil]
FROM Rooms R 
INNER JOIN Building B ON R.BNum=B.BID

SELECT * FROM Payment;

SELECT Name, FName, DName, StartDate AS [Brom qilingan kun], EndDate AS [Oxirgi muddat], Oy=DATEDIFF(MONTH, StartDate, EndDate),
Payed AS [To`landi]
FROM Students S
JOIN Direction D ON S.DID = D.DID
JOIN Faculty F ON F.Fid = D.FID
JOIN Payment P ON P.[SID] = S.Id

SELECT [Name] AS FIO, FName AS Fakultet, DName AS [Yo`nalish], P.RNum AS Xona, 
StartDate AS [Brom qilingan kun], EndDate AS [Oxirgi muddat], [Oy miqdori]=DATEDIFF(MONTH, StartDate, EndDate),
Payed AS [To`landi], Price AS Narxi, Summa=(Price*Months), Qarzdorlik=(Price*Months)-Payed
FROM Payment P INNER JOIN Students S ON P.[SID] = S.Id
INNER JOIN Rooms R ON P.RNum = R.RNum
INNER JOIN Building B ON B.BID = R.BNum
INNER JOIN Direction D ON D.DID = S.DID
INNER JOIN Faculty F ON F.Fid = D.FID

-- Brom taxlash kerak
SELECT * FROM Faculty;
SELECT * FROM Direction;

INSERT INTO Direction VALUES(3, 2, 'Matematika')

SELECT FName, DName FROM Faculty F full JOIN Direction D ON F.Fid=D.FID;

