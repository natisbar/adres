import {AbstractControl, ValidationErrors, ValidatorFn} from '@angular/forms';

export function regularCharacterValidator(): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null => {

        const value = control.value;

        if (!value) {
            return null;
        }

        const stringValid = /[^A-Za-zá-úÁ-Ú\s\u00f1\u00d1]/.test(value);

        return stringValid ? {stringValid:true}: null;
    }
}

export function regularCharacterWithSymbolValidator(): ValidatorFn {
  return (control:AbstractControl) : ValidationErrors | null => {

      const value = control.value;

      if (!value) {
          return null;
      }

      const stringWithSymbolValid = /[^A-Za-zá-úÁ-Ú\s\u00f1\u00d1.,-]/.test(value);

      return stringWithSymbolValid ? {stringWithSymbolValid:true}: null;
  }
}


export function moneyAmountValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null;
    }

    const isValid = /^[0-9]{1,50}$/.test(value);

    return !isValid ? { invalidAmount: true } : null;
  };

}

export function decimalValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null; // Si no hay valor, no hay error.
    }

    try {
      // Intentar convertir el valor a número
      const numberValue = parseFloat(value);

      // Validar si el valor convertido es un número válido
      if (isNaN(numberValue) || numberValue <= 0) {
        return { invalidDecimal: true }; // Si no es un número o es negativo
      }

      return null; // Si el valor es un número válido
    } catch (e) {
      return { invalidDecimal: true }; // En caso de error, es un valor no válido
    }
  };
}


