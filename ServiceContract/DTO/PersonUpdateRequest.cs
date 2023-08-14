using System;
using Entities;
using ServiceContract.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContract.DTO
{
    /// <summary>
    /// Represent the DTo calss that contain person deatils to update
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "person Id can not blank")]
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "person Name can not blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "person Email can not blank")]
        [EmailAddress(ErrorMessage = "Email Value should be a valid email")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderOptions? Gender { get; set; }

        public Guid? CountryId { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }


        public Person ToPerson()
        {
            return new Person()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters

            };
        }
    }
}

