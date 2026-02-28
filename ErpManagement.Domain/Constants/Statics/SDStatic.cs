namespace ErpManagement.Domain.Constants.Statics;

public static class SDStatic
{
    public static class GoRootPath
    {
        public const string SettingImagesPath = "/wwwroot/Images/Setting/";
        public const string SettingFilesPath = "/wwwroot/Files/Setting/";
        public const string SettingAudiosPath = "/wwwroot/Audios/Setting/";
        public const string SettingVideosPath = "/wwwroot/Videos/";

        public const string HRImagesPath = "/wwwroot/Images/HR/";
        public const string HRFilesPath = "/wwwroot/Files/HR/";
        public const string HRAudiosPath = "/wwwroot/Audios/HR/";
        public const string HRVideosPath = "/wwwroot/Videos/HR/";
    }
    public static class ReadRootPath
    {
        public const string SettingImagesPath = "Images/Setting/";
        public const string SettingFilesPath = "Files/Setting/";
        public const string SettingAudiosPath = "Audios/Setting/";
        public const string SettingVideosPath = "wwwroot/Videos/";

        public const string HRImagesPath = "Images/HR/";
        public const string HRFilesPath = "Files/HR/";
        public const string HRAudiosPath = "Audios/HR/";
        public const string HRVideosPath = "Videos/HR/";
    }

    public static class FileSettings
    {
        public const string SpecialChar = @"|!#$%&[]=?»«@£§€{};<>";
        public const int Length = 50;
    }
    public static class Modules
    {
        public const string Auth = "Auth";
        public const string Shared = "Shared";
        public const string Transactions = "Transactions";
        public const string Product = "Product";
        public const string HR = "HR";
        public const string V1 = "v1";
        public const string Bearer = "bearer";

    }
    public static class SuperAdmin
    {
        public const string Id = "b74ddd14-6340-4840-95c2-db12554843e5basb1";
        public const string RoleId = "fab4fac1-c546-41de-aebc-a14da68957ab1";
        public static string Password = "devsuperadmin96";
        public static string RoleNameInAr = "سوبر أدمن";
        public static string RoleNameInTr = "süper yönetici";
    }

    public static class Admin
    {
        public const string Id = "b74ddd14-6340-4840-95c2-db12554843e5basb2";
        public const string RoleId = "fab4fac1-c546-41de-aebc-a14da68957ab2";
        public static string Password = "devadmin96";
        public static string RoleNameInAr = "أدمن";
        public static string RoleNameInTr = "yönetici";
    }
    public static class Roles
    {
        public const string Administrative = "Administrative";
        public const string User = "User";
        public const string SuperAdmin = "SuperAdmin";
    }
    public class RequestLang
    {
        public const char Ar = 'a';
        public const char En = 'e';
        public const char Tr = 't';
    }
    public static class RequestClaims
    {
        public const string Permission = "Permission";
        public const string DomainRestricted = "DomainRestricted";
        public const string UserId = "uid";
        public const string TenantId = "tid";
    }
    public static class Shared
    {
        //public const string ErpManagement = "ErpManagement";
        public const string ErpManagement = "ErpManagement";
        public const string ErpManagementConnection = "ErpManagementConnection";
        public const string JwtSettings = "JwtSettings";
        public const string AccessToken = "access_token";
        public const string CorsPolicy = "CorsPolicy";
        public const string Development = "Development";
        public const string Production = "Production";
        public const string Local = "Local";
        public const string RealimeViewData = "/RealimeViewData";
        public static string[] Cultures = ["en-US", "ar-EG", "tr"];
        public const string Resources = "Resources";
        public const string Company = "Company";
        public const string Department = "Department";
        public const string Task = "Task";
    }
    public static class ApiRoutes
    {
        public class User
        {
            public const string GetAllUsers = "GetAllUsers";
            public const string CreateUser = "CreateUser";
            public const string LoginUser = "LoginUser";
            public const string LogOutUser = "LogOutUser";
            public const string LoginUser1 = "LoginUser1";
            public const string ChangeActiveOrNotUser = "ChangeActiveOrNotUser/{id}";
            public const string UpdateUser = "UpdateUser/{id}";
            public const string ShowPasswordToSpecificUser = "ShowPasswordToSpecificUser/{id}";
            public const string ChangePassword = "ChangePassword";
            public const string DeleteUser = "DeleteUser/{id}";
            public const string SetNewPasswordToSpecificUser = "SetNewPasswordToSpecificUser";
            public const string SetNewPasswordToSuperAdmin = "SetNewPasswordToSuperAdmin/{newPassword}";
        }

