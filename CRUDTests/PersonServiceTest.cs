using System;
using Entities;
using ServiceContract;
using ServiceContract.DTO;
using ServiceContract.Enums;
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

        private List<PersonResponse> AddPersonsIntoList()
        {
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

            //create a list and call AddPerson in loop for all persons
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
            { personAddRequest1, personAddRequest2};


            List<PersonResponse> people = new List<PersonResponse>();
            foreach (PersonAddRequest item in personAddRequests)
            {
                PersonResponse person = _personService.AddPerson(item);
                people.Add(person);
            }

            return people;
        }

        private PersonResponse AddPersonHelper()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            { CountryName = "USA" };
            
            CountryResponse country1 = _countryService.AddCountry(countryAddRequest1);
            
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

            PersonResponse person1 = _personService.AddPerson(personAddRequest1);

            return person1;
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

        #region getFilteredPersons
        //if the search text is empty and seach by is "PersonName", it should return all  persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
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
            foreach (PersonResponse personResponse in people)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            //Act
            List<PersonResponse> personResponses = _personService.GetFilteredPerson(nameof(Person.PersonName),"");

            //Print personResponses list
            _testOutputHelper.WriteLine("Actual Output:");
            foreach (PersonResponse person in personResponses)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            foreach (PersonResponse response in people)
            {
                Assert.Contains(response, personResponses);
            }
        }

        // Frist we will add few persons; and then we will search based on person name with some search string.
        //it should return the matching persons
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            List<PersonResponse> people = AddPersonsIntoList();

            //Print People list
            _testOutputHelper.WriteLine("Expected Output:");
            foreach (PersonResponse personResponse in people)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            //Act
            List<PersonResponse> personResponses =
                _personService.GetFilteredPerson(nameof(Person.PersonName), "sn");

            //Print personResponses list
            _testOutputHelper.WriteLine("Actual Output:");
            foreach (PersonResponse person in personResponses)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            foreach (PersonResponse response in people)
            {
                if (response.PersonName != null)
                {
                    if (response.PersonName.Contains("sn", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(response, personResponses);
                    }
                }
                
            }
        }
        #endregion


        #region GetSortedPersons
        //When we sort based on Personname in Desc, it should return persons list in descending order itself.
        [Fact]
        public void GetSortedPersons()
        {
            //Arrange

            List<PersonResponse> people = AddPersonsIntoList();
            
            //Print People list
            _testOutputHelper.WriteLine("Expected Output:");
            foreach (PersonResponse personResponse in people)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> AllPersons = _personService.GetAllPerson();

            //Act
            List<PersonResponse> personResponses_from_sort =
                _personService.GetSortedPersons(AllPersons,nameof(Person.PersonName), SortOrderOptions.DESC);

            //Print personResponses list
            _testOutputHelper.WriteLine("Actual Output:");
            foreach (PersonResponse person in personResponses_from_sort)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            people = people.OrderByDescending(x => x.PersonName).ToList();

            //Assert
            for(int i =0; i< people.Count; i++)
            {
                Assert.Equal(people[i], personResponses_from_sort[i]);
            }
        }
        #endregion

        #region UpdatePerson
        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            
            //Assert
            Assert.Throws<ArgumentNullException>(() => {
                //Act
                _personService.UpdatePerson(personUpdateRequest);

            });
        }


        //When we supply invalid  Person id , it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() {

                PersonId = Guid.NewGuid()
            };


            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _personService.UpdatePerson(personUpdateRequest);

            });
        }

        //When we supply Person name null , it should throw ArgumentException
        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            PersonResponse person = AddPersonHelper();
            PersonUpdateRequest? personUpdateRequest = person.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = null; 

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _personService.UpdatePerson(personUpdateRequest);

            });
        }

        //When we supply  valid person detail , try to update the detail name and detail
        [Fact]
        public void UpdatePerson_PersonVaildDetails ()
        {
            //Arrange

            PersonResponse person = AddPersonHelper();
            PersonUpdateRequest? personUpdateRequest = person.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "Sanajana";
            personUpdateRequest.Email = "snaju@gmail.com";


            //Act
            PersonResponse response_from_Update = _personService.UpdatePerson(personUpdateRequest);

            PersonResponse? response_from_getperosnbyPersonId = _personService.GetPersonByPersonId(personUpdateRequest.PersonId);

            //Assert
            Assert.Equal(response_from_getperosnbyPersonId, response_from_Update);
        }
        #endregion

        #region DeletePerson

        //If you supply an valid personId ,it should return false
        [Fact]
        public void DeletePerson_ValidPersonId()
        {

            //Arrange
            PersonResponse person = AddPersonHelper();
            //Act
           bool isDeleted = _personService.DeletePerson(person.PersonId);
            //Assert
            Assert.True(isDeleted);

        }

        //If you supply an invalid personId ,it should return false
        [Fact]
        public void DeletePerson_InvalidPersonId()
        {

            //Arrange
            PersonResponse person = AddPersonHelper();
  
            //Act
            bool isDeleted = _personService.DeletePerson(Guid.NewGuid());
            //Assert
            Assert.False(isDeleted);

        }
        #endregion


    }
}

