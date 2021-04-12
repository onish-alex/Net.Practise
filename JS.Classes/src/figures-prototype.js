function Shape() {}

Shape.prototype.getArea = function() {}

function Square(side) {
    this.side = side        
}

Square.prototype.__proto__ = Shape.prototype;

Square.prototype.getArea = function() {
    return this.side*this.side;
}

function Circle(radius){
    this.radius = radius
}

Circle.prototype.__proto__ = Shape.prototype;

Circle.prototype.getArea = function() {
    return Math.PI*this.radius*this.radius
}

export let ShapesPrototype = {
    square: Square,
    circle: Circle
};
