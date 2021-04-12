let GuessGameData = {
    guessedNumber: undefined,
    randomMin: 0,
    randomMax: 20,
}

window.onload = function() {
    let output = document.getElementById("output")
    let interval;
    
    let buttons = document.getElementsByTagName("button")
    
    for (let btn of buttons) {
        btn.addEventListener("click", function() {
            if (interval != undefined) {
                clearInterval(interval)
            }
        })
    }
    
    document.getElementById("date-time").addEventListener("click",  function() {
        interval = setInterval(() => {
            output.innerHTML = new Date()
        }, 1000)
    })
    
    document.getElementById("fun-with-dates").addEventListener("click", function() {
        output.innerHTML = GetFirstJanuarySundayYears(2014, 2050)
    })

    document.getElementById("guess-game").addEventListener("click", function() {
        let input = document.getElementById("guess-game-input").value;
        output.innerHTML = GuessGame(input); 
    })

    document.getElementById("get-url").addEventListener("click", function() {
        output.innerHTML = window.location.href;
    })

    document.getElementById("py-str").addEventListener("click", function() {
        let input = document.getElementById("py-str-input").value;
        output.innerHTML = AddPrefix(input, "PY");
    })

    document.getElementById("vowels-count").addEventListener("click", function() {
        let input = document.getElementById("vowels-count-input").value;
        output.innerHTML = GetVowelsCount(input);
    })

    document.getElementById("is-one-first-or-last").addEventListener("click", function() {
        let input = document.getElementById("is-one-first-or-last-input").value;
        let array = ParseIntArray(input);
        output.innerHTML = IsOneFirstOrLast(array);
    })

    document.getElementById("n-last").addEventListener("click", function() {
        let input = document.getElementById("n-last-array-input").value;
        let n = document.getElementById("n-last-n-input").value;
        let array = input.split(",");

        if (n === '') {
            output.innerHTML = "[" + GetNLast(array) + "]";    
        }
        else {
            output.innerHTML = "[" + GetNLast(array, n) + "]";
        }
    })

    document.getElementById("join").addEventListener("click", function() {
        let input = document.getElementById("join-string-input").value;
        let delimiter = document.getElementById("join-delimiter-input").value;
        let array = input.split(",");
        output.innerHTML = array.join(delimiter);
    })

    document.getElementById("max-product").addEventListener("click", function() {
        let input = document.getElementById("max-product-input").value;
        let array = ParseIntArray(input);
        output.innerHTML = PrintMaxNeighboursProduct(array);
    })

    document.getElementById("remove-duplicates").addEventListener("click", function() {
        let input = document.getElementById("remove-duplicates-input").value;
        let array = input.split(",");
        output.innerHTML = RemoveDuplicates(array);
    })

    document.getElementById("polygon-square").addEventListener("click", function() {
        let input = document.getElementById("polygon-square-input").value;
        output.innerHTML = GetPolygonSquare(input);
    })
    
}

function GetFirstJanuarySundayYears(startYear, endYear) {
    
    let searchedYears = [];

    for (let i = startYear; i <= endYear; i++) {
        let date = new Date(i,0,1,0,0,0,0);
        
        if (date.getDay() == 0) {
            searchedYears.push(i);
        }
    }

    return searchedYears;
}

function GuessGame(input) {
    
    let inputedNumber = parseInt(input);

    if (Number.isNaN(inputedNumber)) {
        return "Ошибка ввода!";
    }
    
    if (GuessGameData.guessedNumber === undefined) {
        GuessGameData.guessedNumber = Math.floor(Math.random() * (GuessGameData.randomMax - GuessGameData.randomMin) + GuessGameData.randomMin);
    }

    if (input < GuessGameData.guessedNumber) {
        return "Загаданное больше!"
    } 
    else if (input > GuessGameData.guessedNumber) {
        return "Загаданное меньше!"
    } 
    else {
        GuessGameData.guessedNumber = undefined;
        return "Вы угадали!"
    }

}

function AddPrefix(str, prefix) {
    if (str === '' 
     || str.substring(0, 2) === prefix) {
         return str;
    } 
    else {
        return prefix+str;
    }
}

function GetVowelsCount(str) {
    let vowels = "euyuia"; 
    let counter = 0;
    
    for (let i = 0; i < str.length; i++) {
        if (vowels.indexOf(str.charAt(i)) !== -1) {
            counter++;
        }
    }

    return counter;
}

function IsOneFirstOrLast(array) {
    return (array[0] === 1 || array[array.length - 1] === 1)
        ? "Да"
        : "Нет"
}

function GetNLast(array, n = 1) {
    return n >= array.length ? array : array.splice(array.length - n, n)
}

function PrintMaxNeighboursProduct(array) {

    if (array.length < 2) {
        return "Incorrect array!"
    }

    let maxProduct = Number.MIN_VALUE;
    let operands = [];

    for (let i = 0; i < array.length - 1; i++) {
        let product = array[i] * array[i + 1]

        if (product > maxProduct) {
            maxProduct = product
            operands[0] = array[i]
            operands[1] = array[i + 1]
        }
    }

    return maxProduct + " = " + operands[0] + " * " + operands[1]
}

function RemoveDuplicates(array) {
    let filteredArray = [];

    for (let item of array) {
        if (filteredArray.indexOf(item) === -1) {
            filteredArray.push(item);
        }
    }

    return filteredArray;
}

function GetPolygonSquare(n) {
    
    let numN = parseInt(n);
    
    if (Number.isNaN(numN)) {
        return "Неверное значение!"
    }

    if (numN === 0) {
        return 0;
    }
    
    let adder = 0;
    let square = 1;

    for (let i = 0; i < n; i++) {
        square += adder;
        adder += 4;
    }

    return square;
}

function ParseIntArray(input) {
    let numbersStr = input.split(",");
    let result = [];

    for (let item of numbersStr) 
    {
        let number = parseInt(item);
        if (!Number.isNaN(item)) {
            result.push(number);
        }
    }
    
    return result;
}