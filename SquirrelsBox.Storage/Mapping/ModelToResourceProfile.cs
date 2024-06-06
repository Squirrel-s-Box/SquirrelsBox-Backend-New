using AutoMapper;
using Base.Generic.Domain.Services.Communication;
using Base.Generic.Resources;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Resources;

namespace SquirrelsBox.Storage.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Box, ReadBoxResource>();

            CreateMap<BoxSectionRelationship, ReadBoxSectionRelationshipResource>();
            CreateMap<Section, ReadSectionResource>();

            CreateMap<SectionItemRelationship, ReadSectionItemRelationshipResource>();
            CreateMap<Item, ReadItemResource>();

            CreateMap<Spec, ReadSpecResource>();


            //Validation Resource
            CreateMap(typeof(BaseResponse<>), typeof(ValidationResource));
        }
    }
}
