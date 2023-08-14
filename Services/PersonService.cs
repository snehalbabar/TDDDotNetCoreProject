using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContract;
using ServiceContract.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {

        //[private field
        private readonly List<Person> _persons;
        private readonly ICountryService _countryService;

        public PersonService()
        {
            _persons = new List<Person>();
            _countryService = new CountriesService();
        }

        private PersonResponse ConveryPersonIntoPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country =
                _countryService.GetCounbtryByCountryId(person.CountryId)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //1.check if "personAddReqest" is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(PersonAddRequest));
            }

            //2. Validate all properties of "personAddRequest"

            //Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            //3.Convert "PersonADdREquest" to "Person"
            Person personObj =  personAddRequest.ToPerson();

            //4.Gnerate a new PersonId
            personObj.PersonId =  Guid.NewGuid();


            //5.Then Add it into Person List
            _persons.Add(personObj);


            //6.Return personResponse object withh generated PersonId
            return ConveryPersonIntoPersonResponse(personObj);
          
        }

        public List<PersonResponse> GetAllPerson()
        {
            //convert person to personResponse type
            //return list them
           return _persons.Select(x => x.ToPersonResponse()).ToList();
        
            
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            //1. Check if "personId" is not null
            if (personId == null) return null;

            //2. get Matching person form List<Person> based on personId and
            //3. Convert Matching person objcet form "person to personresponse Type
           // 4. return PersonResponse Object
            return _persons.FirstOrDefault(x => x.PersonId == personId)? .ToPersonResponse() ?? null ;



            


        }
    }
}

