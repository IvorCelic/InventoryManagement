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
    Sredstvo_ID int references SREDSTVA (Sredstvo_ID) not null,
    Lokacija_ID int references LOKACIJE (Lokacija_ID) not null,
    Osoba_ID int references OSOBE (Osoba_ID) not null
);