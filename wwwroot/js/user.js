function Login() {
    //let acc = $("accountLogin").val()
    //let pass = $("passwordLogin").val()
    var acc = document.getElementById("accountLogin").value;
    var pass = document.getElementById("passwordLogin").value;
    var data = new FormData();
    data.append("UserName", acc)
    data.append("Password", pass)
    data.append("RememberMe", true)
    fetch("/Account/LoginUser", {
        method: "POST",
        body: data
    }).then(res => res.json())
        .then(data => {
            if (data.statusCode == 200) {
                document.getElementById("errorLogin").innerHTML = ""
                window.location.reload()
            } else {
                document.getElementById("errorLogin").innerHTML = data.message
            }
        })
        .catch(err => {
            console.log("err::", err)
            
        })
}

function Register() {
    //let acc = $("accountRegister").val()
    //let pass = $("passtRegister").val()
    //let repass = $("rePasstRegister").val()
    var acc = document.getElementById("accountRegister").value;
    var pass = document.getElementById("passtRegister").value;
    var repass = document.getElementById("rePasstRegister").value;
    var data = new FormData();
    data.append("UserName", acc)
    data.append("PasswordHash", pass)
    data.append("ConfirmPassword", repass)

    fetch("/Account/Register", {
        method: "POST",
        body: data
    }).then(res => {

        window.location.href = '/'
    })
        .catch(err => {
            console.log("err::", err)

        })
}

function Logout() {
    fetch("/Account/Logout", {
        method: "POST"
    })
        .then(res => {
            window.location.href = "/"
        }).
        catch(err => {
            window.location.href = "/"
        })
}