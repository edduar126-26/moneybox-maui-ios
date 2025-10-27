using Money_Box.IService;
using Money_Box.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class FilePickerService : IFilePickerService
    {
        public async Task<DocumentoSeleccionado?> PickFileAsync(string tipo)
        {
            
            var customTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.Android, new[] { "image/*", "application/pdf" } },
            { DevicePlatform.iOS,     new[] { "public.image", "com.adobe.pdf" } },
            { DevicePlatform.WinUI,   new[] { ".jpg",".jpeg",".png",".pdf" } }
        });

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = $"Selecciona archivo ({tipo})",
                FileTypes = customTypes
            });

            if (result == null) return null;

            using var stream = await result.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();

            return new DocumentoSeleccionado
            {
                NombreArchivo = result.FileName,
                Tipo = tipo,
                Archivo = bytes,
                AbrirStreamAsync = () => Task.FromResult<Stream>(new MemoryStream(bytes))
            };
        }
    }
}
