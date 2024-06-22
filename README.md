# Royal API Middleware

**Requirements:**
- Visual Studio 2022
- Microsoft SQL Server Manager (version >= 19.x.x.x)
- Postman

**Setup:**
1. Restore nuget
2. Build Solution
3. ConnectionString, JwtSettings, and Serilog settings should be configured in appsettings.json:
	- ConnectionString should be edited to meet your local db params
	- In JwtSettings SecretKey needs to get a string of random characters
	- In Serilog the MinimumLevel should be changed to ones needs (Informations level recomended)


## Important Developer Notes:

### Docker
This was my first atempt to Containerize a Solution and thus far I haven't been a 100% succesfull.
On my local I can run a containrized version of this in my VS 2022.

**Requirements:**
1. Visual Studio 2022
2. Docker installed (for a more detailed guide in installing Docker [Here](https://docs.docker.com/desktop/install/windows-install/))
3. Docker Desktop.

I've not been able to get the containers up and running standalone in terminal without Visual Studio 2022.
Seeing this is a Practice project I will be tinkering with this in the future.

### Repository Layer
I build in a Repository Layer but left it unused because of the nature of this particular assignment.
Seeing this is a middleware that is making HTTP requests to a dummy API which provides me with all data I need.
I did put in in here because I might do something with it later on and it falls into the skeleton structure of the general type of architecture I tend to use.
But for now the Repository Layer and Entity Framework remain untouched in this repo
