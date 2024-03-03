﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("### Executando -> OnActionExecuted");
        _logger.LogInformation("################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"StatusCode : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("################################################");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("### Executando -> OnActionExecuting");
        _logger.LogInformation("################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
        _logger.LogInformation("################################################");
    }
}
