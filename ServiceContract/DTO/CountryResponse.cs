using System;
using Entities;

namespace ServiceContract.DTO
{
	/// <summary>
	/// Dto class that is used as return type for most of CountriesService Methods
	/// </summary>
	public class CountryResponse
	{
		public Guid CountryId { get; set; }

		public string? CountryName { get; set; }

		//It compres the current object to another object of
		//countryResponse type and returns true, if both values are same;
        public override bool Equals(object? obj)
        {
			if (obj == null)
			{ return false; }

			if (obj.GetType() !=   typeof(CountryResponse))
			{
				return false;
			}
			CountryResponse convertedObj = (CountryResponse)obj;
			return CountryId == convertedObj.CountryId && CountryName == convertedObj.CountryName;

        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

	public static class CountryExtensions
	{
		//convert country objcet to countryresponse object
		public static CountryResponse  ToCountryResponse(this Country country)
		{
			return new CountryResponse()
			{
				CountryId = country.CountryId,
				CountryName = country.CountryName
			};
		}
	}
}

