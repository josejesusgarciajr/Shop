
/*
	create database for project
*/

CREATE DATABASE Arizona;

/*
	create table for Company
*/

CREATE TABLE Company(
	ID int NOT NULL IDENTITY PRIMARY KEY,
	Name varchar(70),
	Address varchar(100),
	HrefAddress varchar(150),
	MissionStatement varchar(255)
);

/*
	insert company onto Company Table
*/

INSERT INTO Company(Name, Address, HrefAddress, MissionStatement)
VALUES ('Vanessa', 'Disney World, Orlando Florida',
	'https://www.google.com/maps/dir//Walt+Disney+World®+Resort,+Orlando,+FL',
	'To make the world a better place!');

/*
	create table for Products
*/

CREATE TABLE Product(
	ID int NOT NULL IDENTITY PRIMARY KEY,
	CompanyID int NOT NULL FOREIGN KEY REFERENCES Company(ID),
	Name varchar(50),
	Price float,
	Description varchar(255),
	DiscountBool bit,
	DiscountPercentage float,
	Flag bit
);

/*
	insert a product onto a Product Table
*/

INSERT INTO Product(CompanyID, Name, Price, Description,
 DiscountBool, DiscountPercentage, Flag)
 VALUES (2, 'Dance', 15.99, 'Express yourself! Feel the beat! Feel the music! Feel Alive!',
 	0, 0.00, 0);

/*
	create table for images
*/

CREATE TABLE Image(
	ID int NOT NULL IDENTITY PRIMARY KEY,
	CompanyID int NOT NULL FOREIGN KEY REFERENCES Company(ID),
	ProductID int NOT NULL FOREIGN KEY REFERENCES Product(ID),
	Thumbnail bit,
	ImagePath varchar(100),
);

/*
	insert image onto Image Table
*/

INSERT INTO Image(CompanyID, ProductID, Thumbnail, ImagePath)
VALUES ();


