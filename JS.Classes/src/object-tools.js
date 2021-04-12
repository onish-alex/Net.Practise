export function PrintObjectMembers(obj, elem) {
    let result = "";

    for (let member of Object.getOwnPropertyNames(obj)) {
        result += member;
        result += "<br>";
    }

    for (let member of Object.getOwnPropertyNames(obj.__proto__)) {
        result += member;
        result += "<br>";
    }

    elem.innerHTML = result;
}

export function ObjectToArray(obj) {
    let result = [];

    for (let member of Object.getOwnPropertyNames(obj)) {
        let arrayMember = {};
        arrayMember[member] = obj[member];
        result.push(arrayMember);
    }

    for (let member of Object.getOwnPropertyNames(obj.__proto__)) {
        let arrayMember = {};
        arrayMember[member] = obj[member];
        result.push(arrayMember);
    }

    return result;
}