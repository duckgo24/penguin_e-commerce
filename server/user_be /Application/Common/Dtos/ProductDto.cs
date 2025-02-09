using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; } = null!;
        public string product_desc { get; set; } = null!;
        public bool status { get; set; }
        public string booth_id { get; set; } = null!;
        public string updated_by { get; set; } = null!;
        public DateTime created_at { get; set; }
        public DateTime last_updated { get; set; }
        public bool is_deleted { get; set; }
        public List<ProductDetailDto> list_product_detail { get; set; } = new List<ProductDetailDto>();
    }
}