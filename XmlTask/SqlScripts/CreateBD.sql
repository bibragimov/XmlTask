Create database XmlTaskBd
ON
(
NAME = 'XmlTaskBd',
FileName = 'C:\Base\XmlTaskBd.mdf',
SIZE=10MB,
MAXSIZE=100MB,
FILEGROWTH=10MB
)
LOG ON
(
NAME = 'XmlTaskBdLog',
FileName = 'C:\Base\XmlTaskBd.ldf',
SIZE=10MB,
MAXSIZE=50MB,
FILEGROWTH=5MB
)
