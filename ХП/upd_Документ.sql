CREATE PROCEDURE upd_Документ
@Номер VARCHAR(30),
@Наименование VARCHAR(100),
@Дата DATETIME,
@ID INT
AS
UPDATE Документ SET Номер = @Номер, Наименование = @Наименование, Дата = @Дата WHERE DocumentID = @ID