using Money_Box.IService;
using Money_Box.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class DocumentosService : IDocumentosService
    {
        public async Task<DocumentoSeleccionado?> TomarFotoAsync(string tipo, Func<string, Task> showAlert)
        {
            if (!await PermisosHelper.EnsureCameraAsync(showAlert)) return null;

            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null) { await showAlert("Cancelado: no se tomó ninguna foto."); return null; }

                var fileName = photo.FileName;
                var ext = Path.GetExtension(fileName);
                if (string.IsNullOrWhiteSpace(ext)) ext = ".jpg";
                ext = ext.ToLowerInvariant();

                string Slug(string s)
                {
                    s = s.Normalize(NormalizationForm.FormD);
                    var chars = s.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark);
                    var clean = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);
                    foreach (var ch in Path.GetInvalidFileNameChars()) clean = clean.Replace(ch, '_');
                    return clean.Replace(' ', '_').Replace('/', '-').Replace('\\', '-');
                }

                var baseName = Slug(Path.GetFileNameWithoutExtension(fileName));
                var tipoSlug = Slug(tipo);

              
                byte[] bytes;
                using (var stream = await photo.OpenReadAsync())
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }

             
                bytes = DownscaleJpeg(bytes, maxSize: 1280, quality: 80);

             
                const int maxBytes = 800 * 1024;
                if (bytes.Length > maxBytes)
                {
                    
                    bytes = DownscaleJpeg(bytes, maxSize: 1280, quality: 70);
                }

               
                ext = ".jpg";

                return new DocumentoSeleccionado
                {
                    NombreArchivo = $"{tipoSlug}_{baseName}{ext}",
                    Tipo = tipo,
                    Archivo = bytes,
                    AbrirStreamAsync = () => Task.FromResult<Stream>(new MemoryStream(bytes))
                };
            }
            catch (FeatureNotSupportedException) { await showAlert("La cámara no está soportada en este dispositivo."); }
            catch (PermissionException) { await showAlert("Permiso de cámara denegado."); }
            catch (Exception ex) { await showAlert($"No se pudo tomar la foto:\n{ex.Message}"); }
            return null;
        }



        public async Task<DocumentoSeleccionado?> SeleccionarArchivoAsync(string tipo, Func<string, Task> showAlert)
        {
           
            if (!await PermisosHelper.EnsureMediaLibraryAsync(showAlert)) return null;

            try
            {
              
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = $"Selecciona {tipo}",
                    FileTypes = FilePickerFileType.Images 
                });

                if (result == null) { await showAlert("Cancelado: no se seleccionó ningún archivo."); return null; }

                var fileName = result.FileName;
                using var stream = await result.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var bytes = ms.ToArray();

                return new DocumentoSeleccionado
                {
                    NombreArchivo = $"{tipo}_{fileName}",
                    Tipo = tipo,
                    Archivo = bytes,
                    AbrirStreamAsync = () => Task.FromResult<Stream>(new MemoryStream(bytes))
                };
            }
            catch (Exception ex)
            {
                await showAlert($"No se pudo seleccionar el archivo:\n{ex.Message}");
                return null;
            }
        }


        public static byte[] DownscaleJpeg(byte[] input, int maxSize = 1280, int quality = 80)
        {
            using var bitmap = SKBitmap.Decode(input);
            if (bitmap == null) return input;

            int w = bitmap.Width, h = bitmap.Height;
            float scale = Math.Min((float)maxSize / w, (float)maxSize / h);
         
            if (scale >= 1f)
            {
                using var img0 = SKImage.FromBitmap(bitmap);
                using var data0 = img0.Encode(SKEncodedImageFormat.Jpeg, quality);
                return data0.ToArray();
            }

            int newW = (int)(w * scale);
            int newH = (int)(h * scale);

          
            var sampling = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None);
            using var resized = bitmap.Resize(new SKImageInfo(newW, newH), sampling);

            using var img = SKImage.FromBitmap(resized ?? bitmap);
            using var data = img.Encode(SKEncodedImageFormat.Jpeg, quality);
            return data.ToArray();
        }

    }
}
