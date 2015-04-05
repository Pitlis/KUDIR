USE [KUDIR1]
GO
/****** Object:  StoredProcedure [dbo].[upd_Выручка]    Script Date: 04/05/2015 18:28:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[upd_Выручка]
@ID INT,
@Реализация MONEY,
@Внереализ MONEY,
@Операция VARCHAR(80),
@Дата_записи DATETIME,
@Номер_док VARCHAR(30),
@Наим_док VARCHAR(100),
@Дата_док DATETIME,
@Док_выручка INT
AS
DECLARE @docID INT, @docIDold INT
IF (@Док_выручка IS NULL) AND (@Номер_док IS NOT NULL) AND (@Дата_док IS NOT NULL)
	BEGIN
		EXECUTE add_Документ @Номер_док, @Наим_док, @Дата_док, @docID output
	END

IF (@Док_выручка IS NOT NULL) AND (@Номер_док IS NOT NULL) AND (@Дата_док IS NOT NULL)
	BEGIN
		EXECUTE upd_Документ @Номер_док, @Наим_док, @Дата_док, @Док_выручка
		SET @docID = @Док_выручка
	END
SET @docIDold = (SELECT Выручка.Документ_выручка FROM Выручка WHERE Выручка.ID = @ID) 
IF (@Док_выручка IS NULL) AND (@docID IS NULL) AND (@Номер_док = '') AND (@docIDold IS NOT NULL)
	BEGIN
		UPDATE Документ SET DEL = 1 WHERE DocumentID = @docIDold
	END

UPDATE Выручка SET Выручка_от_реализации = @Реализация, Внереализационные_доходы = @Внереализ, Дата_записи = @Дата_записи, Содержание_операции = @Операция, Документ_выручка = @docID
WHERE ID = @ID