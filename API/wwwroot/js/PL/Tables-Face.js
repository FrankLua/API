const barter_btn = document.querySelectorAll('#btn-barter');
const table = document.querySelector(".main-table-playlist");
const btn_save = document.querySelector(".save");
const main_id = document.querySelector("#main").title;


btn_update()
















btn_save.onclick = function () { //Функция для кнопки сохранить
    
    let rowsArray = Array.from(table.rows).slice(1);

    let answer = [];
    rowsArray.forEach((row) => {
        let state_btn = row.cells[0]
            .getElementsByTagName("button")[0]
            .value
        
        if (state_btn == "true") {            
            answer.push(row.title);
        }

    });
    
    
    $.ajax({
        type: 'POST',
        url: 'EditPL',
        data: { id: main_id, new_list_ids:answer },
        success: function (data) {
            if (data == true) {
                
                var targetElementTitle = document.querySelector(".content");
                
                //targetElement.load(`/Web/PlayLists/Edit/EditUpdate/?Id=${main_id} .content`);
                $.ajax({
                    url: `/Web/PlayLists/Edit/EditUpdate/?Id=${main_id}`,
                    dataType: 'html',
                    success: function (data) {
                        debugger
                        $('#lable-page').html(data);
                        

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
















function pageLoad(sender, args) {
    $('#body').trigger('create');
}

function btn_update() { //Добавление функции для всех кнопок бартеров
    barter_btn.forEach((el) => {
        el.addEventListener('click', () => { barter_btn_func(el) })

    })
}


function barter_btn_func(el) { //Функция для кнопок "бартеров" да-нет
    debugger
    const value = el.value
    if (value == "true") {
        debugger
        el.textContent = "+"
        el.value = "false"
    }
    else {
        el.textContent = "-"
        el.value = "true"
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