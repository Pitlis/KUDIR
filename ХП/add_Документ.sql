CREATE PROCEDURE add_Документ
@Номер_док VARCHAR(30),
@Наим_док VARCHAR(100),
@Дата_док DATETIME,
@IDRESULT INT OUTPUT
AS
INSERT INTO Документ (Номер, Наименование, Дата, DEL) VALUES (@Номер_док, @Наим_док, @Дата_док, 0)
SET @IDRESULT = SCOPE_IDENTITY() 