using Application.Mapper;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.Application.Mapper
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configuration;

        public MappingProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_Should_HaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
    }
}
