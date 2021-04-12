import { ValidatorsClass } from './validators-class.js'
import { ShapesClass } from './figures-class.js'

document.getElementById("email").addEventListener("click", function() {
    let emailStr = document.getElementById("email-input").value;
    let validator = new ValidatorsClass.emailValidator();
    let result = validator.IsValid(emailStr);
    document.getElementById("email-result").innerHTML = result;
});

document.getElementById("phone").addEventListener("click", function() {
    let phoneStr = document.getElementById("phone-input").value;
    let validator = new ValidatorsClass.phoneValidator();
    let result = validator.IsValid(phoneStr);
    document.getElementById("phone-result").innerHTML = result;
});

document.getElementById("circle").addEventListener("click", function() {
    let radius = document.getElementById("circle-input").value;
    let circle = new ShapesClass.circle(radius);
    let result = circle.getArea();
    document.getElementById("circle-result").innerHTML = result;
});

document.getElementById("square").addEventListener("click", function() {
    let side = document.getElementById("square-input").value;
    let square = new ShapesClass.square(side);
    let result = square.getArea();
    document.getElementById("square-result").innerHTML = result;
});