﻿using System.ComponentModel.DataAnnotations;
using AutoMapper;
using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Models.Location;

namespace PropertySearch.Api.Models.Accommodations;

public class AccommodationViewModel : IMapFrom<AccommodationDomain>
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = String.Empty;
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    [DataType(DataType.Url)]
    public string? PhotoUri { get; set; }
    [Required]
    public string OwnerId { get; set; } = String.Empty;
    public string? OwnerUsername { get; set; }
    public LocationViewModel Location { get; set; } = new();
    public DateTime CreationTime { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<AccommodationDomain, AccommodationViewModel>()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.UserId));
    }
}