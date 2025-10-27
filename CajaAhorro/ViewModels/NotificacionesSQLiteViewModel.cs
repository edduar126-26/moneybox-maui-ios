using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.Services;
using Money_Box.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;



namespace Money_Box.ViewModels
{
    public partial class NotificacionesSQLiteViewModel : ObservableObject
    {
        private readonly IAppDatabase _notificacionDB;
        private readonly IUserService _userService;

        [ObservableProperty]
        private ObservableCollection<Notifier> notificacionesPendientes = new();

        public IRelayCommand<Notifier> MarcarComoResueltaCommand { get; }
        public IRelayCommand<int> VerPagareCommand { get; }
        public IRelayCommand<Notifier> BorrarNotificacionCommand { get; }

        public NotificacionesSQLiteViewModel(IAppDatabase notificacionDB, IUserService userService)
        {
            _notificacionDB = notificacionDB;
            _userService = userService;

            MarcarComoResueltaCommand = new AsyncRelayCommand<Notifier>(MarcarComoResuelta);
            VerPagareCommand = new AsyncRelayCommand<int>(VerPagare);
            BorrarNotificacionCommand = new AsyncRelayCommand<Notifier>(BorrarNotificacion);
        }

        public async Task CargarNotificaciones()
        {
            NotificacionesPendientes.Clear();
            var lista = await _notificacionDB.GetNotificacionesPendientesAsync();
            foreach (var item in lista)
                NotificacionesPendientes.Add(item);
        }

        public async Task SincronizarNotificacionesDesdeApi(int idSocio)
        {
            var apiNotificaciones = await _userService.BuscarNotificaciones(idSocio);

            if (apiNotificaciones != null && apiNotificaciones.Any())
            {
                foreach (var noti in apiNotificaciones)
                {
                    var existe = await _notificacionDB.GetNotificacionPorIdAsync(noti.IdNotificacion);
                    if (existe == null)
                    {
                        await _notificacionDB.SaveNotificacionAsync(new Notifier
                        {
                            IdNotificacion = noti.IdNotificacion,
                            Asunto = noti.Asunto,
                            Mensaje = noti.Mensaje,
                            Resuelta = false,
                            FolioSolicitud = noti.FolioSolicitud
                        });
                    }
                }
            }

            await CargarNotificaciones();
        }

        public async Task MarcarComoEnviadas(int idSocio)
        {
            var notisPendientes = await _notificacionDB.GetNotificacionesPendientesAsync();

            if (notisPendientes?.Any() == true)
            {
                var notisActualizar = notisPendientes.Select(n => new moNotificacion
                {
                    IdNotificacion = n.IdNotificacion,
                    Asunto = n.Asunto,
                    Mensaje = n.Mensaje,
                    IdDestinatario = n.IdDestinatario,
                    Enviado = true
                }).ToList();

                await _userService.ActualizarNotificacionesAsync(idSocio, notisActualizar);
            }
        }

        private async Task MarcarComoResuelta(Notifier noti)
        {
            await _notificacionDB.MarcarNotificacionResueltaAsync(noti.IdNotificacion);
            NotificacionesPendientes.Remove(noti);
        }

        private async Task VerPagare(int folioSolicitud)
        {
            if (folioSolicitud > 0)
                await Application.Current.MainPage.Navigation.PushAsync(new FirmarPagare(folioSolicitud.ToString()));
        }

        private async Task BorrarNotificacion(Notifier noti)
        {
            if (noti != null)
            {
                bool confirmar = await Application.Current.MainPage.DisplayAlert("Confirmar", $"¿Borrar la notificación \"{noti.Asunto}\"?", "Sí", "No");

                if (confirmar)
                {
                    await _notificacionDB.DeleteNotificacionAsync(noti);
                    NotificacionesPendientes.Remove(noti);
                }
            }
        }
    }
}
