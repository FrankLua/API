
const message = document.getElementById('message');

if (message.title != undefined) {
    showbox(message.title)
}

function showbox(str) {
    
    
    console.log(message)
    message.innerHTML = (writer(str));
    message.classList.add('message-state-1');
    setTimeout(function () {
        console.log(message.classList);
        message.classList.remove('message-state-1');
        message.classList.add('message-state-2');
    },
        3000);

    function writer(str) {
        const answer = `<p>${str}</p>`
        return answer;
    }
}



