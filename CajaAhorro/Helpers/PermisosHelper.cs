using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;

public static class PermisosHelper
{
  
    public static async Task<bool> EnsureCameraAsync(Func<string, Task>? showAlert = null)
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            if (Permissions.ShouldShowRationale<Permissions.Camera>() && showAlert != null)
                await showAlert("Necesitamos permiso de cámara para tomar la foto.");
            status = await Permissions.RequestAsync<Permissions.Camera>();
        }
        if (status == PermissionStatus.Granted) return true;
        if (showAlert != null) await showAlert("Sin permiso de cámara. Ve a Ajustes y habilítalo para continuar.");
        return false;
    }


    public static Task<bool> EnsureMediaLibraryAsync(Func<string, Task>? showAlert = null)
    {
        return Task.FromResult(true);
    }

    public static async Task AbrirAjustesAsync() => AppInfo.ShowSettingsUI();

}


