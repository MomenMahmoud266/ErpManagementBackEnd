namespace ErpManagement.DataAccess.Repositories.Logging;

//public class LoggingRepository(ErpManagementDbContext context, ILoggerFactory loggerFactory, IHttpContextAccessor accessor) : ILoggingRepository
//{
//    private readonly IHttpContextAccessor _accessor = accessor;
//    private readonly ErpManagementDbContext _context = context;
//    private readonly ILogger _logger = loggerFactory.CreateLogger("db_logs");

//    public async Task<bool> LogExceptionInDb(Exception exception, string objJson = null!, LogType logType = LogType.Bug)
//    {
//        try
//        {
//            await _context.Logs.AddAsync(new ComLog()
//            {
//                IsSuccess = exception?.Message ?? string.Empty,
//                ExceptionPath = exception?.Source ?? string.Empty,
//                ExceptionInnerPath = exception?.InnerException?.Source ?? string.Empty,
//                InnerException = exception?.InnerException?.Message ?? string.Empty,
//                StackTrace = exception?.StackTrace ?? string.Empty,
//                ObjJson = objJson,
//                InsertBy = _accessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.NameId)?.Value ?? string.Empty
//            });
//            return await _context.SaveChangesAsync() > 0;
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e);
//            _logger.LogError(JsonConvert.SerializeObject(e));
//            throw;
//        }
//    }
//}
