using Money_Box.IService;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Media;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Money_Box.Services
{
    public class ServicioFotoService : IServicioFoto
    {
        private readonly HttpClient _httpClient;

        public ServicioFotoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FileResult> SeleccionarFotoAsync()
        {
            var permiso = await Permissions.RequestAsync<Permissions.Photos>();
            if (permiso != PermissionStatus.Granted)
                return null;

            try
            {
                return await MediaPicker.Default.PickPhotoAsync();
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<FileResult> CapturarFotoAsync()
        {
            var permiso = await Permissions.RequestAsync<Permissions.Camera>();
            if (permiso != PermissionStatus.Granted)
                return null;

            if (!MediaPicker.Default.IsCaptureSupported)
                return null;

            try
            {
                return await MediaPicker.Default.CapturePhotoAsync();
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> SubirFotoAsync(FileResult file, string endpoint)
        {
            if (file == null || string.IsNullOrEmpty(endpoint))
                return null;

            using var stream = await file.OpenReadAsync();
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        
            content.Add(fileContent, "foto", file.FileName);

            return await _httpClient.PostAsync(endpoint, content);
        }
    }
}
