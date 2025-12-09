namespace ErpManagement.Services.Services.WebSocket;

public class NotificationService : INotificationService
{
    //private readonly IUnitOfWork _unitOfWork;
    //private readonly ILoggingRepository _loggingRepository;
    //private readonly IHttpContextAccessor _accessor;
    //private readonly IStringLocalizer<SharedResource> _sharLocalizer;

    //public NotificationService(IUnitOfWork unitOfWork, ILoggingRepository loggingRepository,
    //                           IHttpContextAccessor accessor,
    //                           IStringLocalizer<SharedResource> sharLocalizer)
    //{
    //    _unitOfWork = unitOfWork;
    //    _loggingRepository = loggingRepository;
    //    _accessor = accessor;
    //    _sharLocalizer = sharLocalizer;
    //}

    //#region Notification

    //public async Task<Response<string>> GetNotificationCountAsync()
    //{
    //    int employeeId = GetUserById().Result.Employee_Id;

    //    int notificationsCount = await _unitOfWork.Notifications.CountAsync(x => x.Receiver_Id == employeeId && x.Sender_Id != x.Receiver_Id && !x.IsSeen); 

    //    return new Response<string>()
    //    {
    //        IsSuccess = true,
    //        Data = notificationsCount.ToString()
    //    };
    //}

    //public async Task<Response<SharNotificationResultResponse>> GetNotificationMessageAsync(PaginationRequest model)
    //{
    //    var listOfNotificationsMessage = new List<SharNotificationResultResponse>();
    //    using var transaction = _unitOfWork.BeginTransaction();
    //    try
    //    {
    //        int empId = GetUserById().Result.Employee_Id;

    //        var result = new SharNotificationResultResponse
    //        {
    //            TotalRecords = await _unitOfWork.Notifications.CountAsync(x => x.Receiver_Id == empId && x.Sender_Id != empId),

    //            Data = (await _unitOfWork.Notifications.GetSpecificSelectAsync(x => x.Receiver_Id == empId && x.Sender_Id != empId,
    //                 take: model.PageSize,
    //                 skip: (model.PageNumber - 1) * model.PageSize,
    //            select: x => new NotificationData
    //            {
    //                Id = x.Id,
    //                Type = x.IsMentioned == true ? Noti.Mention
    //                : x.IsMentioned == false ? Noti.TaskComment
    //                : x.IsMentioned == null && x.Client_Id != null ? Noti.ClientComment
    //                : Noti.Task,

    //                Description = x.Body,
    //                DateTime = x.InsertDate.ToGetFullyDate(),
    //                IsRead = x.IsRead,

    //                CompanyId = x.Department_Id != null
    //                ? x.Department.Company_Id
    //                : x.Task_Id != null && x.Task.Project != null 
    //                ? x.Task.Project.Department.Company_Id
    //                : x.Task_Id != null && x.Task.Project == null 
    //                ? x.Task.Department.Company_Id
    //                : x.TaskComment.Task.Department.Company_Id,

    //                DepartmentId = x.Department_Id != null
    //                ? x.Department_Id
    //                : x.Task_Id != null && x.Task.Project != null 
    //                ? x.Task.Project.Department_Id
    //                : x.Task_Id != null && x.Task.Project == null ? x.Task.Department_Id
    //                : x.TaskComment.Task.Department_Id,

    //                ProjectId = x.Task.Project_Id ?? 0,
    //                TaskId = x.Task_Id != null ? x.Task_Id : null
    //            }, orderBy: x =>
    //                   x.OrderByDescending(x => x.InsertDate))).ToList()
    //        };
    //        if (result.TotalRecords is 0)
    //        {
    //            string resultMsg = _sharLocalizer[Localization.NotFoundData];

    //            return new Response<SharNotificationResultResponse>()
    //            {
    //                Data = new SharNotificationResultResponse()
    //                {
    //                    Data = new List<NotificationData>()
    //                },
    //                Error = resultMsg,
    //                Message = resultMsg
    //            };
    //        }

    //        var sss = (await _unitOfWork.Notifications.GetAllAsync(y => result.Data.Select(v => v.Id).Contains(y.Id))).ToList();

    //        sss.ForEach(d => d.IsSeen = true);

    //        _unitOfWork.Notifications.UpdateRange(sss);

    //        bool isUpdated = await _unitOfWork.CompleteAsync() > 0;

    //        transaction.Commit();
    //        return new Response<SharNotificationResultResponse>()
    //        {
    //            IsSuccess = isUpdated,
    //            Data = result

    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        transaction.Rollback();
    //        await _loggingRepository.LogExceptionInDb(ex, string.Empty);
    //        return new Response<SharNotificationResultResponse>()
    //        {
    //            Error = ex.Message,
    //            Message = ex.Message + (ex.InnerException == null ? string.Empty : ex.InnerException.Message)
    //        };
    //    }
    //}

    //public async Task<Response<string>> UpdateNotificationOfReadingByIdAsync(int id)
    //{
    //    var obj = await _unitOfWork.Notifications.GetByIdAsync(id);

    //    if (obj == null)
    //    {
    //        string resultMsg = string.Format(_sharLocalizer[Localization.CannotBeFound],
    //            _sharLocalizer[Localization.Notification]);

    //        return new Response<string>()
    //        {
    //            Data = id.ToString(),
    //            Error = resultMsg,
    //            Message = resultMsg
    //        };
    //    }

    //    string err = _sharLocalizer[Localization.Error];
    //    try
    //    {
    //        obj.IsRead = true;
    //        _unitOfWork.Notifications.Update(obj);
    //        bool result = await _unitOfWork.CompleteAsync() > 0;

    //        return new Response<string>()
    //        {
    //            IsSuccess = result,
    //            Data = id.ToString(),
    //            Message = result ? _sharLocalizer[Localization.Updated] : _sharLocalizer[err]
    //        };

    //    }
    //    catch (Exception ex)
    //    {
    //        await _loggingRepository.LogExceptionInDb(ex, JsonConvert.SerializeObject(id));
    //        return new Response<string>()
    //        {
    //            Error = err,
    //            Message = ex.Message + (ex.InnerException == null ? string.Empty : ex.InnerException.Message)
    //        };
    //    }
    //}


    //private string UserId() =>
    //    _accessor!.HttpContext == null ? string.Empty : _accessor!.HttpContext!.User.GetUserId();
    //private async Task<ApplicationUser> GetUserById() =>
    //    await _unitOfWork.Users.GetFirstOrDefaultAsync(x => x.Id == UserId());

    //#endregion
}
