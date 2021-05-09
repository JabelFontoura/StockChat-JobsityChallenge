using AutoMapper;

namespace StockChat.Application.Mapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(options =>
            {
                options.AddProfile(new UserProfile());
                options.AddProfile(new MessageProfile());
            });
        }
    }
}
