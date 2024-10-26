namespace XUnitTestProjectWebApp.Models
{
    using FluentValidation;


    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            // Ürün ismi kuralları
            RuleFor(p => p.ProductName)
                .Must(name => !string.IsNullOrEmpty(name))
                .WithMessage("Ürün İsmi Boş Geçilemez")
                .Must(name => name.Length >= 2 && name.Length <= 50)
                .WithMessage("Ürün İsmi 2-50 karakter arasında olması gerekiyor");

            // Ürün fiyatı kuralları
            RuleFor(product => product.ProductPrice)
                .Must(price => price > 0)
                .WithMessage("Ürün fiyatı sıfırdan büyük olmalıdır.");

            // Ürün stoğu kuralları
            RuleFor(p => p.ProductStock)
                .Must(stock => stock >= 0)
                .WithMessage("Ürünün Stok Adedi 0'a eşit veya büyük olmak zorundadır");

            // Kategori Id kuralları

        }

    }
}


/* Throw kullanarak yapıldığında **************************************************************  
using FluentValidation;

namespace XUnitTestProjectWebApp.Models
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Ürün İsmi Boş Geçilemez")
                .Length(2, 50).WithMessage("Ürün İsmi 2-50 karakter arasında olması gerekiyor")
                .Must((product, productName) =>
                {
                    if (string.IsNullOrWhiteSpace(productName))
                    {
                        throw new ValidationException("Ürün ismi geçersizdir.");
                    }
                    return true;
                }).WithMessage("Ürün ismi doğrulanamadı.");

            RuleFor(product => product.ProductPrice)
                .GreaterThan(0).WithMessage("Ürün fiyatı sıfırdan büyük olmalıdır.")
                .Must((product, price) =>
                {
                    if (price <= 0)
                    {
                        throw new ValidationException("Ürün fiyatı sıfırdan büyük olmalıdır.");
                    }
                    return true;
                }).WithMessage("Ürün fiyatı geçersizdir.");

            RuleFor(p => p.ProductStock)
                .GreaterThanOrEqualTo(0).WithMessage("Ürünün Stock Adedi 0'a eşit veya büyük olmak zorundadır")
                .Must((product, stock) =>
                {
                    if (stock < 0)
                    {
                        throw new ValidationException("Ürün stoğu negatif olamaz.");
                    }
                    return true;
                }).WithMessage("Ürün stoğu geçersizdir.");

        }
    }
}
*/
//using FluentValidation;

//namespace XUnitTestProjectWebApp.Models
//{
//    public class ProductValidator : AbstractValidator<Product>
//    {
//        public ProductValidator()
//        {
//            RuleFor(p => p.ProductName)
//                .NotEmpty().WithMessage("Ürün İsmi Boş Geçilemez")
//                .Length(2, 50).WithMessage("Ürün İsmi 2-50 karakter arasında olması gerekiyor")
//                .Must((product, productName) =>
//                {
//                    return !string.IsNullOrWhiteSpace(productName);
//                }).WithMessage("Ürün ismi geçersizdir.");

//            RuleFor(product => product.ProductPrice)
//                .GreaterThan(0).WithMessage("Ürün fiyatı sıfırdan büyük olmalıdır.")
//                .Must((product, price) =>
//                {
//                    return price > 0;
//                }).WithMessage("Ürün fiyatı geçersizdir.");

//            RuleFor(p => p.ProductStock)
//                .GreaterThanOrEqualTo(0).WithMessage("Ürünün Stock Adedi 0'a eşit veya büyük olmak zorundadır")
//                .Must((product, stock) =>
//                {
//                    return stock >= 0;
//                }).WithMessage("Ürün stoğu geçersizdir.");
//        }
//    }
//}
