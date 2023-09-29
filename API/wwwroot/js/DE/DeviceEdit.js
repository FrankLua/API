let btn_Edit = document.querySelectorAll('#edit');
let btn_Apply = document.querySelectorAll('#apply')

const selectors = document.querySelectorAll("#selectors")

let listDay = document.querySelectorAll('#list-Day');
let btn_Save = document.querySelector('#btn-Save');
let btn_Cancel = document.querySelectorAll('#cancel');





btn_update()

function btn_update() { //Добавление функции для всех кнопок бартеров
    let btn_Edit = document.querySelectorAll('#edit');
    let btn_Apply = document.querySelectorAll('#apply')  
    const selectors = document.querySelectorAll("#selectors")
    let listDay = document.querySelectorAll('#list-Day');
    let btn_Cancel = document.querySelectorAll('#cancel');

    add_Event_Btn_Edit()
    add_Event_Btn_Apply()
    add_Event_Btn_Cancel()
    
    debugger
    selectors.forEach((td => {
        let from = td.children[0];
        let to = td.children[2];
        from.oninput = function () {
            debugger
            let parent = from.parentElement.childNodes;
            let to = findElement(parent, "to")
            if (from.value != "none" && to.value != "none") { // Проверяем выбраны не пустые опции

                let oldValue = to.parentElement.children[1].innerText.split(' - ')[0]; // Парсим значение которое не будем менять
                let anotherTime = Number(to.value.split(':')[0]); // Узнаем что выбранно в друго слекте 
                let mainTime = Number(from.value.split(':')[0]);
                let target = to.parentElement.children[1];  // берем массив нод опций
                let day = from.parentElement.parentElement.children[0].innerText;// узнаем день который выбран в селекторе

                if (mainTime < anotherTime) { // сравниваем значения 
                    let restulString = `${from.value} - ${to.value}`;
                    target.innerText = restulString;
                    debugger
                    from.parentElement.parentElement.children[0].style.backgroundColor = "brown";
                    from.parentElement.parentElement.children[1].style.backgroundColor = "brown";
                }
            }

        }
        to.oninput = function () {
            debugger
            let parent = to.parentElement.childNodes;
            let from = findElement(parent, "from")
            if (to.value != "none" && from.value != "none") {
                debugger

                let target = to.parentElement.children[1];
                let anotherTime = Number(from.value.split(':')[0]);
                let mainTime = Number(to.value.split(':')[0]);
                if (anotherTime < mainTime) {
                    let restulString = `${from.value} - ${to.value}`;
                    target.innerText = restulString;
                    debugger
                    from.parentElement.parentElement.children[0].style.backgroundColor = "brown";
                    from.parentElement.parentElement.children[1].style.backgroundColor = "brown";

                }

            }

        }
    }));
    

    function add_Event_Btn_Cancel() {
        btn_Cancel.forEach((el => {
            el.addEventListener('click', () => {
                func_Cancel(el)
            })
        }))
    }
    

    function func_Cancel(el) {
        debugger
        let parent = el.parentElement
        let btn_apply = parent.children[3];
        let btn_edit = parent.children[2];
        let inputFild = parent.children[0];


        btn_apply.style.pointerEvents = 'none';
        btn_apply.style.visibility = 'hidden';

        btn_edit.style.visibility = 'visible';
        btn_edit.style.pointerEvents = "all"; // переключаем кнопки

        inputFild.style.visibility = 'hidden';
        inputFild.style.pointerEvents = 'none';


        el.style.pointerEvents = "none";
        el.style.visibility = 'hidden';
    }


    function add_Event_Btn_Apply() { //Добавление функции для всех кнопок "Принять"
        btn_Apply.forEach((el => {
            el.addEventListener('click', () => { func_Apply(el) })

        }))
    }

    function func_Apply(el) {
        debugger
        let parent = el.parentElement
        let inputFild = parent.children[0];
        let newText = parent.children[0].value // находим элемент который изменен <div>
        let edit_element = parent.parentElement.children[0]; // элемент который будем менять       
        let btn_cancel = parent.children[1];
        let btn_edit = parent.children[2];
        edit_element.innerText = newText

        debugger

        btn_cancel.style.pointerEvents = 'none';
        btn_cancel.style.visibility = 'hidden';

        btn_edit.style.visibility = 'visible';
        btn_edit.style.pointerEvents = "all"; // переключаем кнопки

        inputFild.style.visibility = 'hidden';
        inputFild.style.pointerEvents = 'none';


        el.style.pointerEvents = "none";
        el.style.visibility = 'hidden';



    }


    function add_Event_Btn_Edit() { //Добавление функции для всех кнопок редакторов
        btn_Edit.forEach((el) => {
            el.addEventListener('click', () => { func_Edit(el) })
        })
    }
    function func_Edit(el) { //Функция для кнопок "редакторов"

        
        let parent = el.parentElement // находи родителя
        
        let edit_Element = parent.children[0] // находим элемент  который будем менять <div>        
        let btn_apply = parent.children[3] //находим кнопку apply и включаем её 
        let btn_cancel = parent.children[1] //находим кнопку отмены и включаем её 
        let old_Param = edit_Element.innerText
        el.style.visibility = `hidden`;   

       
        btn_apply.style.visibility = "visible";
        btn_apply.style.pointerEvents = "all"; // включаем кнопку 
        
        
        btn_cancel.style.visibility = "visible";
        btn_cancel.style.pointerEvents = "all";
        
        
        edit_Element.style.visibility = "visible";
        edit_Element.style.pointerEvents = "all";

    }
}
