        public class Perm
        {
            public const string GetAllRoles = "GetAllRoles";
            public const string CreateRole = "CreateRole";
            public const string UpdateRole = "UpdateRole/{id}";
            public const string DeleteRole = "DeleteRole/{id}";

            public const string GetEachUserWithHisRoles = "GetEachUserWithHisRoles";
            public const string GetUserRoles = "GetUserRoles/{userId}";
            public const string UpdateUserRoles = "UpdateUserRoles";

            public const string GetAllPermissionsByCategoryName = "GetAllPermissionsByCategoryName";
            public const string GetRolePermissions = "GetRolePermissions/{roleId}";
            public const string UpdateRolePermissions = "UpdateRolePermissions";
        }
        public class Country
        {
            public const string ListOfCountries = "ListOfCountries";
            public const string GetAllCountries = "GetAllCountries";
            public const string CreateCountry = "CreateCountry";
            public const string GetCountryById = "GetCountryById/{id:int}";
            public const string UpdateCountry = "UpdateCountry/{id:int}";
            public const string ChangeActiveOrNotCountry = "ChangeActiveOrNotCountry/{id:int}";
            public const string DeleteCountry = "DeleteCountry/{id:int}";
        }
        public class Branch
        {
            public const string ListOfBranches = "ListOfBranches";
            public const string GetAllBranches = "GetAllBranches";
            public const string CreateBranch = "CreateBranch";
            public const string GetBranchById = "GetBranchById/{id:int}";
            public const string UpdateBranch = "UpdateBranch/{id:int}";
            public const string ChangeActiveOrNotBranch = "ChangeActiveOrNotBranch/{id:int}";
            public const string DeleteBranch = "DeleteBranch/{id:int}";
        }
        public class Department
        {
            public const string ListOfDepartments = "ListOfDepartments";
            public const string GetAllDepartments = "GetAllDepartments";
            public const string CreateDepartment = "CreateDepartment";
            public const string GetDepartmentById = "GetDepartmentById/{id:int}";
            public const string UpdateDepartment = "UpdateDepartment/{id:int}";
            public const string ChangeActiveOrNotDepartment = "ChangeActiveOrNotDepartment/{id:int}";
            public const string DeleteDepartment = "DeleteDepartment/{id:int}";
        }
        public class Product
        {
            public const string ListOfProducts = "ListOfProducts";
            public const string GetAllProducts = "GetAllProducts";
            public const string CreateProduct = "CreateProduct";
            public const string GetProductById = "GetProductById/{id:int}";
            public const string UpdateProduct = "UpdateProduct/{id:int}";
            public const string UpdateActiveOrNotProduct = "{id:int}/toggle-active";
            public const string DeleteProduct = "DeleteProduct/{id:int}";
        }
        public class Supplier
        {
            public const string ListOfSuppliers = "ListOfSuppliers";
            public const string GetAllSuppliers = "GetAllSuppliers";
            public const string CreateSupplier = "CreateSupplier";
            public const string GetSupplierById = "GetSupplierById/{id:int}";
            public const string UpdateSupplier = "UpdateSupplier/{id:int}";
            public const string ChangeActiveOrNotSupplier = "ChangeActiveOrNotSupplier/{id:int}";
            public const string DeleteSupplier = "DeleteSupplier/{id:int}";
        }
        public class Customer
        {
            public const string ListOfCustomers = "ListOfCustomers";
            public const string GetAllCustomers = "GetAllCustomers";
            public const string CreateCustomer = "CreateCustomer";
            public const string GetCustomerById = "GetCustomerById/{id:int}";
            public const string UpdateCustomer = "UpdateCustomer/{id:int}";
            public const string ChangeActiveOrNotCustomer = "ChangeActiveOrNotCustomer/{id:int}";
            public const string DeleteCustomer = "DeleteCustomer/{id:int}";
        }
        public class Biller
        {
            public const string ListOfBillers = "ListOfBillers";
            public const string GetAllBillers = "GetAllBillers";
            public const string CreateBiller = "CreateBiller";
            public const string GetBillerById = "GetBillerById/{id:int}";
            public const string UpdateBiller = "UpdateBiller/{id:int}";
            public const string ChangeActiveOrNotBiller = "ChangeActiveOrNotBiller/{id:int}";
            public const string DeleteBiller = "DeleteBiller/{id:int}";
        }
        public static class Category
        {
            public const string ListOfCategories = "list";
            public const string GetAllCategories = "";
            public const string CreateCategory = "";
            public const string GetCategoryById = "{id:int}";
            public const string UpdateCategory = "{id:int}";
            public const string UpdateActiveOrNotCategory = "{id:int}/toggle-active";
            public const string DeleteCategory = "{id:int}";
        }

