using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Common.Dtos.ResponData;
using Dapper;
using MediatR;

namespace Application.Voucher.Queries.GetVoucherInactivePagination
{
    public class GetVoucherInactivePaginationQuery : IRequest<ResponDataDto>
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
    }
    public class GetVoucherInactivePaginationHandler(IDbConnection dbConnection) : IRequestHandler<GetVoucherInactivePaginationQuery, ResponDataDto>
    {
        public async Task<ResponDataDto> Handle(GetVoucherInactivePaginationQuery request, CancellationToken cancellationToken)
        {
            var voucherData = await dbConnection.QueryMultipleAsync
            (
                "sp_get_voucher_inactive_pagination",
                new
                {
                    page_number = request.page_number,
                    page_size = request.page_size
                }, commandType: CommandType.StoredProcedure
            );

            return new ResponDataDto
            {
                data = voucherData.Read<VoucherDto>(),
                page_number = request.page_number,
                page_size = request.page_size,
                total_record = voucherData.Read<int>().FirstOrDefault()
            };
            
        }
    }
}