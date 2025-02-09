using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Application.Common.Dtos;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using WebApi.DBHelper;

namespace Application.Voucher.Commands.Create
{
    public class CreateVoucherCommand : IRequest<VoucherDto>
    {
        public string voucher_type { get; set; } = null!;
        public string voucher_name { get; set; } = null!;
        public string apply_for { get; set; } = null!;
        public int after_expiry_date { get; set; }
        public string option_expiry_date { get; set; } = null!;
        public int quantity_remain { get; set; }
        public Double discount { get; set; }
        public string type_discount { get; set; } = null!;
    }
    public class CreateVoucherHandler(IDbHelper dbHelper) : IRequestHandler<CreateVoucherCommand, VoucherDto>
    {
        public async Task<VoucherDto> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            DateTime _expriy_date = DateTime.UtcNow;
            if (request.option_expiry_date == "minute")
            {
                _expriy_date = DateTime.UtcNow.AddMinutes(request.after_expiry_date);
            }
            if (request.option_expiry_date == "hour")
            {
                _expriy_date = DateTime.UtcNow.AddHours(request.after_expiry_date);
            }
            else if (request.option_expiry_date == "day")
            {
                _expriy_date = DateTime.UtcNow.AddDays(request.after_expiry_date);
            }
            else if(request.option_expiry_date == "week")
            {
                _expriy_date = DateTime.UtcNow.AddDays(request.after_expiry_date * 7);
            }
            else if(request.option_expiry_date == "month")
            {
                _expriy_date = DateTime.UtcNow.AddMonths(request.after_expiry_date);
            }
            else if(request.option_expiry_date == "year")
            {
                _expriy_date = DateTime.UtcNow.AddYears(request.after_expiry_date);
            }

            var data = await dbHelper.QueryProceduceByUserAsync<VoucherDto>(
                "sp_create_voucher",
                new
                {
                    voucher_id = Guid.NewGuid().ToString(),
                    voucher_type = request.voucher_type,
                    voucher_name = request.voucher_name,
                    voucher_code = new string(Enumerable.Repeat(_chars, 10).Select(s => s[new Random().Next(s.Length)]).ToArray()),
                    apply_for = request.apply_for,
                    expiry_date = _expriy_date,
                    quantity_remain = request.quantity_remain,
                    quantity_used = 0,
                    discount = request.discount,
                    type_discount = request.type_discount,
                    status_voucher = StatusVoucher.Inactive,
                    is_deleted = false
                }
            );
            return data;
        }
    }
}