        public static class Brand
        {
            public const string ListOfBrands = "list";
            public const string GetAllBrands = "";
            public const string CreateBrand = "";
            public const string GetBrandById = "{id:int}";
            public const string UpdateBrand = "{id:int}";
            public const string UpdateActiveOrNotBrand = "{id:int}/toggle-active";
            public const string DeleteBrand = "{id:int}";
        }

        public static class Unit
        {
            public const string ListOfUnits = "list";
            public const string GetAllUnits = "";
            public const string CreateUnit = "";
            public const string GetUnitById = "{id:int}";
            public const string UpdateUnit = "{id:int}";
            public const string UpdateActiveOrNotUnit = "{id:int}/toggle-active";
            public const string DeleteUnit = "{id:int}";
        }

        public static class Tax
        {
            public const string ListOfTaxes = "list";
            public const string GetAllTaxes = "";
            public const string CreateTax = "";
            public const string GetTaxById = "{id:int}";
            public const string UpdateTax = "{id:int}";
            public const string UpdateActiveOrNotTax = "{id:int}/toggle-active";
            public const string DeleteTax = "{id:int}";
        }

        public static class Variant
        {
            public const string ListOfVariants = "list";
            public const string GetAllVariants = "";
            public const string CreateVariant = "";
            public const string GetVariantById = "{id:int}";
            public const string UpdateVariant = "{id:int}";
            public const string UpdateActiveOrNotVariant = "{id:int}/toggle-active";
            public const string DeleteVariant = "{id:int}";
        }

        public static class ProductType
        {
            public const string ListOfProductTypes = "list";
            public const string GetAllProductTypes = "";
            public const string CreateProductType = "";
            public const string GetProductTypeById = "{id:int}";
            public const string UpdateProductType = "{id:int}";
            public const string UpdateActiveOrNotProductType = "{id:int}/toggle-active";
            public const string DeleteProductType = "{id:int}";
        }

        // newly added Company routes
        public class Company
        {
            public const string ListOfCompanies = "ListOfCompanies";
            public const string GetAllCompanies = "GetAllCompanies";
            public const string CreateCompany = "CreateCompany";
            public const string GetCompanyById = "GetCompanyById/{id:int}";
            public const string UpdateCompany = "UpdateCompany/{id:int}";
            public const string ChangeActiveOrNotCompany = "ChangeActiveOrNotCompany/{id:int}";
            public const string DeleteCompany = "DeleteCompany/{id:int}";
        }

