//using FluentValidation;
//using XUnitTestProjectWebApp.Helpers;

//namespace XUnitTestProjectWebApp.Models
//{
//    public class ProductValidator : AbstractValidator<Product>
//    {
//        public ProductValidator()
//        {
//            RuleFor(p => p.ProductName)
//                .NotEmpty().WithMessage("Ürün İsmi Boş Geçilemez")
//                .Length(2, 50).WithMessage("Ürün İsmi 2-50 karakter arasında olması gerekiyor")
//                .Custom((productName, context) =>
//                    ValidationHelper.KosulSaglanirsaHataEkle(string.IsNullOrWhiteSpace(productName),
//                    "Ürün ismi geçersizdir.", nameof(Product.ProductName), context));

//            RuleFor(product => product.ProductPrice)
//                .GreaterThan(0).WithMessage("Ürün fiyatı sıfırdan büyük olmalıdır.")
//                .Custom((price, context) =>
//                    ValidationHelper.KosulSaglanirsaHataEkle(price <= 0,
//                    "Ürün fiyatı sıfırdan büyük olmalıdır.", nameof(Product.ProductPrice), context));

//            RuleFor(p => p.ProductStock)
//                .GreaterThanOrEqualTo(0).WithMessage("Ürünün Stock Adedi 0'a eşit veya büyük olmak zorundadır")
//                .Custom((stock, context) =>
//                    ValidationHelper.KosulSaglanirsaHataEkle(stock < 0,
//                    "Ürün stoğu negatif olamaz.", nameof(Product.ProductStock), context));
//        }


//    }
//}
using FluentValidation;
using XUnitTestProjectWebApp.Helpers;

namespace XUnitTestProjectWebApp.Models
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Ürün İsmi Boş Geçilemez")
                .Length(2, 50).WithMessage("Ürün İsmi 2-50 karakter arasında olması gerekiyor")
                .Custom((productName, context) => ValidationHelper.HataFirlatVeEkle(() =>
                    string.IsNullOrWhiteSpace(productName),
                    "Ürün ismi geçersizdir.", nameof(Product.ProductName), context));

            RuleFor(product => product.ProductPrice)
                .GreaterThan(0).WithMessage("Ürün fiyatı sıfırdan büyük olmalıdır.")
                .Custom((price, context) => ValidationHelper.HataFirlatVeEkle(() =>
                    price <= 0,
                    "Ürün fiyatı sıfırdan büyük olmalıdır.", nameof(Product.ProductPrice), context));

            RuleFor(p => p.ProductStock)
                .GreaterThanOrEqualTo(0).WithMessage("Ürünün Stok Adedi 0'a eşit veya büyük olmak zorundadır")
                .Custom((stock, context) => ValidationHelper.HataFirlatVeEkle(() =>
                    stock < 0,
                    "Ürün stoğu negatif olamaz.", nameof(Product.ProductStock), context));
        }
    }
}
