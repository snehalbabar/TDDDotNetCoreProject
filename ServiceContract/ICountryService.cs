using ServiceContract.DTO;

namespace ServiceContract;

/// <summary>
/// Represnet business logic for manipulating country enity
/// </summary>
public interface ICountryService
{
    /// <summary>
    /// add acountry object to the list of countries
    /// </summary>
    /// <param name="countryAddRequest"></param>
    /// <returns>REturns the country object after adding it.</returns>
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);


    /// <summary>
    /// Returns all countires from the list
    /// </summary>
    /// <returns>list of country </returns>
    List<CountryResponse> GetAllCountries();


    /// <summary>
    /// Return country object based on given country id
    /// </summary>
    /// <param name="countyId"></param>
    /// <returns>matching country object</returns>
    CountryResponse? GetCounbtryByCountryId(Guid? countyId);
  
}

