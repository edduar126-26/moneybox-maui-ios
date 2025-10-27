using CommunityToolkit.Mvvm.ComponentModel;
using Money_Box.IService;
using Money_Box.Models.Local;
using Money_Box.Helpers;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.ViewModels
{
    public partial class DocumentosSocioActualizacionViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;

        public DocumentosSocioActualizacionViewModel(IUserService userService, IDialogService dialogService)
        {
            _userService = userService;
            _dialogService = dialogService;
        }

        public async Task<bool> SubirArchivoUnicoAsync(int idSocio, DocumentoSeleccionado archivo)
        {
            var resultado = await _userService.SubirArchivoUnicoAsync(idSocio, archivo);

            if (resultado)
            {
                await _dialogService.ShowMessage("Éxito", $"Documento {archivo.Tipo} actualizado correctamente.");
                return true;
            }

            await _dialogService.ShowError($"No se pudo subir el documento {archivo.Tipo}.");
            return false;
        }
    }
}
