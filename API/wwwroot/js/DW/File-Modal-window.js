
const overlay = document.getElementById('modal-overlay')
const btns = document.querySelectorAll('.btn')
const loadbtn = document.getElementById('load');
const loadingoverlay = document.getElementById('loading');

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

console.log(loadbtn)
loadbtn.onclick = function () {
    load();
}