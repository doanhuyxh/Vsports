function Login() {
    let acc = $("accountLogin").val()
    let pass = $("passwordLogin").val()
    let data = new FormData();
    data.append("UserName", acc)
    data.append("Password", pass)
    data.append("RememberMe", true)
    fetch("/Account/Login", {
        method: "POST",
        body: data
    }).then(res => res.json())
        .then(data => {
            console.log(data)
        })
        .catch(err => {
            console.log("err::", err)
            document.getElementById("errorLogin").classList.remove("d-none")
        })
}

function Register() {
    let acc = $("accountRegister").val()
    let pass = $("passtRegister").val()
    let repass = $("rePasstRegister").val()
    let data = new FormData();
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