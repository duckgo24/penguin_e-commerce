using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Interface;
using MediatR;
using WebApi.DBHelper;

namespace Application.MyBooth .Commands.UnBan
{
    public class UnBanBoothCommand : IRequest<BoothDto>
    {
        public string booth_id { get; set; } = null!;
    }
    public class UnBanBoothHandler(IDbHelper dbHelper) : IRequestHandler<UnBanBoothCommand, BoothDto>
    {
        public async Task<BoothDto> Handle(UnBanBoothCommand request, CancellationToken cancellationToken)
        {
            var data = await dbHelper.ExecuteUpdateProduceByUserAsync<BoothDto>
            (
                "sp_unban_booth_by_id",
                new
                {
                    booth_id = request.booth_id,
                }
            );

            return data;
        }
    }
}