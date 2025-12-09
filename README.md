You are GitHub Copilot working inside the backend project **ErpManagement**.

Your job is to generate code that follows EXACTLY the final approved architecture of the project.  
You MUST NOT deviate from any rule.  
If something is unclear, ALWAYS follow the Brand module pattern.  
Consistency is REQUIRED — zero exceptions.

===============================================================================
🔥 0. FILE, NAMESPACE & GLOBAL USINGS RULES (VERY IMPORTANT)
===============================================================================

1) You MUST NOT write physical file paths inside any file content.  
   - ❌ No comments like: // file: ErpManagement.Domain/DTOs/...
   - ❌ No hard-coded solution paths or folders.
   - ✅ Just use namespaces.

2) The **namespace alone** is enough to represent the parent folder structure.  
   - Example: `namespace ErpManagement.Domain.DTOs.Request.Products.Brand;`
   - Do NOT repeat or describe folder paths in comments or strings.

3) GLOBAL USINGS RULE (MANDATORY):

   - Each project (API, Domain, DataAccess, Services, etc.) already uses **global usings**.
   - When you introduce a namespace that will be used in many files in that project  
     (e.g. `ErpManagement.Domain.DTOs.Products.Brand`, `ErpManagement.Domain.Interfaces`, `Microsoft.Extensions.Localization`, etc.):
     
     ✔ Add it once to that project’s existing **global usings** file  
       (for example: `GlobalUsings.cs` or the existing global usings file in that project).

     ✔ Prefer:
         `global using ErpManagement.Domain.DTOs.Products.Brand;`

     ❌ Do NOT:
       - Create new global usings files with different names.
       - Hardcode file paths in comments.
       - Scatter normal `using` statements everywhere for things that clearly belong as global usings.

   - For one-off/single-file dependencies, you can keep normal `using` at the top of that file.
   - For core/shared namespaces used across that project, ALWAYS promote them to global using.

===============================================================================
🔥 1. PROJECT ARCHITECTURE (MANDATORY)
===============================================================================

These are the FINAL components you MUST obey:

✔ Multi-tenant architecture  
✔ Global tenant filters (`ITenantEntity`, `TenantEntity`)  
✔ Global soft delete (IsDeleted filter + SaveChanges override)  
✔ `BaseEntity` for audit fields  
✔ Generic `BaseRepository<T>` with:
   - Tenant auto-assignment
   - Soft-delete safe querying
   - IgnoreQueryFilters support
   - Pagination
   - Include string support
✔ `UnitOfWork`
✔ Central DTO patterns
✔ Central Service patterns
✔ Central Controller patterns
✔ Localization (`IStringLocalizer`)
✔ SignalR broadcasting on write operations
✔ AutoMapper profile per module
✔ PermissionsStatic (for authorize attributes)
✔ SDStatic / ApiRoutes for route constants

You MUST NOT introduce any new architecture concepts.  
Use ONLY what is already built.

===============================================================================
🔥 2. RULES FOR ENTITY CREATION (STRICT)
===============================================================================

When creating a new module (e.g., Category, Brand, Unit, Tax, ProductType, Variant, etc.):

✔ Tenant-scoped entities MUST inherit:
      public class X : TenantEntity

✔ Global entities (Country, State, Gender, etc.) MUST inherit:
      public class X : BaseEntity

✔ All IDs MUST be:
      public int Id { get; set; }

✔ NEVER define TenantId manually — TenantEntity already contains it.

✔ Navigation properties must follow EF Core conventions.

✔ DO NOT use GUID.

✔ DO NOT change property types that already exist in the Domain layer or migration.

===============================================================================
🔥 3. REQUIRED DTO SET (STRICT)
===============================================================================

Each module X MUST have ALL of the following DTOs:

1) XCreateRequest  
2) XUpdateRequest  
3) XGetByIdResponse  
4) XGetAllFiltrationsForXRequest : PaginationRequest  
5) PaginatedXData : SelectListMoreResponse  
6) XGetAllResponse : PaginationData<PaginatedXData>  
7) Optional: XSelectListResponse : SelectListResponse

All DTO properties MUST use:

- `[Display(Name = Annotations.X)]`
- `[Required(ErrorMessage = Annotations.FieldIsRequired)]` where applicable
- `[MaxLength(...)]` matching the entity configuration (e.g. 50, 100, 200, 500, 1000)

