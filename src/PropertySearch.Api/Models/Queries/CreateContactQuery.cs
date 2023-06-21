using Microsoft.AspNetCore.Mvc;

namespace PropertySearch.Api.Models.Queries;

public class CreateContactQuery
{
    [FromQuery(Name = "type")] 
    public string Type { get; set; } = string.Empty;
    [FromQuery(Name = "content")] 
    public string Content { get; set; } = string.Empty;
}