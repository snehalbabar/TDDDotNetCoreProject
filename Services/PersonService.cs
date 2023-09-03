using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContract;
using ServiceContract.DTO;
using ServiceContract.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {

        //[private field
        private readonly PersonsDbContext _db;
        private readonly ICountryService _countryService;

        public PersonService(PersonsDbContext personsDbContext, ICountryService countryService)
        {
            _db = personsDbContext;
            _countryService = countryService;
        }

        private  PersonResponse ConvertPersonIntoPersonResponse(Person person)
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
            _db.Persons.Add(personObj);
            _db.SaveChanges();


            //6.Return personResponse object withh generated PersonId
            return ConvertPersonIntoPersonResponse(personObj);
          
        }

        public List<PersonResponse> GetAllPerson()
        {
            //convert person to personResponse type
            //return list them
            
           return _db.Persons.ToList()
                .Select(x => ConvertPersonIntoPersonResponse(x)).ToList();
        
            
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            //1. Check if "personId" is not null
            if (personId == null) return null;

            //2. get Matching person form List<Person> based on personId and
            //3. Convert Matching person objcet form "person to personresponse Type
           // 4. return PersonResponse Object
            return _db.Persons.
                FirstOrDefault(x =>
                x.PersonId == personId)? .ToPersonResponse() ?? null ;

        }

        public List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString)
        {
            //get all people form GetAllperson Method
            List<PersonResponse> allPersons = GetAllPerson();
            List<PersonResponse> matchingPersons = allPersons;


            //1.Check if "SerachBy" is not null.
            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }
            //2.Get matching persons from List<Person> based on given serachBy and searchString
            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.PersonName)?
                    x.PersonName.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase):true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.Email) ?
                    x.Email.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(x =>
                    (x.DateOfBirth != null) ?
                    x.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.Gender) ?
                    x.Gender.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.CountryId):
                    matchingPersons = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.Country) ?
                    x.Country.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.Address) ?
                    x.Address.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                default:
                    matchingPersons = allPersons;
                    break;
            }
            //3.Convert the matching persoons from"Person" type to personRsponse Type
            //4.Return all mathcing PersonResponse Object.
            return matchingPersons;


        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> people,
            string sortBy, SortOrderOptions sortOrder)
        {
            if(string.IsNullOrEmpty(sortBy))
            {
                return people;
            }
            List<PersonResponse> sortedPerson = (sortBy, sortOrder) switch {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                => people.OrderBy(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                => people.OrderByDescending(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                => people.OrderBy(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                => people.OrderByDescending(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                => people.OrderBy(x => x.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                => people.OrderByDescending(x => x.DateOfBirth).ToList(),


                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                => people.OrderBy(x => x.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                => people.OrderByDescending(x => x.Age).ToList(),


                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                => people.OrderBy(x => x.Gender).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                => people.OrderByDescending(x => x.Gender).ToList(),


                (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                => people.OrderBy(x => x.Country).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                => people.OrderByDescending(x => x.Country).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                => people.OrderBy(x => x.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                => people.OrderByDescending(x => x.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                => people.OrderBy(x => x.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                => people.OrderByDescending(x => x.ReceiveNewsLetters).ToList(),

                _ => people

            } ;

            return sortedPerson;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? request)
        {
            //1. Check if "personUdateRequest" is null
            if(request == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //2. Valdtae all propoerties of "personUpdateRequest"
            ValidationHelper.ModelValidation(request);

            //3. Get the Matching "person" object from List<Person> based on personId.
           Person? matchingPerson = _db.Persons.FirstOrDefault(x => x.PersonId == request.PersonId);

            //4. check if matching "person" objcet is not null
            if (matchingPerson == null)
            {

                throw new ArgumentNullException("Given Person id doesn't exisit");
            }

            //5. Update all detials from "PersonUpdateRequest" object to "person" object
            matchingPerson.PersonName = request.PersonName;
            matchingPerson.Email      = request.Email;
            matchingPerson.DateOfBirth = request.DateOfBirth;
            matchingPerson.Gender = request.Gender.ToString();

            _db.SaveChanges();
            //6. Convert the person object from "person" to "personResponse" type
            //7. return PersonResponse object with update details
            return ConvertPersonIntoPersonResponse(matchingPerson);
        }

        public bool DeletePerson(Guid? peronId)
        {
            //1. chcek if "PersonId" is not null
            if (peronId == null)
            {
                throw new ArgumentNullException(nameof(peronId));
            }
            //Get the matching "person" from list
            Person? person = _db.Persons.FirstOrDefault(x => x.PersonId == peronId);
            if (person == null) return false;

            _db.Persons.Remove(_db.Persons.First(
                x => x.PersonId == peronId));
            _db.SaveChanges();
            return true;
        }
    }
}

