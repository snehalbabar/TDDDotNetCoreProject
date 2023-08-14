using System;
using ServiceContract;
using ServiceContract.DTO;
using Services;
using Xunit.Abstractions;

namespace CRUDTests
{
	public class PersonServiceTest 
	{
		//private fields

		private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;

		//constructor
		public PersonServiceTest(ITestOutputHelper testOutputHelper)
		{
			this._personService = new PersonService();
            _countryService = new CountriesService();
            _testOutputHelper = testOutputHelper;
		}

		#region AddPerson

		//when you supply null valus as PersonAddRequest, It should throw ArgumentNullException
		[Fact]
		public void AddPerson_NullPerson()
		{
			//Arrange
			PersonAddRequest request = null;

			//Act
			
			Assert.Throws<ArgumentNullException>(() => {
                _personService.AddPerson(request);
            });
		}


        //when you supply null valus as personName, It should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest request = new PersonAddRequest() {
				PersonName = null
			};

            //Act

            Assert.Throws<ArgumentException>(() => {
                _personService.AddPerson(request);
            });
        }

        //when you supply proper values as PersonAddRequest, It should insert person into the person list
        //and it should return an object of peronresponse , which includes with the newly geneated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "abc",
                Email ="abc@gmail.com",
                Address ="11700 lebanon rd texas",
                CountryId = new Guid(),
                Gender = ServiceContract.Enums.GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2002-12-02"),
                ReceiveNewsLetters = true

                
            };

            //Act
            PersonResponse responseObj = _personService.AddPerson(request);

           List<PersonResponse> allPeopleList =  _personService.GetAllPerson();

            //Assert
            //1.person should get newly created Guid for this entry
            Assert.True(responseObj.PersonId != Guid.Empty);
            //2. person should present in person list
            Assert.Contains(responseObj, allPeopleList);
     
        }

        #endregion


        #region getPersonBypersonId
        //if we supply null as persondId ,it should return nullas personreponse
        [Fact]
        public void GetPersonByPersonId_NullPersonID()
        {
            //arrange
            Guid? personId = null;

            //act
           PersonResponse? personResponseResult = _personService.GetPersonByPersonId(personId);

            //Assert
            Assert.Null(personResponseResult);

        }

        //if we supply valid  persondId ,it should return valid details as personreponse
        [Fact]
        public void GetPersonByPersonId_WithPersonID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "India"
            };
            CountryResponse countryResponse =  _countryService.AddCountry(countryAddRequest);


            //act
            //but we have to create country id first for accssing thhe country name
            PersonAddRequest addRequest = new PersonAddRequest()
            {
                PersonName = "Snehal",
                Email = "snehalBabar@gmail.com",
                Address ="adress",
                CountryId = countryResponse.CountryId,
                DateOfBirth = DateTime.Parse("1997-05-22"),
                Gender = ServiceContract.Enums.GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonResponse personResponse = _personService.AddPerson(addRequest);

            PersonResponse? personResponseWithId =  _personService.GetPersonByPersonId(personResponse.PersonId);


            //Assert
            Assert.Equal(personResponse, personResponseWithId);
           

        }


        #endregion

        #region GetAllPersons
        //the getAllperson() should return an empty list by default
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> personResponses = _personService.GetAllPerson();

            //Assert
            Assert.Empty(personResponses);
        }

        //first we will add person data and the getAllperson() should return an person list by default
        [Fact]
        public void GetAllPersons_PersonList()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            { CountryName = "India" };
            CountryResponse country1 = _countryService.AddCountry(countryAddRequest1);
            CountryResponse country2 = _countryService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Snehal",
                Email = "snehalBabar@gmail.com",
                Address = "adress",
                CountryId = country1.CountryId,
                DateOfBirth = DateTime.Parse("1997-05-22"),
                Gender = ServiceContract.Enums.GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "hafsah",
                Email = "hafsah@gmail.com",
                Address = "adress",
                CountryId = country2.CountryId,
                DateOfBirth = DateTime.Parse("1995-05-22"),
                Gender = ServiceContract.Enums.GenderOptions.Female,
                ReceiveNewsLetters = false
            };
            //PersonResponse person1 = _personService.AddPerson(personAddRequest1);
            //PersonResponse person2 = _personService.AddPerson(personAddRequest2); //OR

            //create a list and call AddPerson in loop for all persons
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
            { personAddRequest1, personAddRequest2};


            List<PersonResponse> people = new List<PersonResponse>();
            foreach (PersonAddRequest item in personAddRequests)
            {
                PersonResponse person = _personService.AddPerson(item);
                people.Add(person);
            }

            //Print People list
            _testOutputHelper.WriteLine("Expected Output:");
            foreach(PersonResponse personResponse in people)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            //Act
            List<PersonResponse> personResponses = _personService.GetAllPerson();

            //Print personResponses list
            _testOutputHelper.WriteLine("Actual Output:");
            foreach (PersonResponse person in personResponses)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            foreach (PersonResponse response in people)
            {
                Assert.Contains(response,personResponses);
            }
        }


        #endregion


    }
}

