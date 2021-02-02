use Northwind;

CREATE TABLE Subjects (
	Id int,
	Name varchar(20),
	Description varchar(100),
	PRIMARY KEY (Id)
	);

CREATE TABLE Rooms (
	Id int,
	Name varchar(20),
	PRIMARY KEY (Id)
	);

CREATE TABLE Lessons (
	Id int,
	Subject_Fid int,
	Room_Fid int,
	[Date] Datetime,
	PRIMARY KEY (Id),
	CONSTRAINT FK_LessonsRooms FOREIGN KEY (Room_Fid)
	REFERENCES Rooms(Id) ON DELETE CASCADE,
	CONSTRAINT FK_LessonsSubjects FOREIGN KEY (Subject_Fid)
	REFERENCES Subjects(Id) ON DELETE CASCADE
	);