@echo off
cd StockChat.Api
dotnet restore
dotnet ef database update --project StockChat.Infrastructure\StockChat.Infrastructure.csproj --context StocksChatDbContext --startup-project StockChat.WebApi\StockChat.WebApi.csproj
cd ..
cd StockChat.Api\StockChat.WebApi\
start dotnet run StockChat.WebApi.csproj
timeout /t 1
cd ..
cd..
cd StockChat.Api\StockChatBot.Worker\
start dotnet run StockChatBot.Worker.Consumer.csproj
timeout /t 1
cd ..
cd ..
cd StockChat.Api\StockChatBot.Worker.Producer\
start dotnet run StockChatBot.Worker.Producer.csproj