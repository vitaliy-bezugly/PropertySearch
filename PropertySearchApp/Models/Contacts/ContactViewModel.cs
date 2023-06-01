﻿using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Models.Contacts;

public class ContactViewModel : IMapFrom<ContactDomain>
{
    public Guid Id { get; set; }
    public string ContactType { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;
}