Follow the same style used in the **Country** and **Brand** modules.

===============================================================================
🔥 4. REPOSITORY RULES (STRICT)
===============================================================================

For every entity X:

✔ Create interface:

      public interface IXRepository : IBaseRepository<X>
      {
          // Optional extra query methods only, no business logic
      }

✔ Create implementation:

      public class XRepository : BaseRepository<X>, IXRepository
      {
          public XRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
              : base(context, currentTenant)
          {
          }
      }

✔ Only pure database queries are allowed here.  
DO NOT write business logic in repositories.

✔ Allowed optional methods (query-only):

- Task<IEnumerable<X>> GetByTenantIdAsync(int tenantId);
- Task<IEnumerable<X>> GetByTypeAsync(string type, int tenantId);
- Task<IEnumerable<X>> GetParentAsync(int tenantId);
- Task<IEnumerable<X>> GetChildrenAsync(int parentId, int tenantId);
- Task<X?> GetWithChildrenAsync(int id, int tenantId);

Nothing else.

===============================================================================
🔥 5. SERVICE RULES (STRICT)
===============================================================================

For module X, create interface:

    public interface IXService
    {
        Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
        Task<Response<XGetAllResponse>> GetAllAsync(RequestLangEnum lang, XGetAllFiltrationsForXRequest model);
        Task<Response<XCreateRequest>> CreateAsync(XCreateRequest model);
        Task<Response<XGetByIdResponse>> GetByIdAsync(int id);
        Task<Response<XUpdateRequest>> UpdateAsync(int id, XUpdateRequest model);
        Task<Response<string>> UpdateActiveOrNotAsync(int id);
        Task<Response<string>> DeleteAsync(int id);
    }

Service IMPLEMENTATION MUST:

✔ ALWAYS use `IUnitOfWork`  
✔ ALWAYS respect tenant filters (do not bypass them)  
✔ ALWAYS check for duplicates using `ExistAsync` with proper conditions  
✔ ALWAYS return `Response<T>`  
✔ ALWAYS use AutoMapper for mapping between Entity and DTO  
✔ ALWAYS broadcast SignalR after Create/Update/Delete:

      await _hubContext.Clients.All.BroadcastMessage();

✔ ALWAYS use localization:

      _sharLocalizer[Localization.X]

✔ ALWAYS use a private helper:

      private async Task<X?> GetObjByIdAsync(int id)

✔ MUST NEVER:
- set TenantId manually (BaseRepository handles it)
- bypass UnitOfWork and talk directly to DbContext
- perform manual property mapping (use AutoMapper)
- use `FindAsync` (must rely on query-filter-respecting methods)
- return raw entities from services

===============================================================================
🔥 6. CONTROLLER RULES (STRICT)
===============================================================================

Every module controller MUST follow the EXACT structure of `BrandsController`.

✔ Decorators:

    [Area(Modules.X)]
    [ApiExplorerSettings(GroupName = Modules.X)]
    [ApiController]
    [Route("api/[controller]")]

✔ Endpoints MUST be:

- `ListAsync`                        → GET    (for dropdowns)
- `GetAllAsync`                      → GET    (with filtrations / pagination)
- `CreateAsync`                      → POST
- `GetByIdAsync`                     → GET
- `UpdateAsync`                      → PUT
- `UpdateActiveOrNotAsync`           → PUT
- `DeleteAsync`                      → DELETE

✔ Authorization MUST use:

    [Authorize(PermissionsStatic.X.View)]
    [Authorize(PermissionsStatic.X.Create)]
    [Authorize(PermissionsStatic.X.Update)]
    [Authorize(PermissionsStatic.X.Delete)]

Following the same pattern as Brand/Category modules.

✔ Language detection MUST use:

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        string lang = Request.Headers.AcceptLanguage.ToString();
        if (lang.StartsWith(RequestLang.Ar)) return RequestLangEnum.Ar;
        else if (lang.StartsWith(RequestLang.En)) return RequestLangEnum.En;
        else return RequestLangEnum.Tr;
    }

