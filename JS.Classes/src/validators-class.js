class Validator {
    IsValid(str) {
        console.log("Не реализован")
    }
}

class EmailValidator extends Validator {
    IsValid(str) {
        var regex = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
        return (regex.test(str) 
            ? "Корректный Email" 
            : "Некорректный Email")
    }
}

class PhoneValidator extends Validator {
    IsValid(str) {
        var regex = /^\d[\d\(\)\ -]{4,14}\d$/
        return (regex.test(str) 
            ? "Корректный номер" 
            : "Некорректный номер")
    }
}

export let ValidatorsClass = {
    emailValidator : EmailValidator,
    phoneValidator : PhoneValidator
}

