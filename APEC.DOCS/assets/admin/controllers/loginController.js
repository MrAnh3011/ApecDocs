
function UserLogin() {
    try {
        Metronic.blockUI('.body');
        $('#span-error').html("");
        var userName = $('#user-name').val();
        var password = $('#user-password').val();
        if (userName === '' || password === '') {
            $('#span-error').html("Bạn cần nhập đủ Tên đăng nhập và Mật khẩu!");
            Metronic.unblockUI('body');
            return;
        }
        $.ajax({
            url: "http://api.apec.com.vn/session/login",
            data: JSON.stringify({
                username: userName,
                password: password
            }),
            dataType: "json",
            type: "POST",
            contentType: 'application/json',
            success: function (response) {
                Metronic.unblockUI('body');
                if (response.status == 'Ok') {
                    var rs = response.result;
                    var loginModel = {
                        SessionKey: rs.SessionKey,
                        Username: rs.Username,
                        DisplayName: rs.DisplayName,
                        AllowDevelop: rs.AllowDevelop,
                        AllowViewAllData: rs.AllowViewAllData
                    }
                    PostSeessionUser(loginModel);
                } else {
                    $('#span-error').html("Tên đăng nhập hoặc Mật khẩu không đúng!");
                    return;
                }
                
            },
            error: function (response) {
                Metronic.unblockUI('body');
                alert(response);
            }
        });
        
    } catch (err) {
        Metronic.unblockUI('body');
        alert(err);
    }
}

function PostSeessionUser(loginModel) {
    try {
        Metronic.blockUI('.body');
        $.ajax({
            url: "/Admin/Login/PostSeessionUser",
            data: loginModel,
            dataType: "json",
            type: "POST",
    //        contentType: 'application/json',
            success: function (response) {
                Metronic.unblockUI('body');
                if (response.Status == 1) {
                    window.location.replace("/Admin/Home/Index");
                }
            },
            error: function (response) {
                Metronic.unblockUI('body');
                alert(response);
            }
        });
        
    } catch (err) {
        Metronic.unblockUI('body');
        alert(err);
    }
}
