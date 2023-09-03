using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContract.Enums;

namespace ServiceContract.DTO
{
    /// <summary>
    /// Act as DTO for adding a person
    /// </summary>
	public class PersonAddRequest
	{
        [Required(ErrorMessage ="person Name can not blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "person Email can not blank")]
        [EmailAddress(ErrorMessage = "Email Value should be a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public GenderOptions? Gender { get; set; }

        [Required(ErrorMessage ="select valid Country")]
        public Guid? CountryId { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }


        public Person ToPerson()
        {
            return new Person()
            {
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

