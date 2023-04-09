﻿using PropertySearchApp.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertySearchApp.Entities;

[Table("Contact")]
public class ContactEntity : IEntity
{
    public Guid Id { get; set; }
    public string ContactType { get; set; }
    public string Content { get; set; }
    public DateTime CreationTime { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
