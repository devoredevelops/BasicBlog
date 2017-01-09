# Basic Blog

This is a blog engine written in ASP.NET Core MVC. 

## Instructions to run

1. Install .NET Core: https://www.microsoft.com/net/core.
2. Download the repo and unzip into folder "BasicBlog".
3. Open Command Prompt/PowerShell from the directory you unzipped the project to.
4. Run command "dotnet restore".
5. Run command "dotnet run".
6. Open your web browser and navigate to "http://localhost:5000".

Here's one feature I'd like to highlight: if a user tries to edit or delete a post created by another user, the app will return a 401 "Unauthorized" response and the user will be redirected to the login page.