btn_Save.onclick = function () {
    debugger;
    const _id = document.querySelector('#id').innerText;
    const name = document.querySelector('#name').innerText;
    const address = document.querySelector('#address').innerText;
    var m_PlayLIst = document.querySelector('#m-PlayList').value;
    var a_PlayLIst = document.querySelector('#a-PlayList').value;

    debugger
    const timeIntervals = searchTimeIntervals();
    debugger
    if (m_PlayLIst == "none")
    {
        m_PlayLIst = null;
    }
    
    if (a_PlayLIst == "none") a_PlayLIst = undefined;
    var device = new Device(_id, name, address, m_PlayLIst, a_PlayLIst)  
    var answerDevice = JSON.stringify(device)
    var answerIntervals = JSON.stringify(timeIntervals)
    debugger
    $.ajax({
        type: 'PUT',
        url: 'DeviceSave',
        
        data: { deviceJson: answerDevice, intervalsJson: answerIntervals },    
        datatype: "text",
        success: function (data) {            
                debugger;
            var targetElementTitle = document.querySelector('#device-contentt');
            var targetElementTitle = document.querySelector(".content");

            //targetElement.load(`/Web/PlayLists/Edit/EditUpdate/?Id=${main_id} .content`);
            $.ajax({               
                url: `/Web/Devices/DevicesEdit/DeviceUpdate/?Id=${_id}`,
                dataType: 'html',
                success: function (data) {
                    debugger
                    $('#device-content').html(data);                  
                    btn_update();
                    //targetElement.remove();
                   


                }
            })

                
            
            

            
        },
        error: function () {
            alert("Произошел сбой");
        }
    });

}

class Device {
    constructor(id, name, address, media_play_list, ad_playlist, intervals) {
        this._id = id;
        this.name = name;
        this.adress = address;
        this.media_play_list = media_play_list;
        this.ad_playlist = ad_playlist;
        this.intervals = [];
    }
}





function searchTimeIntervals() {
    
    var timeIntervals = []
    var day = [];
    days = searchDay();
    debugger;
    
    for (var item, i = 0; item = document.querySelectorAll("#selectors")[i], day = days[i]; i++) {  

        var result = `${day}/${item.children[1].innerText}`;
        timeIntervals.push(result);
    }
    return timeIntervals;
}

function searchDay() {
    debugger
    var listDay =[]
    document.querySelectorAll('#list-Day').forEach((el => {
        listDay.push(el.children[0].innerText);
    }))
    return listDay;
}


function findElement(arrayElements, id) {
   
    let abc 
    arrayElements.forEach( (element) => {
        if (element.id == id) {
            abc = element;            
        }
        
    });      
    return abc;
    
}








































































    











































