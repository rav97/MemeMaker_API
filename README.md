# MemeMaker_API

An ASP.NET Core Web API created for [WPF Meme Maker](https://github.com/rav97/WPF_MemeMaker/tree/MemeMakerOnline) to manage memes and templates from remote apps.

## Features

- IsAlive checking
- ApiKey authorisation
- [Synchronous](https://github.com/rav97/MemeMaker_API/tree/main) or [Asynchronous](https://github.com/rav97/MemeMaker_API/tree/acync-transform) execution
- Manage Templates
- Manage Memes
- Swagger documentation

## Tech and tools

- Visual Studio 2022
- .NET 6
- ASP.NET Core
- MS SQL Server 
- Entity Framework Core
- Swashbuckle Swagger
- DDD Approach (Domain Driven Design)

## How to run
1. Clone repository
2. Prepare database
    - Use `Setup\Database\MemeMakerDB.bak` file to restore database
3. Prepare `Memes` and `Templates` folders
     - Copy `Memes` and `Templates` folder from `\Setup` directory and place them on your `C:\\` drive
     - You can place them in other location but in that case you need to edit `appsettings.json` file and replace `"DiskPaths"` values on your own.
4. Make sure you have installed all required packages and dependencies.
5. Edit `appsettings.json` and replace `"MemeMakerConnection"` to connection string of your database.
6. Build and run project. You can use API through Swagger UI or you can run [WPF Meme Maker](https://github.com/rav97/WPF_MemeMaker/tree/MemeMakerOnline) project which connect with API.

## How it looks like

![Preview1](https://github.com/rav97/ResourcesRepository/blob/main/MemeMakerAPI/GeneralApiPreview1.png?raw=true)
![Preview2](https://github.com/rav97/ResourcesRepository/blob/main/MemeMakerAPI/GeneralApiPreview2.png?raw=true)

## TODO
- Logging
- UnitTesting
- IntegrationTesting

## Future plans

Although project is pretty much ready and working it still needs some more work to do. For sure there are some refactoring needed and minor bugfixes. The functionality of web API and implemented functions can be improved or extended, but these works are time-consuming and the effect of their implementation will be insignificant so I decided to leave it as is. The main purpose of this project was to refresh my ASP.NET Core and C# skills and expand my portfolio and I think it's done pretty well.
