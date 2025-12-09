namespace ErpManagement.Domain.Dtos.Response;

public class SelectListResponse 
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SelectListForUserResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
public class SelectListMoreResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CheckBox
{
    public string DisplayValue { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}


