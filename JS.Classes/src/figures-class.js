'use strict'

class Shape {

    constructor() {}

    getArea() {}

}

class Square extends Shape {
    constructor(side) {
        super()
        this.side = side
    }

    getArea() {
        return this.side*this.side
    }
}

class Circle extends Shape {
    constructor(radius) {
        super()
        this.radius = radius
    }

    getArea() {
        return Math.PI*this.radius*this.radius
    }  
}

export let ShapesClass = {
    square: Square,
    circle: Circle
};