using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PropertySearchApp.Common.Exceptions.Abstract;
using PropertySearchApp.Common.Exceptions;

namespace PropertySearchApp.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToResponse(this Result<bool> result, string successMessage, ITempDataDictionary tempData, Func<IActionResult> ifSuccess, Func<IActionResult> ifFaulted, Action<Exception, string> errorLogger)
    {
        return result.Match<IActionResult>(success =>
        {
            tempData["alert-success"] = successMessage;
            return ifSuccess();
        }, exception =>
        {
            if (exception is Common.Exceptions.Abstract.HandledApplicationException appException)
            {
                tempData["alert-danger"] = appException.BuildExceptionMessage();
                return ifFaulted();
            }
            else if (exception is InternalDatabaseException dbException)
            {
                tempData["alert-danger"] = "Operation failed. Try again later";
                foreach (var error in dbException.Errors)
                {
                    errorLogger?.Invoke(exception, error);
                }
                return ifFaulted();
            }

            errorLogger?.Invoke(exception, "Unhandled exception in create accommodation operation");
            throw exception;
        });
    }
}