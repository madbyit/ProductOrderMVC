# Product order
This started as a Home assignment when applying for a job.
But I will keep improving it.

Prerequisites: 
- .net core 3.1 - https://dotnet.microsoft.com/download/dotnet-core/3.1
- Visual Studio/VS Code	

In this folder you will find one solution and two projects:

The project is a .net core 3.1 MVC web application.
Your task is to read the pipe-separated csv files with order data found in/App_Data/-folder
and display the orders in the web-ui. When parsing the files make sure to use proper models that represents the data.

It is up to you how to store the data, be it a database, in application memory or elsewhere.

Keep abstraction in mind, build the solution to handle any number of data sources, e.g. the csv might in the future be turned into a xml or a database. 

Proposal of steps to complete: 

- Analyze the CSV files and create a model that can represent the order information contained in the files
- Complete the implementation of OrdersController.cs
- Build tests to validate the functionality of the solution. 


Bonus: Alternative solutions or other means that express your talents as a developer are always welcome. For example create a ui to support new features like editing or creating new orders.
