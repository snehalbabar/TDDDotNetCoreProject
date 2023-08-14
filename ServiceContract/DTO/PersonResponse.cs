using System;
using Entities;
using ServiceContract.Enums;

namespace ServiceContract.DTO
{
    /// <summary>
    /// Represent DTO class thhhat is used as return type of most methods of person services.
    /// </summary>
	public class PersonResponse
	{
        public Guid PersonId { get; set; }

        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public double? Age { get; set; }

        public string? Gender { get; set; }

        public Guid? CountryId { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;

            return (PersonId == person.PersonId
                    && PersonName == person.PersonName
                    && Email == person.Email
                    && DateOfBirth == person.DateOfBirth
                    && Age == person.Age
                    && Gender == person.Gender
                    && CountryId == person.CountryId
                    && Country == person.Country
                    && Address == person.Address
                    && ReceiveNewsLetters == person.ReceiveNewsLetters);

        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {

            return $"personId : {PersonId}, personName: {PersonName}," +
                $"Email : {Email}, DateOfBith: {DateOfBirth?.ToString("yyyy dd mm")}," +
                $"Gender : {Gender}, CountryId: {CountryId}";
        }
    }
    /// <summary>
    /// An extension method to convert an object of person class into persoanResponse Class
    /// </summary>
    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            //create and return new person response object
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth !=null) ?
                       Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays/365.25)
                       : null

            };
        }
    }
}

