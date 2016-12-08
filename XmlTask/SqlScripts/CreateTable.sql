USE XmlTaskBd
GO

CREATE TABLE Files
(
	Id smallint IDENTITY NOT NULL,
	Name varchar(20) NOT NULL,
	FileVersion varchar(20) NULL,
	DateTime Date NULL
)
GO