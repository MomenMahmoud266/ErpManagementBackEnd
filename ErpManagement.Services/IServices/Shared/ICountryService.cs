namespace ErpManagement.Services.IServices.Shared;

public interface ICountryService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListOfCountriesAsync(RequestLangEnum lang);
    //Task<Response<SharGetAllCountriesResponse>> GetAllCountriesAsync(RequestLangEnum lang, SharGetAllFiltrationsForCountriesRequest model);
    //Task<Response<SharCreateCountryRequest>> CreateCountryAsync(SharCreateCountryRequest model);
    //Task<Response<SharGetCountryByIdResponse>> GetCountryByIdAsync(int id);
    //Task<Response<SharUpdateCountryRequest>> UpdateCountryAsync(int id, SharUpdateCountryRequest model);
    //Task<Response<string>> UpdateActiveOrNotCountryAsync(int id);
    //Task<Response<string>> DeleteCountryAsync(int id);
}
