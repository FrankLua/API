
const patternInpute = new RegExp(/^(0[7-9]|1[0-9]|2[0-3]):[0-5][0-9]$/)
const patternInterval = new RegExp(/(^[1-2][0-9][0-9]|^[3][0-5][0-9]|[3][6][0]|^[1-9])$/)
const patternIntervalDurak = new RegExp(/^[1-9][0-9]$/)

cheakTime()
function cheakTime() {    
    const inputTime = document.querySelectorAll('#time')

    const cheakBox = document.querySelectorAll('#checkBox')  

    const interval = document.querySelectorAll('#interval')

    interval.forEach((el => {
        el.addEventListener('input', () => {
            let value = el.value;
            debugger
            if (el.value.length > 3) {
                el.value = el.value.substring(0, el.value.length - 1);
                value = el.value;
            }
            if (patternInterval.test(value) || patternIntervalDurak.test(value)) {
                el.parentElement.children[2].innerText = "Ok"
            }
            else  {
                el.parentElement.children[2].innerText = 'Bad'
            }
            
            
        })
    }))


    cheakBox.forEach((el) => {
        
        el.addEventListener('change', () => {
            debugger
            if (el.checked) {
                el.parentElement.children[1].disabled = false;
                el.parentElement.children[2].innerText = 'Bad';
                
                el.parentElement.children[1].value = "";
}
            else {
                el.parentElement.children[1].disabled = true;
                el.parentElement.children[2].innerText = 'Status'
                
                el.parentElement.children[1].value = "";
            }
            
        })
    })

    inputTime.forEach((el) => {
        
        
        el.addEventListener('input', () => {
            
            let time = el.value;
            
            if (patternInpute.test(time)) {                
                el.parentElement.children[1].innerText = "Ok"
            }
            else {
                
                el.parentElement.children[1].innerText = "Bad"
            }
        })
        el.addEventListener('keydown', (event) => {
            
            if (el.value.length == 2 && event.code != "Backspace") {
                el.value = el.value += ':';
            }
            if (el.value.length >= 5) {
                el.value = el.value.substring(0, el.value.length - 1);
            }

        })
    })



    
}
function getActualStatus() {
    var btnS = document.querySelectorAll("#btn-barter")
    btnS.forEach((btn) => {
        var th = btn.parentElement.parentElement;
        var inpute = th.querySelector('#time')
        var interval = th.querySelector('#interval')
        var spanInpute = th.querySelector("#textInpute")
        var spanInterval = th.querySelector('#textInterval');
        var checkBox = th.querySelector("#checkBox");
        if (btn.getAttribute("data_state") == "true") {            
            if (patternInpute.test(inpute.value)) {
                spanInpute.innerText = "Ok";
            }
            else {
                spanInpute.innerText = "Bad";
            }
            if (checkBox.checked) {
                if (patternInterval.test(interval.value)) {
                    spanInterval.innerText = "Ok";
                }
                else {
                    spanInterval.innerText = "Bad";
                }
            }
            else {
                spanInterval.innerText = "Status";
            }
            
        }
        else {
            debugger
            interval.value = "";
            spanInterval.innerText = "Status";
            inpute.value = "";
            spanInpute.innerText = "Status";

        }
    })   


    
}










/*startTimeBTN()*/
//function startTimeBTN() {
//    document.querySelectorAll('#time').forEach((el => {
//        debugger
//        el.oninput = function (e) {
//            debugger
//            e = e || window.event;




//            let type = e.inputType
//            let last = el.value.length - 1;
//            let number = Number(e.data);
//            console.log(typeof number);
//            if (Number(el.value[0]) > 2) {

//                el.value = '2';
//            }
//            if (Number(el.value[1]) > 3) {
//                let first = el.value[0]
//                el.value = first + '3'

//            }
//            if (Number(el.value[3]) > 5) {

//                el.value = el.value[0] + el.value[1] + ':' + '5';
//            }

//            if (typeof (number) == "number") {
//                if (el.value.length == 3 & el.value[last] != ':') {
//                    let firstPart = el.value[0] += el.value[1];
//                    let secondPart = el.value[2];
//                    if (Number(secondPart) > 5) secondPart = '5';
//                    let finishString = firstPart + ':' + secondPart;
//                    el.value = finishString;
//                }
//                if (el.value[last] == ':' & type == "deleteText") {
//                    el.value = el.value.trim(':')
//                }
//                if (el.value.length == 6) {
//                    el.value = el.value.slice(0, -1);
//                }
//            }

//        }

//    }))
//}
 