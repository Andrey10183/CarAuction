#Base image of .Net we using in our project
#Should match sdk we are using in project
#mcr - Microsoft public repository
#buils - this is build stage name
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

#Working directory for subsequent commands 
#Directory inside docker image
WORKDIR /app

#Expose port inside docker. For .Net 8 might be different port!!!!!!!!!!!!!!
EXPOSE 80 

# copy all .cproj files and restore as distinct layers. Use of the same COPY command
#for every dockerfile in the project to take advantage of docker cashing
COPY CarAuction.sln CarAuction.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj

# Restore package dependencies
RUN dotnet restore CarAuction.sln

#Copy the app folders over
COPY src/IdentityService src/IdentityService

#switch directory
WORKDIR /app/src/IdentityService
#perform publish comman -c config Release, -o Output dir
RUN dotnet publish -c Release -o /app/src/out

#Build runtime image
#We just enough to have aspnet image for this 
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# . - means we take all content of build and copy it to out dir
COPY --from=build /app/src/out .
ENTRYPOINT ["dotnet", "IdentityService.dll"]