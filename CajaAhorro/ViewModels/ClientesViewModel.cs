using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.IService;
using Money_Box.Models;
using System.Collections.ObjectModel;

namespace Money_Box.ViewModels
{
    public partial class ClientesViewModel : ObservableObject
    {
        private readonly IAppDatabase _db;
        private readonly IDatabasePathProvider _pathProvider;

        public event EventHandler NavegarAcceso;

        public ObservableCollection<Clientes> ListaClientes { get; } = new();

        public IAsyncRelayCommand ValidaPrimerRegistroCommand { get; private set; }

        public ClientesViewModel(
            IAppDatabase db,
            IDatabasePathProvider pathProvider)
        {
            _db = db;
            _pathProvider = pathProvider;
        }

        public void Inicializar()
        {
            ValidaPrimerRegistroCommand = new AsyncRelayCommand(ValidaPrimerRegistro);
        }

        private async Task ValidaPrimerRegistro()
        {
            try
            {
                var clientes = await _db.GetClientesAsync();

                if (clientes.Count == 0)
                {
                    var cliente = new Clientes
                    {
                        IdCliente = 1,
                        NombreCliente = "Caja Mancomunada de Empleados  Alupeco A. C. ",
                        NombreContrato = "Caja Mancomunada de Empleados  Alupeco A. C. ",
                        NombrePagare = "Caja Mancomunada de Empleados  Alupeco A. C. ",
                        CLABECuentaBancaria = "036180500360847012",
                        RazonSocial = "Caja Mancomunada de Empleados  Alupeco A. C. ",
                        RFCCliente = "CAN161208C96",
                        Calle = "AV. PONIENTE 140",
                        NumeroExterior = "720",
                        NumeroInterior = "PLANTA BAJA",
                        Colonia = "INDUSTRIAL VALLEJO",
                        CodigoPostal = "02300",
                        IdEstado = 10,
                        AbreviaturaEstado = "CDMX ",
                        NombreEstado = "CIUDAD DE MÉXICO",
                        IdPoblacion = 268,
                        NombrePoblacion = "AZCAPOTZALCO",
                        NoTelefono = "+52 (55) 5328 3326",
                        RepresentanteLegal01 = "ARTURO SALTO GONZÁLEZ",
                        RepresentanteLegal02 = "HÉCTOR RODRIGUEZ GUADARRAMA",
                        RepresentanteLegal03 = "RAQUEL MARTINEZ TLATILPA",
                        Estatus = true
                    };

                    await _db.InsertClienteAsync(cliente);
                }

                NavegarAcceso?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Error!", "Ocurrió un error, favor de reportar a sistemas", "OK");
            }
        }
    }
}
