using EndangeredNearYou.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EndangeredNearYou.Test
{
    public class LocationRepositoryTests : IClassFixture<TestFixture>
    {
        private readonly ILocationRepository _locationRepository;

        //Arrange
        public LocationRepositoryTests(TestFixture fixture)
        {
            _locationRepository = fixture.ServiceProvider.GetRequiredService<ILocationRepository>();
        }

        [Theory]
        [InlineData(4)]
        public void GetAllContinents_Test(int expected)
        {
            //Act
            var actual = _locationRepository.GetAllContinents();

            //Assert
            Assert.Equal(expected, actual.Count());
        }

        [Theory]
        [InlineData("NA", "United States of America", "US", 840)]
        [InlineData("OC", "American Samoa", "AS", 16)]
        public void GetCountriesByContinent_Test(string continentCode, string expectedCountryName, string expectedCountryCode, int expectedCountryNumber)
        {
            //Act
            var actual = _locationRepository.GetCountriesByContinent(continentCode);
            var actualCountry = actual.Where(c => c.Country_Name == expectedCountryName).FirstOrDefault();

            //Assert
            Assert.Equal(continentCode, actualCountry.Continent_Code);
            Assert.Equal(expectedCountryName, actualCountry.Country_Name);
            Assert.Equal(expectedCountryCode, actualCountry.Two_Letter_Country_Code);
            Assert.Equal(expectedCountryNumber, actualCountry.Country_Number);
        }

        [Theory]
        [InlineData("US", "Orange Beach", 30.294370, -87.573590)]
        [InlineData("AQ", "McMurdo Station", -77.845500, 166.669800)]
        public void GetLocationsByCountry_Test(string countryCode, string expectedLocationName, double expectedLatitude, double expectedLongitude)
        {
            //Act
            var actual = _locationRepository.GetLocationsByCountry(countryCode);
            var actualLocation = actual.Where(c => c.Name == expectedLocationName).FirstOrDefault();

            //Assert
            Assert.Equal(countryCode, actualLocation.Country_Code);
            Assert.Equal(expectedLocationName, actualLocation.Name);
            Assert.Equal(expectedLatitude, actualLocation.Latitude);
            Assert.Equal(expectedLongitude, actualLocation.Longitude);
        }

        [Theory]
        [InlineData(3, "Century City", 34.055570, -118.417860)]
        [InlineData(5, "El Paso", 31.758720, -106.486930)]
        public void GetLocationById_Test(int locationId, string expectedLocationName, double expectedLatitude, double expectedLongitude)
        {
            //Act
            var actual = _locationRepository.GetLocationById(locationId);

            //Assert
            Assert.Equal(locationId, actual.City_Id);
            Assert.Equal(expectedLocationName, actual.Name);
            Assert.Equal(expectedLatitude, actual.Latitude);
            Assert.Equal(expectedLongitude, actual.Longitude);
        }


        [Fact]
        public void GetRandomLocation_Test()
        {
            //Act
            var actual = _locationRepository.GetRandomLocation();

            //Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Name);
            Assert.NotNull(actual.Latitude);
            Assert.NotNull(actual.Longitude);
        }
    }
}
