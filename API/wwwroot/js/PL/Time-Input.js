const inputTime = document.querySelectorAll('#time')
startTimeBTN()
function startTimeBTN() {
    document.querySelectorAll('#time').forEach((el => {
        debugger
        el.oninput = function (e) {
            debugger
            e = e || window.event;




            let type = e.inputType
            let last = el.value.length - 1;
            let number = Number(e.data);
            console.log(typeof number);
            if (Number(el.value[0]) > 2) {

                el.value = '2';
            }
            if (Number(el.value[1]) > 3) {
                let first = el.value[0]
                el.value = first + '3'

            }
            if (Number(el.value[3]) > 5) {

                el.value = el.value[0] + el.value[1] + ':' + '5';
            }

            if (typeof (number) == "number") {
                if (el.value.length == 3 & el.value[last] != ':') {
                    let firstPart = el.value[0] += el.value[1];
                    let secondPart = el.value[2];
                    if (Number(secondPart) > 5) secondPart = '5';
                    let finishString = firstPart + ':' + secondPart;
                    el.value = finishString;
                }
                if (el.value[last] == ':' & type == "deleteText") {
                    el.value = el.value.trim(':')
                }
                if (el.value.length == 6) {
                    el.value = el.value.slice(0, -1);
                }
            }

        }

    }))
}
 