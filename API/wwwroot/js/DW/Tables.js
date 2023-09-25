const talbeFile = document.querySelector('#table-file');


talbeFile.onclick = function (e) {
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

function sortTablUp(colNum, type) {
    debugger
    let tbody = talbeFile.querySelector('tbody');
    let rowsArray = Array.from(talbeFile.rows)
        .slice(1);
    let compare; //переменная для функции

    if (colNum == 0) {
        switch (type) {
            case 'string': {
                compare = function (rowA, rowB) {
                    debugger
                    let A = rowA.cells[colNum].innerText
                    let B = rowB.cells[colNum].innerText 
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
                    debugger
                    let A = rowA.cells[colNum].innerText
                    let B = rowB.cells[colNum].innerText 
                    return A < B ? 1 : -1
                }
                break;
            }

        }

    }

    rowsArray.sort(compare)
    tbody.append(...rowsArray);
}
function sortTablDown(colNum, type) {  // сортировка 
    debugger
    let tbody = talbeFile.querySelector('tbody');
    let rowsArray = Array.from(talbeFile.rows)
        .slice(1);
    let compare; //переменная для функции

    if (colNum == 0) {
        switch (type) {
            case 'string': {
                compare = function (rowA, rowB) {
                    debugger
                    let A = rowA.cells[colNum].innerText                        
                    let B = rowB.cells[colNum].innerText 
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
                    debugger
                    let A = rowA.cells[colNum].innerText
                    let B = rowB.cells[colNum].innerText 
                    return A > B ? 1 : -1
                }
                break;
            }

        }

    }

    rowsArray.sort(compare)
    tbody.append(...rowsArray);
}