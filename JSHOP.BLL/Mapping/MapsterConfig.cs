using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using JSHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace JSHOP.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
           TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.Name, src => src.Translations.Where(t=>t.Language == CultureInfo.CurrentCulture.Name).Select(t=>t.Name
                ).FirstOrDefault());


        }
    }
}
