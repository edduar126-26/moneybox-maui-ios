using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.ViewModels
{
    public partial class InsertUserViewModel : ObservableObject
    {

        #region Servicios
        private readonly IUserService _usuarioService;
        private readonly IDialogService _dialogService;
        #endregion

        #region Commands
        
        public IAsyncRelayCommand InsertaSocioCommand { get; }
        #endregion
        #region
        private RegistroSocioDto _socio;
        #endregion

        #region Constructores

        public InsertUserViewModel(RegistroSocioDto socio,IUserService userService, IDialogService dialogService)
        {
            _socio = socio;
            _usuarioService = userService;
            InsertaSocioCommand = new AsyncRelayCommand(InsertaSocio);
            _dialogService = dialogService;
        }

        #endregion

        #region Funciones



        private async Task InsertaSocio()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogService.ShowError(connection.ErrorMessage);
                        return;
                    }

                    EntRespuesta respuesta = await _usuarioService.InsertaSocio(_socio.Socio);

                    if (respuesta != null && respuesta.Estado)
                    {
                        int idSocio = Convert.ToInt32(respuesta.Resultado);
                        bool archivosSubidos = await _usuarioService.SubirArchivosAsync(idSocio, _socio.Archivos);

                        if (!archivosSubidos)
                        {
                            await _dialogService.ShowMessage("Advertencia", "Socio creado, pero los archivos no se pudieron subir.");
                        }

                        await ((NavigationPage)Application.Current.MainPage).PushAsync(
                            new FolioGeneradoRegistroSocio(respuesta.Mensaje, _socio.Socio, false, true));
                    }
                    else
                    {
                        string mensaje = string.IsNullOrWhiteSpace(respuesta?.Mensaje)
                            ? "Ocurrió un error al crear el movimiento"
                            : respuesta.Mensaje;

                        await _dialogService.ShowError( mensaje);
                        Console.WriteLine("Error de API: " + mensaje);
                    }
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepción: " + ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }


        #endregion



    }
}
