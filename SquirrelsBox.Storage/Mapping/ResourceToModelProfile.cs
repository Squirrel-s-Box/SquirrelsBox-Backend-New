using AutoMapper;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Resources;

namespace SquirrelsBox.Storage.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<ReadBoxResource, Box>();
            CreateMap<SaveBoxResource, Box>();
            CreateMap<UpdateBoxResource, Box>();

            CreateMap<SaveBoxSectionsListResource, BoxSectionRelationship>();
            CreateMap<SaveSectionResource, Section>();
            CreateMap<UpdateBoxSectionsListResource, BoxSectionRelationship>();
            CreateMap<UpdateSectionResource, Section>();

            CreateMap<SaveSectionItemResource, SectionItemRelationship>();
            CreateMap<SaveItemResource, Item>();
            CreateMap<UpdateSectionItemListResource, SectionItemRelationship>();
            CreateMap<UpdateItemResource, Item>();

            //CreateMap<SaveItemSpecListResource, ItemSpecRelationship>();
            CreateMap<SaveSpecResource, Spec>();
            //CreateMap<UpdateItemSpecListResource, ItemSpecRelationship>();
            CreateMap<UpdateSpecResource, Spec>();
        }
    }
}
