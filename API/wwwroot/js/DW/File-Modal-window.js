
const overlay = document.getElementById('modal-overlay')
const btns = document.querySelectorAll('.btn')
const loadbtn = document.getElementById('load');
const loadingoverlay = document.getElementById('loading');

var file 

btns.forEach((el)=>{
    
    el.addEventListener('click',(e) => {
        let path = e.currentTarget.getAttribute('data-path');
        overlay.style.visibility = 'visible';
        overlay.style.opacity = 1;
        document.querySelector(`[data-target="${path}"]`).style.display = 'block';
    })
})
overlay.onclick = function (e) {     
    if (e.target == overlay) {
        overlay.style.opacity = 0;
        overlay.style.visibility = 'hidden';
    }
    
}
function load() {
    
    loadingoverlay.style.visibility = 'visible';
    loadingoverlay.style.opacity = 1;
    loadingoverlay.style.zIndex = 5;
}
function loadoff() {

    loadingoverlay.style.visibility = 'hidden';
    loadingoverlay.style.opacity = 0;
    loadingoverlay.style.zIndex = 0;
}

function upload(inpute) {
    debugger
    file = inpute.files[0]
}
function makeid(length) {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;
    let counter = 0;
    while (counter < length) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
        counter += 1;
    }
    return result;
}
function func_Delete(el) {
    debugger
    const valuE = el.value.split('/')[0];
    const type = el.value.split('/')[1];
    load();
    $.ajax({
        type: 'POST',
        url: 'Delete',
        data: { id: valuE, type: type },
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
function strPad() {
    const btnDelete = document.querySelectorAll("#button");
    debugger
    btnDelete.forEach((el) => {
        el.addEventListener('click', () => { func_Delete(el) });
    })
    
       
    

    
}

loadbtn.onclick = function () {   
    load()
    let loadId = makeid(10);
    const sse = new EventSource(`/Web/Download/Upload/?id=${loadId}`);
    sse.onmessage = function (event) {
        debugger
        let totalProgress = event.data.split('/')[1];
        let actualProgress = event.data.split('/')[0];
        let procent = 0;
        if (totalProgress != 0 && actualProgress != 0) {
            procent = Math.floor(actualProgress / totalProgress * 100);
            document.querySelector("#progressProcent").innerText = procent;
            document.querySelector("#progress").style.width = `${procent}%`;
        }
        else {
            document.querySelector("#progressProcent").innerText = `${procent}%`;
            document.querySelector("#progress").style.width = `${1}%`;
            
        }
        
        

    }
    
    var formData = new FormData();    
    formData.append("file", file);
    formData.append("ad", document.querySelector("#option").value);
    formData.append("role", document.querySelector("#option").getAttribute("data-role"));
    formData.append("loadId", loadId);
    
    
    debugger
    $.ajax({
        type: 'POST',
        url: '/Web/Download/DownloadFace',
        datatype: 'json',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,

        success: function (data) {
            sse.close();
            
            $.ajax({
                type: 'POST',
                url: '/Web/Download/DeleteIdLodear',
                datatype: 'json',
                data: { id: loadId }
,
            })
           
            $.ajax({
                type: 'Get',
                url: '/Web/Download/DownloadFaceUp',
                success(data) {
                    
                    document.querySelector("#main-content").innerHTML = data;
                    debugger
                    strPad()
                    
                }
            })
            loadoff()
            console.log('Ok')
        },
        error: function () {
            sse.close();
            console.log('Bad')
        }
    });
    
}



