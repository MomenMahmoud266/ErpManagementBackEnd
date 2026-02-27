namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CountriesController(ICountryService service) : ControllerBase
{
    private readonly ICountryService _service = service;

    [HttpGet(ApiRoutes.Country.ListOfCountries)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfCountriesAsync() =>
        Ok(await _service.ListOfCountriesAsync(GetCurrentRequestLanguage()));

    //[Authorize(PermissionsStatic.Country.View)]
    //[HttpGet(ApiRoutes.Country.GetAllCountries)]
    //[Produces(typeof(Response<SharGetAllCountriesResponse>))]
    //public async Task<IActionResult> GetAllCountriesAsync([FromQuery] SharGetAllFiltrationsForCountriesRequest model) =>
    //    Ok(await _service.GetAllCountriesAsync(GetCurrentRequestLanguage(), model));

    //[Authorize(PermissionsStatic.Country.Create)]
    //[HttpPost(ApiRoutes.Country.CreateCountry)]
    //public async Task<IActionResult> CreateCountryAsync(SharCreateCountryRequest model)
    //{
    //    var response = await _service.CreateCountryAsync(model);
    //    if (response.IsSuccess)
    //        return Ok(response);
    //    else if (!response.IsSuccess)
    //        return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
    //    return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    //}

    //[Authorize(PermissionsStatic.Country.View)]
    //[HttpGet(ApiRoutes.Country.GetCountryById)]
    //[Produces(typeof(Response<SharGetCountryByIdResponse>))]
    //public async Task<IActionResult> GetJobByIdAsync([FromRoute] int id)
    //{
    //    var response = await _service.GetCountryByIdAsync(id);
    //    if (response.IsSuccess)
    //        return Ok(response);
    //    else if (!response.IsSuccess)
    //        return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
    //    return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    //}

    //[Authorize(PermissionsStatic.Country.Update)]
    //[HttpPut(ApiRoutes.Country.UpdateCountry)]
    //public async Task<IActionResult> UpdateCountryAsync([FromRoute] int id, SharUpdateCountryRequest model)
    //{
    //    var response = await _service.UpdateCountryAsync(id, model);
    //    if (response.IsSuccess)
    //        return Ok(response);
    //    else if (!response.IsSuccess)
    //        return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
    //    return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    //}

    //[Authorize(PermissionsStatic.Country.Update)]
    //[HttpPut(ApiRoutes.Country.ChangeActiveOrNotCountry)]
    //public async Task<IActionResult> UpdateActiveOrNotCountryAsync([FromRoute] int id)
    //{
    //    var response = await _service.UpdateActiveOrNotCountryAsync(id);
    //    if (response.IsSuccess)
    //        return Ok(response);
    //    else if (!response.IsSuccess)
    //        return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
    //    return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    //}

    //[Authorize(PermissionsStatic.Country.Delete)]
    //[HttpDelete(ApiRoutes.Country.DeleteCountry)]
    //public async Task<IActionResult> DeleteCountryAsync([FromRoute] int id)
    //{
    //    var response = await _service.DeleteCountryAsync(id);
    //    if (response.IsSuccess)
    //        return Ok(response);
    //    else if (!response.IsSuccess)
    //        return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
    //    return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    //}

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        string lang = Request.Headers.AcceptLanguage.ToString();

        if (lang.StartsWith(RequestLang.Ar))
            return RequestLangEnum.Ar;

        else if (lang.StartsWith(RequestLang.En))
            return RequestLangEnum.En;

        else
            return RequestLangEnum.Tr;
    }
}
