const btnDelete = document.querySelectorAll('#del')


btn_update();
function btn_update() { //Добавление функции для всех кнопок delete
    document.querySelectorAll('#del').forEach((el) => {
        el.addEventListener('click', () => { delete_btn_func(el) })

    })
}


function delete_btn_func(el) { //Функция для кнопок delete
    debugger
    var id = el.value;
    $.ajax({
        type: 'DELETE',        
        url: `/Web/Devices/DeviceFace/Delete/?Id=${id}`,
        success: function (data) {
            debugger

            $.ajax({
                url: `/Web/Devices/DeviceFace/UpdateDevice`,
                dataType: 'html',
                success: function (data) {
                    debugger
                    alert("Device delited");
                    $('#main-content').html(data);
                    btn_update();

                }
            })
        },
        error: function () {
            alert("error");
        }
    });



}