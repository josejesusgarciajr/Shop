
/*
	create database for project
*/

CREATE DATABASE Arizona;

/*
	create table for companies
*/

CREATE TABLE Company(
	ID int NOT NULL IDENTITY PRIMARY KEY,
	Name varchar(70),
	Address varchar(100),
	HrefAddress varchar(150),
	MissionStatement varchar(255)
);

/*
	create table for products
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