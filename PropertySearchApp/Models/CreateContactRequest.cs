﻿using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models;

public class CreateContactRequest
{
    [Required]
    public string ContactType { get; set; }
    [Required]
    public string Content { get; set; }
}
