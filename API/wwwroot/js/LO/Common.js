document.querySelector("#common-elastic").oninput = function () //������� ��� ������ - ��� ��������� ����� ������ ������
{
    debugger
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