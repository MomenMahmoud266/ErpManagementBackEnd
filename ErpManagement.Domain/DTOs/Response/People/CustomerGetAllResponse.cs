namespace ErpManagement.Domain.DTOs.Response.People.Customer;

public class CustomerGetAllResponse : PaginationData<CustomerPaginatedData>
{
}

public class CustomerPaginatedData : SelectListMoreResponse
{
}