        // newly added Warehouse routes
        public class Warehouse
        {
            public const string ListOfWarehouses = "ListOfWarehouses";
            public const string GetAllWarehouses = "GetAllWarehouses";
            public const string CreateWarehouse = "CreateWarehouse";
            public const string GetWarehouseById = "GetWarehouseById/{id:int}";
            public const string UpdateWarehouse = "UpdateWarehouse/{id:int}";
            public const string UpdateActiveOrNotWarehouse = "UpdateActiveOrNotWarehouse/{id:int}";
            public const string DeleteWarehouse = "DeleteWarehouse/{id:int}";
        }
        public class Sale
        {
            public const string GetAll = "GetAll";
            public const string Create = "Create";
            public const string GetById = "GetById/{id:int}";
            public const string Update = "Update/{id:int}";
            public const string ChangeActiveOrNot = "ChangeActiveOrNot/{id:int}";
            public const string Delete = "Delete/{id:int}";
        }

        public class Tenant
        {
            public const string Me = "Me";
        }

        public class Appointment
        {
            public const string GetAll = "GetAll";
            public const string Create = "Create";
            public const string GetById = "GetById/{id}";
            public const string Update = "Update/{id}";
            public const string Delete = "Delete/{id}";
            public const string CompleteAndInvoice = "CompleteAndInvoice/{id}";
        }

        public class Gym
        {
            public const string GetAllPlans = "GetAllPlans";
            public const string CreatePlan = "CreatePlan";
            public const string UpdatePlan = "UpdatePlan/{id}";
            public const string DeletePlan = "DeletePlan/{id}";
            public const string PurchaseMembership = "PurchaseMembership";
            public const string CheckIn = "CheckIn";
        }

        public class Cashbox
        {
            public const string GetAll = "GetAll";
            public const string Create = "Create";
            public const string OpenShift = "OpenShift";
            public const string CloseShift = "CloseShift";
            public const string AddMovement = "AddMovement";
            public const string ShiftById = "ShiftById/{id}";
            public const string Ledger = "Ledger";
            public const string ShiftLedger = "ShiftLedger/{id}";
            public const string TreasurySummary = "TreasurySummary";
        }

        public class Statements
        {
            public const string Customer = "Customer";
            public const string Supplier = "Supplier";
        }

        public class Reports
        {
            public const string ProfitLoss = "ProfitLoss";
        }

        public class InventoryPeriods
        {
            public const string GetAll = "GetAll";
            public const string Create = "Create";
            public const string GetById = "GetById/{id:int}";
            public const string Close = "Close/{id:int}";
            public const string AddPhysicalCount = "AddPhysicalCount/{periodId:int}";
            public const string Delete = "Delete/{id:int}";
        }

    }

    public static class Localization
    {
        public const string Done = "Done";
        public const string Updated = "Updated";
        public const string Deleted = "Deleted";
        public const string Activated = "Activated";
        public const string DeActivated = "DeActivated";
        public const string CannotBeFound = "CannotBeFound";
        public const string IsExist = "IsExist";
        public const string CannotDeleteHasChildren = "CannotDeleteHasChildren";

        public class Shared
        {
            // Added entity keys used by services for localized messages
            public const string Country = "Country";
            public const string Category = "Category";
            public const string Brand = "Brand";
            public const string Unit = "Unit";
            public const string Tax = "Tax";
            public const string Variant = "Variant";
            public const string Product = "Product";

            // newly added for organization modules
            public const string Warehouse = "Warehouse";
            public const string Stock = "Stock";
            public const string Branch = "Branch";
            public const string Company = "Company";

            // newly added existing keys kept
            public const string Customer = "Customer";
            public const string Supplier = "Supplier";
            public const string Biller = "Biller";
            public const string CompanyBranch = "CompanyBranch";
        }
        public class Auth
        {
            public const string Role = "Role";
        }



