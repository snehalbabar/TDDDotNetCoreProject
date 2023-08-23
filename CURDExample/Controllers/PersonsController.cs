using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContract;
using ServiceContract.DTO;
using ServiceContract.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeApplication.Controllers
{
    [Route("Persons")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;

        public PersonsController(IPersonService personService , ICountryService countryService)
        {
            _personService = personService;
            _countryService = countryService;
        }

        // GET: /<controller>/
        [Route("index")]
        [Route("/")]
        public IActionResult Index(string searchBy,  string? searchString,
            string sortBy= nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder= SortOrderOptions.ASC)
        {
            //searching
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {  nameof(PersonResponse.PersonName), "Person Name" },
                {  nameof(PersonResponse.Email), "Email" },
                {  nameof(PersonResponse.DateOfBirth), "Date Of Birth" },
                {  nameof(PersonResponse.Gender), "Gender" },
                {  nameof(PersonResponse.CountryId), "Country" },
                {  nameof(PersonResponse.Address), "Address" },
               
            };
            List<PersonResponse> personResponses =  _personService.GetFilteredPerson(searchBy,searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //sort

            List<PersonResponse> SortedpersonResponses =_personService.GetSortedPersons(personResponses, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(SortedpersonResponses); 
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries= _countryService.GetAllCountries();
            ViewBag.AllCountries = countries.Select(x => new SelectListItem() { Text = x.CountryName, Value=x.CountryId.ToString()});
            
            return View();
        }

       
        [HttpPost]
        [Route("[action]")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if(!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countryService.GetAllCountries();
                ViewBag.AllCountries = countries;
                ViewBag.Errors= ModelState.Values.SelectMany(er => er.Errors).SelectMany(e => e.ErrorMessage).ToList();
                return View();
            }
            //call the service method
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);


            return RedirectToAction("Index","Persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Update(Guid personId)
        {
            PersonResponse? person = _personService.GetPersonByPersonId(personId);
            if (person == null)
            {
                return RedirectToAction("Index");
            }
            PersonUpdateRequest updateRequest = person.ToPersonUpdateRequest();

            List<CountryResponse> countries = _countryService.GetAllCountries();
            ViewBag.AllCountries = countries.Select(x => new SelectListItem() { Text = x.CountryName, Value = x.CountryId.ToString() });

            return View(updateRequest);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Update(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? person = _personService.GetPersonByPersonId(personUpdateRequest.PersonId);
            if (person == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = _countryService.GetAllCountries();
                ViewBag.AllCountries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(er => er.Errors).SelectMany(e => e.ErrorMessage).ToList();
                return View(person.ToPersonUpdateRequest());
            }          

        }


        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Delete(Guid? personId)
        {
           PersonResponse? person = _personService.GetPersonByPersonId(personId);
            if(person == null)
            {
                return RedirectToAction("Index");
            }
            return View(person);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? person = _personService.GetPersonByPersonId(personUpdateRequest.PersonId);
            if (person == null) return RedirectToAction("Index");

            _personService.DeletePerson(person.PersonId);
            return RedirectToAction("Index");
        }
    }
}

