using FluentValidation;

namespace XUnitTestProjectWebApp.Models
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()

        {

            RuleFor(p => p.ProductName).NotEmpty().WithMessage("Ürün İsmi Boş Geçilemez")
                .Length(2, 50).WithMessage("Ürün İsmi 2-50 karakter arasında olması gerekiyor");
            RuleFor(p => p.ProductPrice).GreaterThan(0).WithMessage("Ürünün Fiyatı 0 dan büyük olmak zorundadır");
            RuleFor(p => p.ProductStock).GreaterThanOrEqualTo(0).WithMessage("Ürünün Stock Adedi 0'a eşit veya büyük olmak zorundadır");
            RuleFor(p => p.CategoryId).NotNull().WithMessage("Ürüne Ait kategori Id alanı boş geçilmez");
        }
    }
}
