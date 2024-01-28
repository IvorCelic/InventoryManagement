use master;
drop database if exists Inventura;
go
create database InventoryManagment;
go
alter database InventoryManagment collate Croatian_CI_AS;
go
use InventoryManagment;
go


create table Persons (
    Id int primary key identity (1, 1) not null,
    FirstName varchar(20) not null,
    LastName varchar(20) not null,
    Email varchar(50) not null,
    Password varchar(100) not null
);

create table Products (
    Id int primary key identity (1, 1) not null,
    Name varchar(50) not null,
    Description varchar(100) null,
    IsUnitary bit not null,
    Person int references OSOBE (Osoba_ID) not null
);

create table Locations (
    Id int primary key identity (1, 1) not null,
    Name varchar(50) not null,
    Description varchar(100) null
);

create table ProductsLocations (
    Id int primary key identity (1, 1) not null,
    Quantity int not null,
    Price decimal not null,
    Description varchar(255) null,
    Product int references SREDSTVA (Sredstvo_ID) not null,
    Location int references LOKACIJE (Lokacija_ID) not null,
    Person int references OSOBE (Osoba_ID) not null
);




---------- INSERTI ----------

insert into Persons (FirstName, LastName, Email, Password) values
    ('Ivor', 'Ćelić', 'ivorcelic@gmail.com', 'lozinka'),
    ('Tomislav', 'Jakopec', 'tjakopec@gmail.com', 'TomoCar123'),
    ('Pero', 'Đurić', 'peroduric@gmail.com', 'lozinka');

insert into SREDSTVA (Name, Description, IsUnitary, Person) values
    ('Cannon LBP6650', 'Printer crno-bijeli', 0, 1),
    ('T-shirt', 'Majice sa EPSO 2023', 1, 2),
    ('Rama za 100m', 'Drvena rama', 1, 1);

insert into LOKACIJE (Name, Description) values
    ('46', 'El.mete, ekrani, televizori'),
    ('41', 'Metalurgija, rame...'),
    ('24', 'Printeri, monitori, stativi, projektori, mete, uredski materijal...'),
    ('26', 'Alati');

insert into ProductsLocations (Quantity, Price, Description, Product, Location, Person) values
    (1, 1000, null, 1, 3, 1),
    (200, 5, '5xM, 10xS, 25xXS', 2, 3, 1),
    (40, 5, '20 komada u neiskoristivom stanju', 3, 2, 2);




---------- SELECT ----------

select a.IsUnitary, a.Price, a.Description, b.Name as 'Naziv sredstva', c.Name as 'Skladište', CONCAT(d.FirstName, ' ', d.LastName) as 'Osoba'
from ProductsLocations a
inner join Products b on a.Id=b.Id
inner join Locations c on a.Id=c.Id
inner join Persons d on a.Id=d.Id