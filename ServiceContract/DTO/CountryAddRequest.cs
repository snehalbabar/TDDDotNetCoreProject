﻿using System;
using Entities;

namespace ServiceContract.DTO
{
	public class CountryAddRequest
	{
		public string? CountryName { get; set; }

		public Country ToCountry()
		{
			return new Country() { CountryName = this.CountryName };
		}

	}
}

