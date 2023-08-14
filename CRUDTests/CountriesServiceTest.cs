using System;
using ServiceContract;
using ServiceContract.DTO;
using Services;

namespace CRUDTests
{
	public class CountriesServiceTest
	{
		private readonly ICountryService _countryServices;

		public CountriesServiceTest()
		{
			_countryServices = new CountriesService();

		}

        #region AddCountry

        //1. When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
		public void AddCountry_NullCountry()
		{
			//Arrange
			CountryAddRequest? countryAddRequest = null;

			//Assert
			Assert.Throws<ArgumentNullException>(() =>
			{
                //Act
                _countryServices.AddCountry(countryAddRequest);

            });

		}

		//2. When CountryName is null, it should throw ArgumentException
		[Fact]
        public void AddCountry_CountryNameIsNull()
        {
			//Arrange
			CountryAddRequest? countryAddRequest = new CountryAddRequest() {
				CountryName = null
			};

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryServices.AddCountry(countryAddRequest);

            });

        }

        //3. When CoiuntryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryAddRequest? countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryServices.AddCountry(countryAddRequest1);
                _countryServices.AddCountry(countryAddRequest2);

            });

        }

        //4. When you supply proper country name, it should insert(add) the contry to the existing list of countries.
        [Fact]
        public void AddCountry_ProperCountryDeatils()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            CountryResponse response =
              _countryServices.AddCountry(countryAddRequest);

            List<CountryResponse> countires_from_GetAllCountries = _countryServices.GetAllCountries();
            //Assert
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, countires_from_GetAllCountries);
            

        }

        #endregion


        //the list of countires should be empty by default.(before adding any country)
        [Fact]
        public void GetAllCountires_EmptyList()
        {
            //Act
            List<CountryResponse> ActualResult_countries = _countryServices.GetAllCountries();

            //Assert
            Assert.Empty(ActualResult_countries);
        }

        [Fact]
        public void GetAllCountires_AddFewCountires()
        {
            //Arrange
            List<CountryAddRequest> countryAdds = new List<CountryAddRequest>()
            {
                new CountryAddRequest(){ CountryName= "USA"},
                 new CountryAddRequest(){ CountryName= "India"},
            };

            //Act
            List<CountryResponse> countries_list_add_country = new List<CountryResponse>();
            foreach (CountryAddRequest request in countryAdds)
            {
                countries_list_add_country.Add
                    (_countryServices.AddCountry(request));
            }
            List<CountryResponse> actualCountryList = _countryServices.GetAllCountries();

            //Assert
            //read each element form countries_list_add_country
            foreach (CountryResponse expected_country in countries_list_add_country)
            {
                Assert.Contains(expected_country, actualCountryList);
            }
           
        }

        #region GetcountryByCountryId
        [Fact]
        //if we supply null it should return return null
        public void GetCountryByCountryId_NUllCountryId()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse country_response_form_get_method = _countryServices.GetCounbtryByCountryId(countryId);


            //Assert
            Assert.Null(country_response_form_get_method);
        }

        [Fact]
        //if we supply 
        public void GetCountryByCountryId_ValidCountryId()
        {
            //Arrange
                //by default we have empty list so add a new country 
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Japan"
            };
                //add this country to our list
            CountryResponse response =
              _countryServices.AddCountry(countryAddRequest);

            

            //Act
            CountryResponse? country_response_form_get_method = _countryServices.GetCounbtryByCountryId(response.CountryId);


            //Assert
            Assert.Equal(response, country_response_form_get_method);
        }



        #endregion



    }
}