        public const char Ar = 'a';
        public const string Arabic = "ar-EG";
        public const string English = "en-US";
        public const string Project = "Project";
        public const string Task = "Task";
        public const string Notification = "Notification";
        public const string CanNotAddingToNotThereIsProjectAndDepartment = "CanNotAddingToNotThereIsProjectAndDepartment";

        public const string CannotUpdateTaskStatus = "CannotUpdateTaskStatus";
        public const string TaskExist = "TaskExist";
        public const string Department = "Department";
        public const string TaskComment = "TaskComment";
        public const string EmployeeVacation = "EmployeeVacation";
        public const string DepartmentManager = "DepartmentManager";
        public const string Error = "Error";
        public const string ThisAmountCannotBePaidFromTheMainTreasuryDueToItsAvailability = "ThisAmountCannotBePaidFromTheMainTreasuryDueToItsAvailability";
        public const string ThisAmountCannotBePaidFromTheTreasuryBranchDueToItsAvailability = "ThisAmountCannotBePaidFromTheTreasuryBranchDueToItsAvailability";
        public const string ThisAmountCannotBeTransferedFromTheTreasuryDueToItsAvailability = "ThisAmountCannotBeTransferedFromTheTreasuryDueToItsAvailability";
        public const string ThisAmountCannotBeTransferedFromTheBranchTreasuryDueToItsAvailability = "ThisAmountCannotBeTransferedFromTheBranchTreasuryDueToItsAvailability";
        public const string ThisAmountCannotBeReceitedAsThisClientHasNotPrice = "ThisAmountCannotBeReceitedAsThisClientHasNotPrice";
        public const string ItIsNecessaryThatAmountMoreThanZero = "ItIsNecessaryThatAmountMoreThanZero";

        public const string Departments = "Departments";

        public const string MainScreenCategory = "MainScreenCategory";
        public const string MainScreen = "MainScreen";
        public const string SubMainScreen = "SubMainScreen";
        public const string DepartmentsExist = "DepartmentsExist";
        public const string Jobs = "Jobs";
        public const string JobExist = "JobExist";
        public const string Projects = "Projects";
        public const string ProjectsExisit = "ProjectsExisit";
        public const string Tasks = "Tasks";
        public const string CannotDeletedThisRole = "CannotDeletedThisRole";
        public const string Service = "Service";
        public const string ServicesCategory = "ServicesCategory";
        public const string Policy = "Policy";
        public const string News = "News";

        public const string CurrentAndNewPasswordIsTheSame = "CurrentAndNewPasswordIsTheSame";
        public const string CurrentPasswordIsIncorrect = "CurrentPasswordIsIncorrect";
        public const string UserName = "UserName";
        public const string UserNameOrEmail = "UserNameOrEmail";
        public const string User = "User";
        public const string ThisEmployeeWasDeleted = "ThisEmployeeWasDeleted";
        public const string NotFound = "NotFound";
        public const string Email = "Email";
        public const string PasswordNotmatch = "PasswordNotmatch";

        public const string Employee = "Employee";
        public const string EmployeeExist = "EmployeeExist";
        public const string ThereAreNotAttachments = "ThereAreNotAttachments";
        public const string CanNotAssignAnyEmpToFindingOther = "CanNotAssignAnyEmpToFindingOther";
        public const string CanNotAssignAnyEmpToFindingComManager = "CanNotAssignAnyEmpToFindingComManager";
        public const string CanNotRemoveThisEmployeeAsHasAmountInHisTreasury = "CanNotRemoveThisEmployeeAsHasAmountInHisTreasury";
        public const string Request = "Request";

        public const string Job = "Job";
        public const string Category = "Category";
        //public const string Item = "Item";

