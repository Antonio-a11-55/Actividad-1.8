using System.Windows;
using System.Windows.Input;

namespace Actividad_1._6
{
    public partial class DetalleHabito : Window
    {
        public DetalleHabito(Habito habito)
        {
            InitializeComponent();

            txtId.Text = habito.Id.ToString();
            txtNombre.Text = habito.Nombre;
            txtFecha.Text = habito.Fecha;
            txtCantidad.Text = habito.Cantidad.ToString();
        }

        // ESC cierra esta ventana y vuelve a la anterior
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}