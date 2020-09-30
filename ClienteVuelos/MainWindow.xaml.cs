using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClienteVuelos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = dv;
            cv.Get();
            cv.AlHaberMovimientos += Cv_AlHaberMovimientos;
            cmbEstados.Items.Add("A TIEMPO");
            cmbEstados.Items.Add("ABORDANDO");
            cmbEstados.Items.Add("RETRASADO");
            cmbEstados.Items.Add("CANCELADO");
            cronometro.Interval = TimeSpan.FromSeconds(2);
            cronometro.Tick += Cronometro_Tick;
            cronometro.Start();
        }

        private void Cronometro_Tick(object sender, EventArgs e)
        {
            cv.Get();
        }

        private void Cv_AlHaberMovimientos()
        {
            lsvVuelos.ItemsSource = cv.Modelo;
        }
        DispatcherTimer cronometro = new DispatcherTimer();
        DatosVuelos dv = new DatosVuelos();
        DatosVuelos dv2 = new DatosVuelos();
        ClienteVuelo cv = new ClienteVuelo();
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cv.Agregar(dv2);
                btnAgregar.IsEnabled = false;
                txtVuelo.IsEnabled = txtHora.IsEnabled = txtDestino.IsEnabled = false;
                txtDestino.Text = txtHora.Text = txtVuelo.Text = cmbEstados.Text = "";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dv.Hora = txtHora.Text;
                dv.Vuelo = txtVuelo.Text;
                dv.Destino = txtDestino.Text;
                dv.Estado = cmbEstados.Text;
                cv.Editar(dv);
                cv.Get();
                cronometro.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (lsvVuelos.SelectedIndex != -1)
            {
            try
            {
                    DatosVuelos dvs = new DatosVuelos();
                    dvs=lsvVuelos.SelectedItem as DatosVuelos;
                    if (MessageBox.Show($"El vuelo {dvs.Vuelo} está a punto de ser eliminado. ¿Desea continuar?", "Atencion",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        cv.Eliminar(dv);
                        txtDestino.Text = txtHora.Text = txtVuelo.Text = cmbEstados.Text = "";
                        cv.Get();
                        cronometro.Start();
                    }  
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            }
            else
            {
                MessageBox.Show("Es necesario que se seleccione un vuelo para ser eliminado.", "Atencion", MessageBoxButton.OK);
            }
        }
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = dv2;
            btnAgregar.IsEnabled = true;
            txtDestino.IsEnabled = true;
            txtHora.IsEnabled = true;
            txtVuelo.IsEnabled = true;
        }

        private void lsvVuelos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvVuelos.SelectedItem != null)
            {
                cronometro.Stop();
                dv = lsvVuelos.SelectedItem as DatosVuelos;
                txtHora.Text = dv.Hora;
                txtDestino.Text = dv.Destino;
                txtVuelo.Text = dv.Vuelo;
                cmbEstados.Text = dv.Estado;
            }
        }
    }
}
