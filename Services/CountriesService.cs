 using Entities;
using ServiceContract;
using ServiceContract.DTO;

namespace Services;


public class CountriesService : ICountryService
{
    //private list
    private readonly List<Country> _countries;

    public CountriesService(bool initialize = true)
    {
        _countries = new List<Country>();
        if (initialize)
        {
            _countries.AddRange(new List<Country>() {
            new Country() {
                CountryId = Guid.Parse("e098d035-94d4-4119-88a4-3f5ff6e54680"),
                CountryName = "USA"
            },
            new Country()
            {
                CountryId = Guid.Parse("68d84e65-555f-48a2-9840-436b5c695b14"),
                CountryName = "India"
            },
            new Country()
            {
                CountryId = Guid.Parse("ffd39392-fe23-4ec0-955e-130f5f3ad192"),
                CountryName = "japan"
            },
            new Country()
            {
                CountryId = Guid.Parse("9baa4486-82d7-4037-b4f1-3b83111d8213"),
                CountryName = "Korea"
            },
            new Country()
            {
                CountryId = Guid.Parse("2b2fc077-39f8-4339-9e7c-134af028f10f"),
                CountryName = "Germany"
            },

            });


        }
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

