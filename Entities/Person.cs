using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
	/// <summary>
	/// person Domain Model class
	/// </summary>
	public class Person
	{
		[Key]
		public Guid PersonId { get; set; }


		[StringLength(40)]
		public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email{ get; set; }

        public DateTime? DateOfBirth { get; set; }

		[StringLength(10)]
		public string? Gender { get; set; }

		//uniqueIdentifyer
        public Guid? CountryId { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

		//bit
        public bool ReceiveNewsLetters { get; set; }


    }
}

