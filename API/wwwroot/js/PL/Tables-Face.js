const barter_btn = document.querySelectorAll('#btn-barter');
const table = document.querySelector(".main-table-playlist");
const btn_save = document.querySelector(".save");
const main_id = document.querySelector("#main").title;


btn_update()
















btn_save.onclick = function () { //Функция для кнопки сохранить
    
    let rowsArray = Array.from(document.querySelector(".main-table-playlist").rows).slice(1);
    const type = table.title;
    let url1;
    let url2;
    let answer = [];
    
    
    
    if (type == "Ad") {
        url1 = "/Web/PlayLists/Edit/EditAdPL";
        url2 = `/Web/PlayLists/Edit/EditUpdateAd?Id=${main_id}`;
        let intervals = []; // переменная для интервалов (проверок над ними)
        let correctly = true; // переменная для отслеживания всё ли корректно заполненр
        rowsArray.forEach((row) => {
            let state_btn = row.cells[0]
                .getElementsByTagName("div")[0]
                .title

            if (state_btn == "true") {
                let id = row.title.split('/')[0];
                let time = row.cells[4].children[0].value;
                if (time.length != 5) {
                    alert("You did not correctly write time")
                    correctly = false;
                    return;
                }
                
                let interval = new ad_files(id, time);
                
                
                intervals.push(interval);
            }

        });
        
        boolintevals = searchBadIntervals(intervals);        
        let playlist = JSON.stringify(new Media_Ad_playlist(main_id, document.querySelector('#main').dataset.name, intervals));       
        debugger
        if (correctly & boolintevals) {
            $.ajax({
                type: 'POST',
                url: url1,
                data: { playlistJson:playlist},
                success: function (data) {
                    if (data == true) {

                        var targetElementTitle = document.querySelector(".content");

                        
                        $.ajax({
                            url: url2,
                            dataType: 'html',
                            success: function (data) {
                                debugger
                                document.querySelector('#lable-page').innerHTML = data;
                                
                                alert("Changes is saved");
                                startTimeBTN();
                                btn_update();
                                debugger


                            }
                        })
                    }
                    else {

                    }
                },
                error: function () {
                    alert("Произошел сбой");
                }
            });

        }



        
    }
    else {
        rowsArray.forEach((row) => {
            let state_btn = row.cells[0]
                .getElementsByTagName("div")[0]
                .title

            if (state_btn == "true") {
                answer.push(row.title.split('/')[0]);

            }

        });
        url1 = 'EditPL';
        url2 = `/Web/PlayLists/Edit/EditUpdate/?Id=${main_id}`;
        $.ajax({
            type: 'POST',
            url: url1,
            data: { id: main_id, new_list_ids: answer },
            success: function (data) {
                if (data == true) {

                    var targetElementTitle = document.querySelector(".content");

                    //targetElement.load(`/Web/PlayLists/Edit/EditUpdate/?Id=${main_id} .content`);
                    $.ajax({
                        url: url2,
                        dataType: 'html',
                        success: function (data) {
                            debugger
                            var kel = document.querySelector('.lable-page').innerHTML;
                            kel.innerHTML = data;
                            alert("Changes is saved");
                            //$('#lable-page').innerHTML = data;


                            //targetElement.remove();
                            debugger


                        }
                    })
                }
                else {

                }
            },
            error: function () {
                alert("Произошел сбой");
            }
        });
    }
    
   
}



table.onclick = function (e) {
    console.log(e.target.tagName)
    if (e.target.tagName != 'TH') {
        return;
    }
    let th = e.target
    debugger
    if (th.title == "true") {
        sortTablUp(th.cellIndex, th.dataset.type);
        th.title = "false"
    }
    else {
        sortTablDown(th.cellIndex, th.dataset.type);
        th.title = "true"
    }
}



