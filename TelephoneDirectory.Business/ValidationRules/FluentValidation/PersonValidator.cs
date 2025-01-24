using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TelephoneDirectory.Entities.Concrete;

namespace TelephoneDirectory.Business.ValidationRules.FluentValidation
{
    public class PersonValidator:AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(p => p.Name).MinimumLength(3).WithMessage("İsim uzunluğu en az üç karakter olmalı");
            RuleFor(p => p.SurName).NotEmpty().WithMessage("Soyisim alanı boş olamaz");
            RuleFor(p => p.SurName).MinimumLength(2).WithMessage("Soyisim uzunluğu en az iki karakter olmalı");
            RuleFor(p => p.PhoneNumber).NotEmpty().WithMessage("Telefon numarası alanı boş geçilemez");
            RuleFor(p => p.PhoneNumber).Matches(@"^\d+$").WithMessage("Telefon numarası yalnızca rakamlardan oluşmalıdır.");

        }

     
    }
}