        public const string Section = "Section";
        public const string ClientCategory = "ClientCategory";
        public const string ThisEmployeeIsNotTechnician = "ThisEmployeeIsNotTechnician";
        public const string CompanyBranch = "CompanyBranch";
        public const string LockTechnicalsLogins = "LockTechnicalsLogins";
        public const string UnLockTechnicalsLogins = "UnLockTechnicalsLogins";
        public const string ThereIsActiveEmployeesRelatedToThisBranch = "ThereIsActiveEmployeesRelatedToThisBranch";
        public const string UserWasLoggedOutBefore = "UserWasLoggedOutBefore";
        public const string PleaseChangeEmployeeActivationState = "PleaseChangeEmployeeActivationState";
        public const string TheEmployeeNotActive = "TheEmployeeNotActive";
        public const string Region = "Region";
        public const string State = "State";
        public const string TaxOffice = "TaxOffice";
        public const string UserNotExist = "UserNotExist";
        public const string UserIsLoggedOut = "UserIsLoggedOut";
        public const string Location = "Location";
        public const string UserWithEmailExists = "UserWithEmailExists";
        public const string UserWithNameExists = "UserWithNameExists";
        public const string CompanyIsNotActivated = "CompanyIsNotActivated";
        public const string Email_Password_InCORRECT = "Email_Password_INCORRECT";
        public const string UserDataIsIncorrect = "UserDataIsIncorrect";
        public const string Company = "Company";
        public const string Allowance = "Allowance";
        public const string Benefit = "Benefit";
        public const string Qualification = "Qualification";
        public const string Deduction = "Deduction";

        public const string Image = "Image";
        public const string CompanyIsNotActive = "Company is not active";
        public const string NotActive = "NotActive";
        public const string NotActiveNotUpdate = "NotActiveNotUpdate";
        public const string NotFoundMainBranchToCompany = "NotFoundMainBranchToCompany";
        public const string NotFoundData = "NotFoundData";
        public const string CanNotAddCommentToSpecificComment = "CanNotAddCommentToSpecificComment";
        public const string Technician = "Technician";
        public const string UserIsAlreadyLoggedIn = "UserIsAlreadyLoggedIn";
        public const string HasAnyRelation = "HasAnyRelation";
        public const string Item = "Item";
        public const string Data = "Data";
        public const string Admin = "Admin";
        public const string Client = "Client";
        public const string ClientAppointmentMaking = "ClientAppointmentMaking";
        public const string CompletionStatus = "CompletionStatus";
        public const string ClientNotes = "ClientNotes";
        public const string ClientProcedure = "ClientProcedure";

