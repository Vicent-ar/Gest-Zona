using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace proyecto_admin.Vistas
{
    class ValidaRango : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex rx = new Regex(@"^[ñÑ/^$|\s+/]+$");
            if (rx.IsMatch(value.ToString()))
                return new ValidationResult(false, "El valor no puede estar vacio");
            else
                return new ValidationResult(true, null);
        }
    }
    class ValidaDouble : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex rx = new Regex(@"-?\d+(?:\.\d+)?");
            if (rx.IsMatch(value.ToString()))
                return new ValidationResult(true,null);
            else
                return new ValidationResult(false, "Formato válido decimal. Si desea entero ponga el numero coma 0");
        }
    }
    class ValidacionNumbers : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex rx = new Regex(@"^[0-9]+$");
            if (!rx.IsMatch(value.ToString()))
                return new ValidationResult(false, "Los caracteres solo \npueden ser enteros");
            else
                return new ValidationResult(true, null);
        }
    }
    class ValidaLetras : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex rx = new Regex(@"^[0-9]+$");
            if (rx.IsMatch(value.ToString()))
                return new ValidationResult(false, "Los caracteres solo \npueden ser letras");
            else
                return new ValidationResult(true, null);
        }
    }
    class ValidacionLenght : ValidationRule
    {
        public ValidacionLenght() { }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value.ToString().Length >= 5)
                return new ValidationResult(true, null);
            else
                return new ValidationResult(false, "El nombre tiene que tener al menos 5 caracteres");
        }
    }
    class ValidacionIban : ValidationRule
    {


        public ValidacionIban() { }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex rx = new Regex(@"^[A-Z]{2}([0-9]){22}");
            if (!rx.IsMatch(value.ToString()))

                return new ValidationResult(false, "El IBAN se compone de ES \ny ventidos cifras.");

            else

                return new ValidationResult(true, null);
        }
    }
    class ValidacionDni : ValidationRule
    {


        public ValidacionDni() { }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex rx = new Regex(@"^(\d{8})([A-Z])+$");
            if (!rx.IsMatch(value.ToString()))

                return new ValidationResult(false, "El DNI se compone de 8 cifras \ny una letra en mayúscula.");

            else

                return new ValidationResult(true, null);
        }
    }
    class ValidacionEmail : ValidationRule
    {
        public ValidacionEmail() { }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex rx = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
            if (!rx.IsMatch(value.ToString()))

                return new ValidationResult(false, "Debes introducir una dirección de correo Valida");

            else

                return new ValidationResult(true, null);

        }

    }
    class ValidacionTel : ValidationRule
    {
        public ValidacionTel() { }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value.ToString().Length == 9)

                return new ValidationResult(true, null);

            else

                return new ValidationResult(false, "El teléfono debe tener 9 números");

        }

    }
}
