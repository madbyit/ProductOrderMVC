# Product order
[Work In Progress]

## General
In this folder you will find one solution and two projects:
I run this on VS Code in Linux.
ASP.NET Core Runtime 6.0.2

## The task
The task is to read the pipe-separated csv files with order data found in/App_Data/-folder and display the orders in the web-ui. 

- Analyze the CSV files and create a model that can represent the order information contained in the files
- Complete the implementation of OrdersController.cs
- Build tests to validate the functionality of the solution. 

## ToDo
Create a ui to support new features like editing or creating new orders.


## NOTES:
The project was at start a .net core 3.1 MVC web application, I have modified it to .net 6.0 core instead.

Installed nuget package for postgresl
$ dotnet add package Npgsql --version 6.0.3