        public const string ReceiptVoucher = "ReceiptVoucher";
        public const string PaymentVoucher = "PaymentVoucher";
        public const string Treasury = "Treasury";
        public const string BranchTreasury = "BranchTreasury";
        public const string PaymentGatway = "PaymentGatway ";
        public const string ClientBranch = "ClientBranch";
        public const string FinancialYear = "FinancialYear";
        public const string CanNotAddFinancialYear = "CanNotAddFinancialYear";
        public const string CanNotAddFinancialYearAsThereIsActiveOne = "CanNotAddFinancialYearAsThereIsActiveOne";
        public const string CanNotAddFinancialYearAsThereIsDateNotConventioned = "CanNotAddFinancialYearAsThereIsDateNotConventioned";
        public const string CanNotActivateFinancialYearAsThereIsActiveOne = "CanNotActivateFinancialYearAsThereIsActiveOne";
        public const string ThisEmployeeAlreadyIsAssignedBefore = "ThisEmployeeAlreadyIsAssignedBefore";
        public const string CanNotRemoveThisBranchTreasuryAsThereIsAmount = "CanNotRemoveThisBranchTreasuryAsThereIsAmount";
        public const string CompanyIsRequired = "CompanyIsRequired";
        public const string FinancialYearIsRequired = "FinancialYearIsRequired";
        public const string BothOfCompanyAndFinancialYearRequired = "BothOfCompanyAndFinancialYearRequired";
        public const string BillBook = "BillBook";
        public const string CanNotAddBillBook = "CanNotAddBillBook";
        public const string FinancialYearIsNotActive = "FinancialYearIsNotActive";
        public const string CannotDeleteItemHasRelativeData = "CannotDeleteItemHasRelativeData";
        public const string ThereIs = "ThereIs";
        public const string NumOfObjectsNotEqualNumOfUploadedImages = "NumOfObjectsNotEqualNumOfUploadedImages";
        public const string IsNotSuperAdmin = "IsNotSuperAdmin";
        public const string PaymentGateway = "PaymentGateway";
        public const string PathRoute = "PathRoute";
        public const string ThisEmployeeIsNotTech = "ThisEmployeeIsNotTech";
        public const string Unit = "Unit";
        public const string UnitOfConversion = "UnitOfConversion";
        public const string RefusedPermission = "RefusedPermission";
        public const string ThisStockHasAlreadyTechnique = "ThisStockHasAlreadyTechnique";
        public const string ThisStockWithOutTechnique = "ThisStockWithOutTechnique";
        public const string Stock = "Stock";
        public const string NotFoundPhotos = "NotFoundPhotos";
        public const string ThereIsOnlineTech = "ThereIsOnlineTech";
        public const string NotValidDate = "NotValidDate";
        public const string StockTrans = "StockTrans";
        public const string InvalidDocNum = "InvalidDocNum";
        public const string StockTransType = "StockTransType";
        public const string ItemSerial = "ItemSerial";
        public const string ThereIsNotEnoughQuantityInTheStock = "ThereIsNotEnoughQuantityInTheStock";
        public const string ThereIsSomeItemsNotEnoughQuantityInTheStock = "ThereIsSomeItemsNotEnoughQuantityInTheStock";
        public const string AvailableAmount = "AvailableAmount";
        public const string RequiredAmount = "RequiredAmount";
        public const string CannotSendDatatoNoOne = "CannotSendDatatoNoOne";

        public const string ReqEarlyExit = "ReqEarlyExit";
        public const string ReqLateAttendance = "ReqLateAttendance";
        public const string ReqPermitExit = "ReqPermitExit";
        public const string ReqPermitFromHome = "ReqPermitFromHome";
        public const string ReqPermitFingerprint = "ReqPermitFingerprint";
        public const string ReqResign = "ReqResign";
        public const string ReqSalaryInc = "ReqSalaryInc";
        public const string PermitTrust = "PermitTrust";
        public const string ReqVacation = "ReqVacation";
        public const string ReqTrustCash = "ReqTrustCash";
        public const string ReqTransfer = "ReqTransfer";
        public const string ReqReward = "ReqReward";
        public const string ReqWorkDayCalc = "ReqWorkDayCalc";
        public const string ReqExtraTimeCalc = "ReqExtraTimeCalc";
        public const string ReqExitPermission = "ReqExitPermission";
        public const string ReqAddvance = "ReqAddvance";
        public const string ThisCannotBeDoneDueToThePresenceOf = "ThisCannotBeDoneDueToThePresenceOf";
    }

    public class LoggingIsSuccesss
    {
        public const string ErrorWhileDeleting = "Error while deleting for";
        public const string Id = "id: ";
        public const string Obj = "and obj: ";
    }

    public class Annotations
    {
        public const string FieldIsRequired = "The {0} is required";
        public const string AdjustmentDate = "AdjustmentDate";
        public const string Purchase = "Purchase";
        public const string Sale = "Sale";
        public const string FieldMustBeGreaterThanZero = "FieldMustBeGreaterThanZero";
        public const string Items = "Items";
        public const string NameAr = "Name in Arabic";
        public const string NameEn = "Name in English";
        public const string NameTr = "Name in Turkish";

