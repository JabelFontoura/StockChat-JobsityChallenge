# StockChat - JobsityChallenge
This application should allow several users to talk in a chatroom and also to get stock quotes
from an API using a specific command.


## How to run
### Requirements
* **[.NET SDK 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)**
* **[Entity Framework Core tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)**
* **[SQL Server Express LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15)**
* **[RabbitMq](https://www.rabbitmq.com/download.html)**
* **[npm](https://nodejs.org/en/download/)**

- Option 1
  - Download the repository and execute the following steps in the root folder.
  - Execute `ExecuteApi.bat` to restore, update database and run the applications `WebApi`, `Worker.Consumer` and `Worker.Producer`.
  - Execute `ExecuteApp.bat` to install npm depencies and start client `StockChat.App` application.
  - The client application should be available at **[http://localhost:3000](http://localhost:3000)**.
- Option 2
  - Download the repository and execute the following steps in the root folder.
  - Open `StockChat.sln` in Visual Studio and build solution.
  - Run `dotnet ef database update --project StockChat.Infrastructure\StockChat.Infrastructure.csproj --context StocksChatDbContext --startup-project StockChat.WebApi\StockChat.WebApi.csproj` command in `StockChat.Api` folder to update database.
  - Select multiple startup projects `WebApi`, `Worker.Consumer` and `Worker.Producer` in Visual Studio and run.
  - In `StockChat.App` folder run `npm install` and `npm start`
  - The client application should be available at **[http://localhost:3000](http://localhost:3000)**.
  
## Mandatory Features
- [x] Allow registered users to log in and talk with other users in a chatroom.
- [x] Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code
- [x] Create a decoupled bot that will call an API using the stock_code as a parameter
(https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the
stock_code)
- [x] The bot should parse the received CSV file and then it should send a message back into
the chatroom using a message broker like RabbitMQ. The message will be a stock quote
using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
the bot.
- [x] Have the chat messages ordered by their timestamps and show only the last 50
messages.
● Unit test the functionality you prefer.

## Bonus (Optional)
- [x] Use .NET identity for users authentication
- [x] Handle messages that are not understood or any exceptions raised within the bot.
- [ ] Build an installer
