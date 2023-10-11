const table = document.getElementById('table');
window.onload = function () {
    function strPad() {        
        const valuE = this.value.split('/')[0];
        const type = this.value.split('/')[1];      
        
        load();
        $.ajax({
            type: 'POST',
            url: 'Delete',
            data: { id: valuE, type:type },
            success: function (data) {
                if (data.error != null) {
                    loadoff()
                    const str = data.error;                    
                }
                else {
                    loadoff()
                    const tbody = document.getElementById(valuE);
                    
                    tbody.parentNode.removeChild(tbody);
                    const str = data.data                                   
                }
            },
            error: function () {
                alert("Error");
            }
        });
 
    }

    var bt = document.querySelectorAll('button[id^="button"]');
    for (var i = 0; i < bt.length; i++) {
        bt[i].onclick = strPad;
    }
}
