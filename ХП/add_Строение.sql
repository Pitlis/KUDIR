CREATE PROCEDURE add_��������
@�������� VARCHAR(150),
@����� VARCHAR(200),
@����_������ DATETIME,
@����_���_������� DATETIME,
@����_����_����� DATETIME,
@����_������� DATETIME,
@����� TINYINT,
@��_����� VARCHAR(30),
@��_���� VARCHAR(100),
@��_���� DATETIME,
@��_����� VARCHAR(30),
@��_���� VARCHAR(100),
@��_���� DATETIME,
@��_����� VARCHAR(30),
@��_���� VARCHAR(100),
@��_���� DATETIME
AS
DECLARE @ID_�� INT, @ID_�� INT, @ID_�� INT

if(@��_����� IS NOT NULL)
	BEGIN
		EXEC add_�������� @��_�����, @��_����, @��_����, @ID_�� output
	END
	
if(@��_����� IS NOT NULL)
	BEGIN
		EXEC add_�������� @��_�����, @��_����, @��_����, @ID_�� output
	END
	
if(@��_����� IS NOT NULL)
	BEGIN
		EXEC add_�������� @��_�����, @��_����, @��_����, @ID_�� output
	END
	
INSERT INTO �������� (������������, �����, ����_������������, ����_���_�����������, ����_�����_�_������������, ����_�������, �����, ��������_������������, ��������_�����������, ��������_�������, DEL)
VALUES (@��������, @�����, @����_������, @����_���_�������, @����_����_�����, @����_�������, @�����, @ID_��, @ID_��, @ID_��, 0)