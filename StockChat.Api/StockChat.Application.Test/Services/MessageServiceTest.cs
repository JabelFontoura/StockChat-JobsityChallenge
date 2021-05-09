using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockChat.Application.Mapper;
using StockChat.Application.Services;
using StockChat.Domain.Models;
using StockChat.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StockChat.Application.Test.Services
{
    public class MessageServiceTest
    {
        private readonly DbContextOptions<StocksChatDbContext> _options;
        private readonly IMapper _mapper;

        public MessageServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MessageProfile());
            });

            _mapper = config.CreateMapper();

            _options = new DbContextOptionsBuilder<StocksChatDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthGroupAppServiceTest").Options;
        }

        [Fact]
        public async Task SaveMessageShouldSaveToDatabase()
        {
            using (var inMemoryContext = new StocksChatDbContext(_options))
            {
                inMemoryContext.Database.EnsureDeleted();

                var service = new MessageService(inMemoryContext, _mapper);
                var result = await service.SaveMessage(new IdentityUser() { UserName = "Test" }, "Test message");

                Assert.Equal(1, await inMemoryContext.Messages.CountAsync());
                Assert.NotNull(result);
                Assert.Equal("Test message", result.Text);
                Assert.Equal("Test", result.User.UserName);
            }
        }

        [Fact]
        public async Task SaveMessageWithSlashStockShouldntSaveToDatabase()
        {
            using (var inMemoryContext = new StocksChatDbContext(_options))
            {
                inMemoryContext.Database.EnsureDeleted();

                var service = new MessageService(inMemoryContext, _mapper);
                var result = await service.SaveMessage(new IdentityUser() { UserName = "Test" }, "/stock=aapl.us");

                Assert.Equal(0, await inMemoryContext.Messages.CountAsync());
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetLast50MessagesWithLessThan50MessagesShouldReturnAllMessages()
        {
            using (var inMemoryContext = new StocksChatDbContext(_options))
            {
                inMemoryContext.Database.EnsureDeleted();

                var service = new MessageService(inMemoryContext, _mapper);

                for (int i = 0; i < 10; i++)
                    inMemoryContext.Messages.Add(new Message("Message", new IdentityUser() { UserName = "Test" }));

                inMemoryContext.SaveChanges();

                var result = await service.GetLast50Messages();

                Assert.NotNull(result);
                Assert.Equal(10, result.ToList().Count);
            }
        }

        [Fact]
        public async Task GetLast50MessagesWithMoreThan50MessagesShouldReturn50Messages()
        {
            using (var inMemoryContext = new StocksChatDbContext(_options))
            {
                inMemoryContext.Database.EnsureDeleted();

                var service = new MessageService(inMemoryContext, _mapper);

                for (int i = 0; i < 65; i++)
                    inMemoryContext.Messages.Add(new Message("Message", new IdentityUser() { UserName = "Test" }));

                inMemoryContext.SaveChanges();

                var result = await service.GetLast50Messages();

                Assert.NotNull(result);
                Assert.Equal(50, result.ToList().Count);
            }
        }
    }
}
