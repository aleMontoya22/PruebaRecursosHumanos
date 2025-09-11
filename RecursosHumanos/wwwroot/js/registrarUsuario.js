$(document).ready(function () {
    $('#formRegistroUsuario').submit(function (e) {
        e.preventDefault();

        const formData = {
            Nombre: $('#Nombre').val(),
            Apellido: $('#Apellido').val(),
            Email: $('#Email').val(),
            Contraseña: $('#Contrasena').val(), 
            ContrasenaConfirmacion: $('#ContrasenaConfirmacion').val()
        };

        // Validar contraseñas
        if (formData.Contraseña !== formData.ContrasenaConfirmacion) {
            $('#mensajeRegistro').html('<div class="alert alert-danger">Las contraseñas no coinciden.</div>');
            return;
        }

        console.log(formData);

        $.ajax({
            url: urlGuardarUsuario,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                $('#mensajeRegistro').html('<div class="alert alert-success">Usuario registrado correctamente.</div>');
                $('#formRegistroUsuario')[0].reset();
                //window.location.href = urlLogin;
            },
            error: function (xhr) {
                $('#mensajeRegistro').html('<div class="alert alert-danger">Error al registrar el usuario.</div>');
            }
        });
    });
});