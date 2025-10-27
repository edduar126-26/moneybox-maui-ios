using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Money_Box.ViewModels
{
    public partial class FirmaContratoNormativoViewModel: ObservableObject
    {
        private readonly int _idusuario;
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;


        [ObservableProperty]
        private string firmaBase64;

        public IAsyncRelayCommand FirmaCommand { get; }

        public FirmaContratoNormativoViewModel(int IdUsuario, IUserService usuarioService,IDialogService dialogoService)
        {
            _idusuario = IdUsuario;
            _userService = usuarioService;
            _dialogService = dialogoService;

            FirmaCommand = new AsyncRelayCommand(EnviarFirmaAsync);
        }


        public async Task<bool> EnviarFirmaAsync()
        {
            bool exito = await _userService.EnviarFirmaAsyncN(FirmaBase64, _idusuario); 

            if (exito)
                await _dialogService.ShowMessage("Exito","Firma enviada correctamente");
            else
                await _dialogService.ShowError("No se pudo enviar la firma");

            return exito;
        }

        public async Task<bool> ContratoFirmado(int IdUsuario)
        {
            var userService = _userService;
            return await userService.YaContratoCFirmado(IdUsuario);
        }
    }
}
