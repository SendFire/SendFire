# SendFire
A .NET Core Based Server Deployment and Command Execution System that uses a fork of [Hangfire](http://github.com/HangfireIO/Hangfire) for its Scheduling Engine

SendFire is two components:
1. An ASP.NET Core 2.0 Administration Web Portal that allows for administration of Nodes in your network.
2. A .NET Core 2.0 Windows Service which runs locally on all servers that you wish to become nodes on your network that can be controlled by the SendFire Administration Portal.

Developer Information
=====================
SendFire Requires the following programs to be installed for local Development:
1. [SQL Server Express 2017](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express) for the Hangfire / SendFire Job and Node information Storage.
2. [.NET Core SDK](https://www.microsoft.com/net/download/windows), 2.1.2 or above recommended.
3. [Node.JS](https://nodejs.org/en/) for NPM Package Management. If running on Windows we recommend [Node Version Manager for Windows](https://github.com/coreybutler/nvm-windows/releases) to simplify local node version control.
3. [Visual Studio 2017 Community or Visual Studio Code](https://www.visualstudio.com/) is Recommended as a code editor.

To run a local developement environment of Sendfire in Visual Studio Code, open a terminal window and run the following:
1. `npm install` will install all the local node packages necessary to run.
2. `dotnet restore` will install all the local nuget packages necessary to run.
3. `dotnet watch run` will build and run the local web aministration portal.

SendFire DB Setup for Development
=================================
SQL Express using the default installation local on the local host `.\sqlexpress` is expected using the default connection string settings in the application. If you don't want to use this you will need to modify your local copy of the appsettings.json accordingly.

1. Setup a blank *SendFire* database in SQL Management Studio.
2. Create a *SendFire* user using the following settings. The users default password is `SendFire2017#!`.

