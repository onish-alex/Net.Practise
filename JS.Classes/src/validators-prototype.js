function Validator() {}

Validator.prototype.IsValid = function(str) {
    console.log("Не реализован")
}


function EmailValidator() {}

EmailValidator.prototype.__proto__ = Validator.prototype;

EmailValidator.prototype.IsValid = function(str) {
    var regex = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    return(regex.test(str) 
        ? "Корректный Email" 
        : "Некорректный Email")
}


function PhoneValidator() {}

PhoneValidator.prototype.__proto__ = Validator.prototype;

PhoneValidator.prototype.IsValid = function(str) {
    var regex = /^\d[\d\(\)\ -]{4,14}\d$/
    return(regex.test(str) 
        ? "Корректный номер" 
        : "Некорректный номер")
}

export let ValidatorsClass = {
    emailValidator : EmailValidator,
    phoneValidator : PhoneValidator
}
