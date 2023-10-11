const btnSend = document.querySelector('#send');




btnSend.onclick = function () {
    debugger
    let name = document.querySelector('#name').value;
    let address = document.querySelector('#address').value;
    if (name != "" && address != "") {
        $.ajax({
            type: 'POST',
            data: { name: name, address: address },
            url: '/Web/Devices/DeviceFace/CreateDevice',
            success: function (data) {
                debugger

                $.ajax({
                    url: `/Web/Devices/DeviceFace/UpdateDevice`,
                    dataType: 'html',
                    success: function (data) {

                        alert("Device created");
                        $('#main-content').html(data);
                        debugger
                        btn_update();
                        loadoff();


                    }
                })
            },            
            error: function () {
                alert("error");
            }
        });
    }
    else {
        alert("You did not correctly filled in web form");
    }
    







}