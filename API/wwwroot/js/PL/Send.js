const btn_Del = document.querySelectorAll('#button-delete')
const btn_Save = document.querySelector('#save')


btn_Delete_Update();
function btn_Delete_Update() {
    document.querySelectorAll('#button-delete').forEach((el) => {

        el.addEventListener('click', (e) => {
            debugger
            let id = el.value.split('/')[0];
            let type = el.value.split('/')[1];

            
            let url
            if (type == "Ad") {
                url = '/Web/PlayLists/PlaylistsFace/DeleteAd'
            }
            else {
                url ='/Web/PlayLists/PlaylistsFace/DeleteMed'
            }
            
            $.ajax({
                url: url,
                type: "DELETE",
                data: { id: id},


                success: function (data) {
                    debugger;
                    $.ajax({
                        url: `/Web/PlayLists/PlaylistsFace/Update`,
                        dataType: 'html',
                        success: function (data) {

                            $('#playlist-content').html(data); 
                            alert("the playlist was delete");
                            btn_Delete_Update();

                        }
                    })






                },
                error: function () {
                    alert("Произошел сбой");
                }
            });
        })
    })

}






btn_Save.onclick = function () {
    debugger
    const name = document.querySelector('#playlist-name').value;    
    const type = document.querySelector('#playlist-type').value;
    const role = document.querySelector('#playlist-type').getAttribute('data-role')

    let mock1 = true;    
    if (name != "") {
        $.ajax({
            url: '/Web/PlayLists/PlaylistsFace/CreatePlaylist',
            type: "POST",
            data: { name: name, type: type, role: role},


            success: function (data) {
                debugger;
                $.ajax({
                    url: `/Web/PlayLists/PlaylistsFace/Update`,
                    dataType: 'html',
                    success: function (data) {
                        
                        $('#playlist-content').html(data);

                        overlay.style.opacity = 0;
                        overlay.style.visibility = 'hidden';
                        alert("Playlist was create")
                        btn_Delete_Update();
                    }
                })






            },
            error: function () {
                alert("Error");
            }
        });
        

    }
    else {
        alert("You did not correctly filled in web form")
    }
    debugger
    

}