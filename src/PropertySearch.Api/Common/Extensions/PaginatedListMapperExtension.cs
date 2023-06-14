using AutoMapper;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Models.Accommodations;

namespace PropertySearch.Api.Common.Extensions;

public static class PaginatedListMapperExtension
{
    public static PaginatedList<AccommodationViewModel> ToPaginatedListViewModel(this IMapper mapper, PaginatedList<AccommodationDomain> paginatedList)
    {
        var viewModels = paginatedList.Items.Select(mapper.Map<AccommodationViewModel>).ToList();
        var paginatedListViewModel = new PaginatedList<AccommodationViewModel>(viewModels, paginatedList.PageNumber, paginatedList.TotalPages, paginatedList.TotalCount);
        return paginatedListViewModel;
    }
}