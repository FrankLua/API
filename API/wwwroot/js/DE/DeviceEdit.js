let btn_Edit = document.querySelectorAll('#edit');
let btn_Apply = document.querySelectorAll('#apply')
let selector = document.querySelector('#selected-day')
let from = document.querySelector('#from')
let to = document.querySelector('#to')
let listDay = document.querySelectorAll('#list-Day');
let btn_Save = document.querySelector('#btn-Save');
let btn_Cancel = document.querySelectorAll('#cancel');





btn_update()

function btn_update() { //Добавление функции для всех кнопок бартеров
    let btn_Edit = document.querySelectorAll('#edit');
    let btn_Apply = document.querySelectorAll('#apply')
    let selector = document.querySelector('#selected-day');
    let from = document.querySelector('#from')
    let to = document.querySelector('#to');
    let listDay = document.querySelectorAll('#list-Day');
    let btn_Cancel = document.querySelectorAll('#cancel');

    add_Event_Btn_Edit()
    add_Event_Btn_Apply()
    add_Event_Btn_Cancel()
    selector.oninput = function () { // функция для дней и времени
        debugger
        if (selector.value != "none") {
            from.style.visibility = "visible";
            to.style.visibility = "visible";
        }
        else {
            from.style.visibility = "hidden";
            to.style.visibility = "hidden";
        }
    }
    from.oninput = function () {

        if (selector.value != "none" && from.value != "none" && to.value != "none") { // Проверяем выбраны не пустые опции

            let oldValue = selector.value.split('/')[1]; // Парсим значение которое не будем менять
            let anotherTime = Number(to.value.split(' ')[1].split(':')[0]); // Узнаем что выбранно в друго слекте 
            let mainTime = Number(from.value.split(' ')[1].split(':')[0]);
            let optionsSelector = selector.options; // берем массив нод опций
            let daySelector = selector.value.split('/')[2]// узнаем день который выбран в селекторе

            if (mainTime <= anotherTime) { // сравниваем значения 
                for (var opt, j = 0; opt = optionsSelector[j]; j++) {
                    let dayOpt = opt.value.split('/')[2]; //узнаём день который опции через его value           


                    if (dayOpt == daySelector) {

                        let newValue = from.value + '/' + to.value + '/' + daySelector; // создаем новый value
                        opt.value = newValue; // Added value in option
                        selector.options[j].remove() //delete old option
                        selector.insertBefore(opt, selector.children[j]) //added option in selector
                        break;

                    }

                }
                for (var item, j = 0; item = listDay[j]; j++) {


                    if (item.children[0].innerText == daySelector) {
                        let fromTime = from.value.split(' ')[1].split(':')[0] + ":" + from.value.split(' ')[1].split(':')[1]; // парсим данные из уже записаных нами ранее данных
                        let toTime = to.value.split(' ')[1].split(':')[0] + ":" + to.value.split(' ')[1].split(':')[1];
                        let newTime = fromTime + ' - ' + toTime;
                        item.children[1].innerText = newTime;
                        item.children[0].style.backgroundColor = "brown";
                        item.children[1].style.backgroundColor = "brown";
                        break;
                    }

                }
            }




        }

    }
    to.oninput = function () {

        if (selector.value != "none" && to.value != "none" && from.value != "none") {
            let oldValue = selector.value.split('/')[0];
            let optionsSelector = selector.options;
            let anotherTime = Number(from.value.split(' ')[1].split(':')[0]);
            let mainTime = Number(to.value.split(' ')[1].split(':')[0]);
            let daySelector = selector.value.split('/')[2];
            if (anotherTime < mainTime) {
                for (var opt, j = 0; opt = optionsSelector[j]; j++) {
                    let dayOpt = opt.value.split('/')[2];

                    if (dayOpt == daySelector) {
                        let newValue = from.value + '/' + to.value + '/' + daySelector;
                        opt.value = newValue;
                        selector.options[j].remove()
                        selector.insertBefore(opt, selector.children[j])
                        break;
                    }
                }
                for (var item, j = 0; item = listDay[j]; j++) {

                    debugger
                    if (item.children[0].innerText == daySelector) {
                        let fromTime = from.value.split(' ')[1].split(':')[0] + ":" + from.value.split(' ')[1].split(':')[1];
                        let toTime = to.value.split(' ')[1].split(':')[0] + ":" + to.value.split(' ')[1].split(':')[1];
                        let newTime = fromTime + ' - ' + toTime;
                        item.children[1].innerText = newTime;
                        item.children[0].style.backgroundColor = "brown";
                        item.children[1].style.backgroundColor = "brown";
                        break;
                    }

                }
            }

        }

    }

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
    const _id = document.querySelector('#id').innerText;
    const name = document.querySelector('#name').innerText;
    const address = document.querySelector('#address').innerText;
    const m_PlayLIst = document.querySelector('#m-PlayList').value;
    const a_PlayLIst = document.querySelector('#a-PlayList').value;

    
    const timeIntervals = searchTimeIntervals();
    var device = new Device(_id, name, address, m_PlayLIst, a_PlayLIst)   
    const day = new Date();
    console.log(typeof (timeIntervals));
    debugger
    $.ajax({
        type: 'PUT',
        url: 'DeviceSave',             
        data: { device: device, times: timeIntervals },     

        
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
        this.intervals = intervals;
    }
}





function searchTimeIntervals() {
    var options = document.querySelector('#selected-day').options;
    var timeIntervals = []
    
    for (var item, i = 1; item = options[i]; i++) {
        let day = item.value.split('/')[2];
        let from = item.value.split('/')[0];
        let to = item.value.split('/')[1];  
        let finalString = from.split(' ')[1].split(':')[0] +'/'+ to.split(' ')[1].split(':')[0] +'/'+ day;
        //debugger
        //let newTimeInterval = TimeIntervals.constructor(day, from, to)
        timeIntervals.push(finalString);
    }
    return timeIntervals;
}












































































    











