        // Common labels used by DTOs (added to support DTO Display attributes)
        public const string Name = "Name";
        public const string Address = "Address";
        public const string Product = "Product";
        public const string Phone = "Phone";
        public const string Email = "Email";
        public const string Country = "Country";
        public const string Warehouse = "Warehouse";
        public const string Title = "Title";
        public const string Type = "Type";
        public const string Description = "Description";
        public const string ImagePath = "ImagePath";
        public const string Amount = "Amount";
        public const string UnitType = "UnitType";
        public const string Symbol = "Symbol";
        public const string ConfirmationPassword = "Confirmation password";
        public const string DepartmentName = "Department";
        public const string ConfirmationPasswordNotMatch = "Password and confirmation password must match.";
        public const string AttachmentsNotes = "Attachments notes.";

        // Product-specific labels
        public const string ProductCode = "ProductCode";
        public const string Category = "Category";
        public const string Supplier = "Supplier";
        public const string Brand = "Brand";
        public const string BranchId = "BranchId";
        public const string Unit = "Unit";
        public const string Tax = "Tax";
        public const string Price = "Price";
        public const string Cost = "Cost";
        public const string Discount = "Discount";
        public const string Quantity = "Quantity";
        public const string AlertQuantity = "AlertQuantity";
        public const string Barcode = "Barcode";
        public const string SKU = "SKU";
        public const string IsFeatured = "IsFeatured";
        public const string IsExpired = "IsExpired";
        public const string IsPromoSale = "IsPromoSale";
        public const string ExpiryDate = "ExpiryDate";
        public const string ManufactureDate = "ManufactureDate";
        public const string ColorVariantIds = "ColorVariantIds";
        public const string SizeVariantIds = "SizeVariantIds";

        // MaxLength helpers used in DTO attributes
        public const string MaxLengthIs200 = "The {0} must be at most 200 characters.";
        public const string MaxLengthIs100 = "The {0} must be at most 100 characters.";
        public const string MaxLengthIs50 = "The {0} must be at most 50 characters.";
        public const string MaxLengthIs500 = "The {0} must be at most 500 characters.";
        public const string MaxLengthIs300 = "The {0} must be at most 300 characters.";
        public const string MaxLengthIs10 = "The {0} must be at most 10 characters.";
        public const string MaxLengthIs1000 = "The {0} must be at most 1000 characters.";

        public const string AmountMustBeValid = "The {0} must be a valid amount.";

        public const string AttachmentsType = "Attachments type.";
        public const string BirthDate = "Birth date.";

        public const string NationalID = "National ID";
        public const string FieldIsEqual = "The {0} field length must be equal 14.";
        public const string ProfilePhoto = "Profile photo.";
        public const string Files = "Personal files.";
        public const string CourseMatrial = "Course matrial.";
        public const string CourseMatrialType = "Course matrial type.";
        public const string Password = "Password";
        public const string Code = "Code";
        public const string Company = "Company";
        public const string Job = "Job";
        public const string Gender = "Gender";
        public const string Region = "Region";
        public const string MaritalStatus = "MaritalStatus";
        public const string MilitaryStatus = "MilitaryStatus";

        public const string NameInArabic = "Name in Arabic";
        public const string NameInEnglish = "Name in English";
        public const string CompanyOwner = "Company owner";
        public const string PersonalEmail = "Personal Email";
        public const string PhoneNumber = "Phone number";
        public const string HireDate = "Hire date";
        public const string Task = "Task";
        public const string UserName = "User name";
        public const string UserNameOrEmail = "User name or email";

        public const string CourseAsset = "Course asset";
        public const string CourseAssetDescription = "Course asset description";
        public const string CourseAssetType = "Course asset type";
        public const string CourseAssetTypeDescription = "Course asset type description";

        public const string StartDate = "Start date";
        public const string EndDate = "End date";
        public const string RememberMe = "Remember me?";
    }
}
