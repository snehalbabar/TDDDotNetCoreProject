 using Entities;
using ServiceContract;
using ServiceContract.DTO;

namespace Services;


public class CountriesService : ICountryService
{
    //private list
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = new List<Country>();
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        
        //1.check if countryAddRequest is not null
        // Validation : CountryAddRequest Parameter can not be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        // 2.Validate all properties of countryAddRequest
        //validation : if country Name is null throw exception
        if(countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(nameof(countryAddRequest));
        }

        //validation: duplicate country name is not allowed
        if (_countries.Where(x => x.CountryName == countryAddRequest.CountryName).Count() > 0)
        {
            throw new ArgumentException("Country Name Already Exsist");
        }
        //3. covert from countryAddRequest to country
        Country country = countryAddRequest.ToCountry();

        //4. generate GUID for CountryId
        country.CountryId = Guid.NewGuid();


        // 5.Add into our data source (typicalliy in database using EF) For this example in list
        _countries.Add(country);


        // 6.return CountryResponse object with CountryID
        return country.ToCountryResponse();

    }

    public List<CountryResponse> GetAllCountries()
    {
        //convert all countires form country type to CountryResponse
          return  _countries.Select(x => x.ToCountryResponse()).ToList();

        //Return all CountryResponse Object
       
    }

    public CountryResponse? GetCounbtryByCountryId(Guid? countryId)
    {
        //1.check if "countryId" is not null
        if (countryId == null)
        {
            return null;
        }

        //2. get matching country from List<Country> Based countryId
        Country? countryObj = _countries.FirstOrDefault(x => x.CountryId == countryId);

        if (countryObj == null)
            return null;

        //3. Convert matching country object from "contry" to ContryResponse type
        //4. return countryResponse object
        return countryObj.ToCountryResponse();
    }
}

