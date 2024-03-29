﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PropertySearch.Api.Common.Constants;

namespace PropertySearch.Api.Common.Extensions;

public static class OperationResultExtensions
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