document.querySelector('#elastic').oninput = function () { //функция для импута - для обработки ввода каждой кнопки
    let value = this.value.trim();
    let items = document.querySelectorAll('#item');
    if (value != '') {
        items.forEach(function (elem) {
            let row = elem.parentElement.parentElement;
            if (elem.innerText.search(value) == -1) { //функция search сравнивает поступаемые данные с данными эелмента и отдает 1 или -1 как контейнс

                row.classList.add('hide');
            }
            else {
                row.classList.remove('hide')
            }

        })
    }
    else {
        items.forEach(function (elem) {
            let row = elem.parentElement.parentElement;
            row.classList.remove('hide');
        })
    }


}

class Media_Ad_playlist {
    constructor(id, name, ad_files) {
        this._id = id,
            this.name = name,

            this.ad_files = ad_files

    }

}

class ad_files {
    constructor(file, start) {
        this.file = file,
            this.start_time = start
    }
}











function pageLoad(sender, args) {
    $('#body').trigger('create');
}

function btn_update() { //Добавление функции для всех кнопок бартеров
    document.querySelectorAll('#btn-barter').forEach((el) => {
        el.addEventListener('click', () => { barter_btn_func(el) })

    })
}


function barter_btn_func(el) { //Функция для кнопок "бартеров" да-нет
    debugger
    const value = el.title
    if (value == "true") {
        
        el.classList = "btn-barter-enable";
        el.title = "false";
        debugger
        var th = el.parentElement.parentElement;
        th.classList = "";
        th.classList = "disable_row";
    }
    else {

        el.classList = "btn-barter-disable";
        el.title = "true";
        debugger
        var th = el.parentElement.parentElement;
        th.classList = "";
        th.classList = "enable_row";
    }



}
function sortTablDown(colNum, type) {  // сортировка 
    debugger
    let tbody = table.querySelector('tbody');
    let rowsArray = Array.from(table.rows)
        .slice(1);
    let compare; //переменная для функции

    if (colNum == 0) {
        switch (type) {
            case 'string': {
                compare = function (rowA, rowB) {
                    let A = rowA.cells[colNum]
                        .getElementsByTagName("button")[0]
                        .textContent
                    let B = rowB.cells[colNum]
                        .getElementsByTagName("button")[0]
                        .textContent
                    return A > B ? 1 : -1
                }
                break;
            }

        }
    }
    else {
        switch (type) {
            case 'string': {
                compare = function (rowA, rowB) {
                    let A = rowA.cells[colNum]
                        .getElementsByTagName("p")[0]
                        .textContent
                    let B = rowB.cells[colNum]
                        .getElementsByTagName("p")[0]
                        .textContent
                    return A > B ? 1 : -1
                }
                break;
            }

        }

    }

    rowsArray.sort(compare)
    tbody.append(...rowsArray);
}
function sortTablUp(colNum, type) {
    
    let tbody = table.querySelector('tbody');
    let rowsArray = Array.from(table.rows)
        .slice(1);
    let compare; //переменная для функции

    if (colNum == 0) {
        switch (type) {
            case 'string': {
                compare = function (rowA, rowB) {
                    let A = rowA.cells[colNum]
                        .getElementsByTagName("button")[0]
                        .textContent
                    let B = rowB.cells[colNum]
                        .getElementsByTagName("button")[0]
                        .textContent
                    return A < B ? 1 : -1
                }
                break;
            }

        }
    }
    else {
        switch (type) {
            case 'string': {
                compare = function (rowA, rowB) {
                    let A = rowA.cells[colNum]
                        .getElementsByTagName("p")[0]
                        .textContent
                    let B = rowB.cells[colNum]
                        .getElementsByTagName("p")[0]
                        .textContent
                    return A < B ? 1 : -1
                }
                break;
            }

        }

    }

    rowsArray.sort(compare)
    tbody.append(...rowsArray);
}




function searchBadIntervals(array) {
    let length = array.length;
    let score = 0;
    
    for (var itemOne, i = 0;  i < length; i++) {
        itemOne = array[i].start_time;
        for (var itemTwo, j = 0; j < length ; j++) {
            itemTwo = array[j].start_time;
            if (itemOne == itemTwo) {
                score++;
            }
        }

    }
    if (score > array.length) {
        alert("You did not correctly write time")
        return false;
    }
    else {
        return true;
    }
}