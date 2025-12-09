using ErpManagement.Domain.DTOs.Request.Organization.Company;
using ErpManagement.Domain.DTOs.Response.Organization.Company;
using ErpManagement.Services.IServices.Organization;

namespace ErpManagement.API.Areas.Organization.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class CompaniesController(ICompanyService service) : ControllerBase
{
    private readonly ICompanyService _service = service;

    [HttpGet(ApiRoutes.Company.ListOfCompanies)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfCompaniesAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Company.GetAllCompanies)]
    [Produces(typeof(Response<CompanyGetAllResponse>))]
    public async Task<IActionResult> GetAllCompaniesAsync([FromQuery] CompanyGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Company.CreateCompany)]
    public async Task<IActionResult> CreateCompanyAsync(CompanyCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Company.GetCompanyById)]
    [Produces(typeof(Response<CompanyGetByIdResponse>))]
    public async Task<IActionResult> GetCompanyByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Company.UpdateCompany)]
    public async Task<IActionResult> UpdateCompanyAsync([FromRoute] int id, CompanyUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Company.ChangeActiveOrNotCompany)]
    public async Task<IActionResult> UpdateActiveOrNotCompanyAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Company.DeleteCompany)]
    public async Task<IActionResult> DeleteCompanyAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        string lang = Request.Headers.AcceptLanguage.ToString();
        if (lang.StartsWith(RequestLang.Ar)) return RequestLangEnum.Ar;
        else if (lang.StartsWith(RequestLang.En)) return RequestLangEnum.En;
        else return RequestLangEnum.Tr;
    }
}