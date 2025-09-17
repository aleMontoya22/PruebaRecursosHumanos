$(document).ready(function () {
    $('#formIngresoUsuario').submit(function (e) {

        e.preventDefault();

        const email = $('#exampleInputEmail').val();
        const password = $('#exampleInputPassword').val();

        if (!email || !password) {
            alert('Por favor completa los campos.');
            return;
        }

        $.ajax({
            url: urlGuardarUsuario,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Email: email, Contraseña: password }),
            success: function (response) {
                if (response.success) {
                    window.location.href = urlLogin;
                } else {
                    alert(response.message || 'Credenciales inválidas');
                }
            },
            error: function () {
                alert('Error al intentar iniciar sesión.');
            }
        });
    });
});