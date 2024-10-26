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
