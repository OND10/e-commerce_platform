using System.ComponentModel.DataAnnotations;

namespace Service.Coupons.Api.Model.DTOs
{
    public class CouponResponseDTO
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; } = string.Empty;

        public decimal DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        public string StripeCouponId { get; set; }


        public CouponResponseDTO FromModel(Coupon model)
        {
            return new CouponResponseDTO
            {
                CouponId = model.CouponId,
                CouponCode = model.CouponCode,
                DiscountAmount = model.DiscountAmount,
                MinAmount = model.MinAmount,
                StripeCouponId = model.StripeCouponId
            };
        }

        public List<CouponResponseDTO> FromModel(IEnumerable<Coupon> model)
        {

            List<CouponResponseDTO> responses = new List<CouponResponseDTO>();
            foreach (var item in model)
            {
                var res = new CouponResponseDTO
                {
                    CouponId = item.CouponId,
                    CouponCode = item.CouponCode,
                    DiscountAmount = item.DiscountAmount,
                    MinAmount = item.MinAmount,
                    StripeCouponId = item.StripeCouponId
                };
                responses.Add(res);
            }
            var list = new List<CouponResponseDTO>();
            list.AddRange(model.Select((x) => FromModel(x)));
            return list;

        }
    }
}
