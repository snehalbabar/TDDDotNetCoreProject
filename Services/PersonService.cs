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
        private readonly List<Person> _persons;
        private readonly ICountryService _countryService;

        public PersonService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countryService = new CountriesService();
            if (initialize)
            {
                _persons.AddRange(new List<Person>() {  
                new Person() {
                    PersonId = Guid.Parse("0374eeb6-243e-4a9e-9fae-ba91efcc407e"),
                    PersonName = "Snehal",
                    Email = "snehalBabar@gmail.com",
                    Address = "6 Del Sol Pass",
                    CountryId = Guid.Parse("e098d035-94d4-4119-88a4-3f5ff6e54680"),
                    DateOfBirth = DateTime.Parse("1997-05-22"),
                    Gender = "Female",
                    ReceiveNewsLetters = false
                },

                new Person()
                {
                    PersonId = Guid.Parse("a0ecb92a-21f2-4345-ba08-3d3dc988a4fd"),
                    PersonName = "Shellysheldon",
                    Email = "sgranville1@angelfire.com",
                    Address = "39434 Birchwood Court",
                    CountryId = Guid.Parse("e098d035-94d4-4119-88a4-3f5ff6e54680"),
                    DateOfBirth = DateTime.Parse("1996-11-14"),
                    Gender = "Male",
                    ReceiveNewsLetters = true
                },
                 new Person()
                 {
                     PersonId = Guid.Parse("6a622865-fd8e-4b71-8b40-4ec0c9b49d89"),
                     PersonName = "Zyan",
                     Email = "sbrik2@dagondesign.com",
                     Address = "00 Hagan Park",
                     CountryId = Guid.Parse("68d84e65-555f-48a2-9840-436b5c695b14"),
                     DateOfBirth = DateTime.Parse("2013-02-22"),
                     Gender = "Male",
                     ReceiveNewsLetters = false
                 },
                 new Person()
                 {
                     PersonId = Guid.Parse("bb3d0cd4-51ed-400b-992f-c0f8d7077b87"),
                     PersonName = "Maynard",
                     Email = "mnelson3@about.com",
                     Address = "43636 Kensington Circle",
                     CountryId = Guid.Parse("68d84e65-555f-48a2-9840-436b5c695b14"),
                     DateOfBirth = DateTime.Parse("2020-07-12"),
                     Gender = "Male",
                     ReceiveNewsLetters = true
                 },
                 new Person()
                 {
                     PersonId = Guid.Parse("74e42c90-bcd1-4f5e-95ca-9c8419a360ec"),
                     PersonName = "Gay",
                     Email = "gbiggadike4@hostgator.com",
                     Address = "0 Corry Drive",
                     CountryId = Guid.Parse("ffd39392-fe23-4ec0-955e-130f5f3ad192"),
                     DateOfBirth = DateTime.Parse("1997-04-13"),
                     Gender = "Male",
                     ReceiveNewsLetters = false
                 },
                  new Person()
                  {
                      PersonId = Guid.Parse("077e9a14-7ee0-490b-9784-ce813f171073"),
                      PersonName = "Ewen",
                      Email = "ebowers5@chronoengine.com",
                      Address = "88 Bobwhite Point",
                      CountryId = Guid.Parse("ffd39392-fe23-4ec0-955e-130f5f3ad192"),
                      DateOfBirth = DateTime.Parse("2005-03-13"),
                      Gender = "Male",
                      ReceiveNewsLetters = false
                  },
                  new Person()
                  {
                      PersonId = Guid.Parse("5ea0f23a-99ab-4837-a3e6-f92406fc540c"),
                      PersonName = "Ody",
                      Email = "osimner6@archive.org",
                      Address = "51473 Manufacturers Place",
                      CountryId = Guid.Parse("ffd39392-fe23-4ec0-955e-130f5f3ad192"),
                      DateOfBirth = DateTime.Parse("2020-02-25"),
                      Gender = "Male",
                      ReceiveNewsLetters = true
                  },
                   new Person()
                   {
                       PersonId = Guid.Parse("762109fb-65f4-4495-b7ec-34f02f078619"),
                       PersonName = "Florentia",
                       Email = "fdugood7@forbes.com",
                       Address = "3870 Dayton Plaza",
                       CountryId = Guid.Parse("ffd39392-fe23-4ec0-955e-130f5f3ad192"),
                       DateOfBirth = DateTime.Parse("2000-11-24"),
                       Gender = "Female",
                       ReceiveNewsLetters = true
                   },
                   });

            }
        }

        private PersonResponse ConvertPersonIntoPersonResponse(Person person)
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
            return ConvertPersonIntoPersonResponse(personObj);
          
        }

        public List<PersonResponse> GetAllPerson()
        {
            //convert person to personResponse type
            //return list them
           return _persons.Select(x => ConvertPersonIntoPersonResponse(x)).ToList();
        
            
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

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> people, string sortBy, SortOrderOptions sortOrder)
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
           Person? matchingPerson = _persons.FirstOrDefault(x => x.PersonId == request.PersonId);

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
            Person? person = _persons.FirstOrDefault(x => x.PersonId == peronId);
            if (person == null) return false;

            _persons.RemoveAll(x => x.PersonId == peronId);
            return true;
        }
    }
}

