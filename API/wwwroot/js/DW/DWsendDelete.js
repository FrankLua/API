const table = document.getElementById('table');
window.onload = function () {
    function strPad() {
        console.log(this.value)
        const valuE = this.value
        load();
        $.ajax({
            type: 'POST',
            url: 'Delete',
            data: { id: this.value },
            success: function (data) {
                if (data.error != null) {
                    loadoff()
                    const str = data.error;
                    showbox(str);
                }
                else {
                    loadoff()
                    const tbody = document.getElementById(valuE);
                    
                    tbody.parentNode.removeChild(tbody);
                    const str = data.data
                    showbox(str);                    
                }
            },
            error: function () {
                alert("Произошел сбой");
            }
        });
 
    }

    var bt = document.querySelectorAll('button[id^="button"]');
    for (var i = 0; i < bt.length; i++) {
        bt[i].onclick = strPad;
    }
}
