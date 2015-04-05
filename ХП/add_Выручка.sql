CREATE PROCEDURE add_Выручка
@Реализация MONEY,
@Внереализ MONEY,
@Операция VARCHAR(80),
@Номер_док VARCHAR(30),
@Наим_док VARCHAR(100),
@Дата_док DATETIME
AS
DECLARE @IDdoc INT

IF (@Номер_док IS NOT NULL)
	BEGIN
EXEC add_Документ @Номер_док, @Наим_док, @Дата_док, @IDdoc output;
	END
INSERT INTO Выручка (Выручка_от_реализации, Внереализационные_доходы, Содержание_операции, Дата_записи,	Документ_выручка, DEL)
VALUES (@Реализация, @Внереализ, @Операция, GETDATE(), @IDdoc, 0)
