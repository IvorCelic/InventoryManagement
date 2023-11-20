use master;
drop database if exists Inventura;
go
create database Inventura;
use Inventura;
go


create table OPERATER (
    Operater_ID int primary key identity (1, 1) not null,
    Ime varchar(20) not null,
    Email varchar(50) not null,
    Lozinka varchar(50) not null
);

create table OSOBA (
    Osoba_ID int primary key identity (1, 1) not null,
    Ime varchar(20) not null,
    Prezime varchar(20) not null,
    Email varchar(50) not null,
    Lozinka varchar(50) not null
);

create table SREDSTVO (
    Sredstvo_ID int primary key identity (1, 1) not null,
    Naziv varchar(50) not null,
    Opis varchar(100) null,
    Komadno bit not null,
    Osoba_ID int references OSOBA (Osoba_ID) not null
);

create table LOKACIJA (
    Lokacija_ID int primary key identity (1, 1) not null,
    Naziv varchar(50) not null,
    Opis varchar(100) null
);

create table SREDSTVO_LOKACIJA (
    Sredstvo_lokacija_ID int primary key identity (1, 1) not null,
    Kolicina int not null,
    Cijena decimal not null,
    Sredstvo_ID int references SREDSTVO (Sredstvo_ID) not null,
    Lokacija_ID int references LOKACIJA (Lokacija_ID) not null,
    Osoba_ID int references OSOBA (Osoba_ID) not null
);