✔ Response handling MUST follow:

    if (response.IsSuccess)
        return Ok(response);
    else if (!response.IsSuccess)
        return StatusCode(StatusCodes.Status400BadRequest, response);
    return StatusCode(StatusCodes.Status500InternalServerError, response);

No other patterns are allowed.

===============================================================================
🔥 7. AUTOMAPPER RULES (STRICT)
===============================================================================

Every module X MUST have a dedicated AutoMapper profile:

    public class XProfile : Profile
    {
        public XProfile()
        {
            CreateMap<X, XCreateRequest>().ReverseMap();
            CreateMap<X, XUpdateRequest>().ReverseMap();
            CreateMap<X, XGetByIdResponse>().ReverseMap();
            CreateMap<X, PaginatedXData>().ReverseMap();
        }
    }

No manual mapping is allowed except for very small/simple projections where DTO != entity and AutoMapper is not used in the project for that particular case.  
By default, ALWAYS use AutoMapper.

===============================================================================
🔥 8. DI REGISTRATION (STRICT)
===============================================================================

For every module X you MUST register in the DI configuration:

    services.AddScoped<IXRepository, XRepository>();
    services.AddScoped<IXService, XService>();

Follow the exact pattern already used for:

- `ICountryService, CountryService`
- `IBrandService, BrandService`
- `ICategoryService, CategoryService`
- etc.

===============================================================================
🔥 9. MULTI-TENANCY REQUIREMENTS (STRICT)
===============================================================================

✔ DO NOT EVER assign TenantId manually in Services or Controllers.  
✔ DO NOT bypass global query filters.  
✔ DO NOT use `.FindAsync()` — it may bypass filters.  
✔ ALWAYS use query methods that respect filters (`FirstOrDefaultAsync`, where, etc.).  
✔ `BaseRepository` already sets TenantId for `ITenantEntity` on Create / CreateRange.  
✔ `ErpManagementDbContext` already enforces global tenant filter and soft delete filter.  
✔ `SaveChangesAsync` already applies audit fields and soft delete behavior.

===============================================================================
🔥 10. SIGNALR REQUIREMENTS (STRICT)
===============================================================================

After any **Create**, **Update**, or **Delete** in a Service:

    await _hubContext.Clients.All.BroadcastMessage();

This is mandatory for UI auto-refresh.  
Follow the same pattern as in the Country / Brand service.

===============================================================================
🔥 11. LOCALIZATION RULES (STRICT)
===============================================================================

Use localization for ALL user-facing messages:

    _sharLocalizer[Localization.Done]
    _sharLocalizer[Localization.NotFoundData]
    _sharLocalizer[Localization.IsExist]
    _sharLocalizer[Localization.Updated]
    _sharLocalizer[Localization.Deleted]
    _sharLocalizer[Localization.Activated]
    _sharLocalizer[Localization.DeActivated]
    // etc.

DO NOT hardcode English strings directly in the services or controllers.

===============================================================================
🔥 12. OUTPUT FORMAT (STRICT)
===============================================================================

When I ask you to “create CRUD for X”, you MUST output **ALL required parts**, in this order:

1. DTOs (Request + Response)  
2. Repository interface  
3. Repository implementation  
4. Service interface  
5. Service implementation  
6. Controller  
7. AutoMapper Profile  
8. DI registration snippet  

If the Entity already exists in the Domain, do NOT modify it unless I explicitly say so.

EVERYTHING must use the EXACT final style, naming, layout, and logic already in the project.

===============================================================================
🔥 13. ABSOLUTELY FORBIDDEN (STRICT)
===============================================================================

❌ Do NOT:
- Invent new architecture patterns  
- Rename existing classes, namespaces, or conventions  
- Produce incomplete modules  
- Ignore tenant model  
- Use GUIDs for IDs  
- Add new folders or layers  
- Add minimal APIs, MediatR, CQRS, etc.  
- Change controller method signatures  
- Break naming conventions  
- Bypass UnitOfWork or DbContext lifetime rules  
- Write file system paths inside code files  

===============================================================================
🔥 END OF RULES
===============================================================================

You MUST follow these rules WITHOUT EXCEPTION.  
If anything is unclear, copy the **Brand** module EXACTLY and adapt it for the new entity X.  
Your outputs MUST ALWAYS compile inside this architecture without modifications.
#   E r p M a n a g e m e n t  
 