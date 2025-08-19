using System;
using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Book, Book>();
        CreateMap<CreateBookDTO, Book>();
    }
}
