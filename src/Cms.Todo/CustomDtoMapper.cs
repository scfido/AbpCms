using AutoMapper;
using Cms.Todo.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cms.Todo
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<TodoDto, Core.Todo>();
        }
    }
}
