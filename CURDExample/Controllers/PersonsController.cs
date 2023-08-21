﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using ServiceContract.DTO;
using ServiceContract.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeApplication.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService )
        {
            _personService = personService;
        }

        // GET: /<controller>/
        [Route("persons/index")]
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
    }
}

