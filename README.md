# Galaxy Backend
![.NET Core](https://github.com/bugs-x64/galaxy_backend/workflows/.NET%20Core/badge.svg?branch=master)

* API Description - https://localhost:5001/swagger
* Tech stack - .NET Core 3.1, Github Actions, Docker, Azure App Service, EntityFramework Core, XUnit, MSSQL
* Some API methods is authorized access only. Call [POST] /user or [POST] /auth/token methods and add bearer token authorize header.
* Before launch project, update database from GalaxyRepository. Run the following command in Package Manager Console **Add-Migration Initial** and **Update-Database**.
