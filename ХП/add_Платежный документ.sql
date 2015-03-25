CREATE PROCEDURE add_ПлатежныйДокумент
@Номер VARCHAR(20),
@Дата DATETIME,
@Сумма MONEY,
@IDRESULT INT OUTPUT
AS

INSERT INTO Платежный_документ (Номер_платежного_документа,	Дата, Сумма, DEL)
VALUES(@Номер, @Дата, @Сумма, 0)
SET @IDRESULT = SCOPE_IDENTITY()