use master;
drop database if exists Inventura;
go
create database Inventura;
go
use Inventura;
go


create table OSOBE (
    Osoba_ID int primary key identity (1, 1) not null,
    Ime varchar(20) not null,
    Prezime varchar(20) not null,
    Email varchar(50) not null,
    Lozinka varchar(50) not null
);

create table SREDSTVA (
    Sredstvo_ID int primary key identity (1, 1) not null,
    Naziv varchar(50) not null,
    Opis varchar(100) null,
    Komadno bit not null,
    Osoba_ID int references OSOBE (Osoba_ID) not null
);

create table LOKACIJE (
    Lokacija_ID int primary key identity (1, 1) not null,
    Naziv varchar(50) not null,
    Opis varchar(100) null
);

create table SREDSTVA_LOKACIJE (
    Sredstvo_lokacija_ID int primary key identity (1, 1) not null,
    Kolicina int not null,
    Cijena decimal not null,
    Opis varchar(255) null,
    Sredstvo_ID int references SREDSTVA (Sredstvo_ID) not null,
    Lokacija_ID int references LOKACIJE (Lokacija_ID) not null,
    Osoba_ID int references OSOBE (Osoba_ID) not null
);


select a.*, b.*, c.*
from SREDSTVA a
inner join SREDSTVA_LOKACIJE b on a.Sredstvo_ID = b.Sredstvo_ID
inner join LOKACIJE c on c.Lokacija_ID = b.Lokacija_ID;




---------- INSERTI ----------

insert into OSOBE (Ime, Prezime, Email, Lozinka) values
    ('Ivor', 'Ćelić', 'ivorcelic@gmail.com', 'lozinka'),
    ('Tomislav', 'Jakopec', 'tjakopec@gmail.com', 'TomoCar123'),
    ('Pero', 'Đurić', 'peroduric@gmail.com', 'lozinka');

insert into SREDSTVA (Naziv, Opis, Komadno, Osoba_ID) values
    ('Cannon LBP6650', 'Printer crno-bijeli', 0, 1),
    ('T-shirt', 'Majice sa EPSO 2023', 1, 2),
    ('Rama za 100m', 'Drvena rama', 1, 1);

insert into LOKACIJE (Naziv, Opis) values
    ('46', 'El.mete, ekrane, televizore'),
    ('41', 'Metalurgija, rame...'),
    ('24', 'Printeri, monitori, stativi, projektori, mete, uredski materijal...'),
    ('26', 'Alati');

insert into SREDSTVA_LOKACIJE (Kolicina, Cijena, Opis, Sredstvo_ID, Lokacija_ID, Osoba_ID) values
    (1, 1000, null, 1, 3, 1),
    (200, 5, '5xM, 10xS, 25xXS', 2, 3, 1),
    (40, 5, '20 komada u neiskoristivom stanju', 3, 2, 2);




---------- SELECT ----------

select a.Kolicina, a.Cijena, a.Opis, b.Naziv as 'Naziv sredstva', c.Naziv as 'Skladište', CONCAT(d.Ime, ' ', d.Prezime) as 'Osoba'
from SREDSTVA_LOKACIJE a
inner join SREDSTVA b on a.Sredstvo_ID=b.Sredstvo_ID
inner join LOKACIJE c on a.Lokacija_ID=c.Lokacija_ID
inner join OSOBE d on a.Osoba_ID=d.Osoba_ID