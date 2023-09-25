document.querySelector("#common-elastic").oninput = function () //функция для импута - для обработки ввода каждой кнопки
{
    debugger
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