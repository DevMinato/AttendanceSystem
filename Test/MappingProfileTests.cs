using AttendanceSystem.Application.Profiles;
using AutoMapper;
using Xunit;

namespace Test
{
    public class MappingProfileTests
    {
        [Fact]
        public void AssertConfigurationIsValid()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddProfiles(new List<Profile>
            {
                new MappingProfile()
            }));

            mapperConfig.AssertConfigurationIsValid();
        }
    }
}