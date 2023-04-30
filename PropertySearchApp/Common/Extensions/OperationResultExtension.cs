using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PropertySearchApp.Common.Constants;

namespace PropertySearchApp.Common.Extensions;

public static class OperationResultExtension
{
    public static IActionResult ToResponse(this OperationResult result, string successMessage, ITempDataDictionary tempData, Func<IActionResult> ifSuccess, Func<IActionResult> ifFaulted)
    {
        if(result.Succeeded)
        {
            tempData[Alerts.Success] = successMessage;
            return ifSuccess();
        }

        tempData[Alerts.Danger] = result.ErrorMessage;
        return ifFaulted();
    }
}
