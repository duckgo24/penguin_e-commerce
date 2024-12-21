using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Common.Dtos.ResponData;
using Application.Dtos;
using Azure;
using Dapper;
using MediatR;
using WebApi.DBHelper;

namespace Application.Category.Queries.GetCategoriesWithPagination
{
    public class GetCategoryPaginationQuery : IRequest<ResponDataDto>
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
    }

    public class GetCategoryPaginationHandler(IDbConnection dbConnection) : IRequestHandler<GetCategoryPaginationQuery, ResponDataDto>
    {
        public async Task<ResponDataDto> Handle(GetCategoryPaginationQuery request, CancellationToken cancellationToken)
        {
        
            var categoryData = await dbConnection.QueryMultipleAsync
            (
                "sp_get_categories_pagination",
                new
                {
                    page_number = request.page_number,
                    page_size = request.page_size
                }, 
                commandType: CommandType.StoredProcedure
            );

            Dictionary<string, CategoryDto> cgDic = new Dictionary<string, CategoryDto>();
            var category = categoryData.Read<CategoryDto, CategoryDetailDto, CategoryDto>
            (
                (cg, cgd) => {
                    if(!cgDic.TryGetValue(cg.Id, out var cgEntry))
                    {
                        cgEntry = cg;
                        cgEntry.list_category_detail = new List<CategoryDetailDto>();
                        cgDic.Add(cgEntry.Id, cgEntry);
                    }


                    if(cgd.category_id != null)
                    {
                        cgEntry.list_category_detail.Add(cgd);
                    }

                    return cgEntry;
                },
                splitOn: "category_id"
            );


            return new ResponDataDto()
            {
                data = cgDic.Values.ToList(),
                page_number = request.page_number,
                page_size = request.page_size,
                total_record = categoryData.ReadFirst<int>()
            };
        }


    }

}