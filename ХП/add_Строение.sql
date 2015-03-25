CREATE PROCEDURE add_Строение
@Название VARCHAR(150),
@Адрес VARCHAR(200),
@Дата_приобр DATETIME,
@Дата_гос_регистр DATETIME,
@Дата_ввод_экспл DATETIME,
@Дата_выбытие DATETIME,
@Право TINYINT,
@ДП_номер VARCHAR(30),
@ДП_Наим VARCHAR(100),
@ДП_Дата DATETIME,
@ДР_номер VARCHAR(30),
@ДР_Наим VARCHAR(100),
@ДР_Дата DATETIME,
@ДВ_номер VARCHAR(30),
@ДВ_Наим VARCHAR(100),
@ДВ_Дата DATETIME
AS
DECLARE @ID_дп INT, @ID_др INT, @ID_дв INT

if(@ДП_номер IS NOT NULL)
	BEGIN
		EXEC add_Документ @ДП_номер, @ДП_Наим, @ДП_Дата, @ID_дп output
	END
	
if(@ДР_номер IS NOT NULL)
	BEGIN
		EXEC add_Документ @ДР_номер, @ДР_Наим, @ДР_Дата, @ID_др output
	END
	
if(@ДВ_номер IS NOT NULL)
	BEGIN
		EXEC add_Документ @ДВ_номер, @ДВ_Наим, @ДВ_Дата, @ID_дв output
	END
	
INSERT INTO Строение (Наименование, Адрес, Дата_приобретения, Дата_гос_регистрации, Дата_ввода_в_эксплуатацию, Дата_выбытия, Право, Документ_приобретение, Документ_регистрация, Документ_выбытие, DEL)
VALUES (@Название, @Адрес, @Дата_приобр, @Дата_гос_регистр, @Дата_ввод_экспл, @Дата_выбытие, @Право, @ID_дп, @ID_др, @ID_дв, 0)