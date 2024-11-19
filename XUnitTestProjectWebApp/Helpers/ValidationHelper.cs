//using FluentValidation;
//using FluentValidation.Results;
//using XUnitTestProjectWebApp.Models;

//namespace XUnitTestProjectWebApp.Helpers

//{

//    public static class ValidationHelper
//    {
//        public static void KosulSaglanirsaHataEkle(bool condition, string errorMessage, string propertyName, ValidationContext<Product> context)
//        {
//            if (condition)
//            {
//                context.AddFailure(new ValidationFailure(propertyName, errorMessage));
//            }
//        }
//    }
//}

using FluentValidation;
using FluentValidation.Results;
using XUnitTestProjectWebApp.Models;
using System;

namespace XUnitTestProjectWebApp.Helpers
{
    public static class ValidationHelper
    {
        public static void HataFirlatVeEkle(Func<bool> condition, string errorMessage, string propertyName, ValidationContext<Product> context)
        {
            try
            {
                // Şart sağlanıyorsa throw fırlatmak için if'e gir
                if (condition())
                {
                    throw new ValidationException(errorMessage);
                }
            }
            catch (ValidationException ex)
            {
                // Hata durumunda hatayı context üzerine ekleyerek tüm kuralların devam etmesini sağla
                context.AddFailure(new ValidationFailure(propertyName, ex.Message));
            }
        }
    }
}
