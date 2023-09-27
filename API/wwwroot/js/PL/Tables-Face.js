const barter_btn = document.querySelectorAll('#btn-barter');
const table = document.querySelector(".main-table-playlist");
const btn_save = document.querySelector(".save");
const main_id = document.querySelector("#main").title;


btn_update()
















btn_save.onclick = function () { //������� ��� ������ ���������
    debugger
    let rowsArray = Array.from(table.rows).slice(1);
    const type = table.title;
    let url1;
    let url2;
    let answer = [];
    rowsArray.forEach((row) => {
        let state_btn = row.cells[0]
            .getElementsByTagName("div")[0]
            .title
        
        if (state_btn == "true") {            
            answer.push(row.title.split('/')[0]);
            
        }

    });
    debugger
    if (type == "Ad") {
        url1 = "/Web/PlayLists/Edit/EditAdPL";
        url2 = `/Web/PlayLists/Edit/EditUpdateAd?Id=${main_id}`
    }
    else {
        url1 = 'EditPL';
        url2 = `/Web/PlayLists/Edit/EditUpdate/?Id=${main_id}`;
    }
    
    $.ajax({
        type: 'POST',
        url: url1,
        data: { id: main_id, new_list_ids:answer },
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
            alert("��������� ����");
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



document.querySelector('#elastic').oninput = function () { //������� ��� ������ - ��� ��������� ����� ������ ������
    let value = this.value.trim();
    let items = document.querySelectorAll('#item');
    if (value != '') {
        items.forEach(function (elem) {
            let row = elem.parentElement.parentElement;
            if (elem.innerText.search(value) == -1) { //������� search ���������� ����������� ������ � ������� �������� � ������ 1 ��� -1 ��� ��������

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

function btn_update() { //���������� ������� ��� ���� ������ ��������
    barter_btn.forEach((el) => {
        el.addEventListener('click', () => { barter_btn_func(el) })

    })
}


function barter_btn_func(el) { //������� ��� ������ "��������" ��-���
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
function sortTablDown(colNum, type) {  // ���������� 
    debugger
    let tbody = table.querySelector('tbody');
    let rowsArray = Array.from(table.rows)
        .slice(1);
    let compare; //���������� ��� �������

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
    let compare; //���������� ��� �������

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