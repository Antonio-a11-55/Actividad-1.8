using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Actividad_1._6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConexionBD bd = new ConexionBD();
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                bd.CrearTabla();
            }catch(Exception ex)
            {
                MessageBox.Show($"Error al iniciar la base de datos: {ex.Message}");
            }
            CargarDatos();
        }

        // Insertar
        private void BtnInsertar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text.Trim();

            if (String.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nomnbre no puede estar vacío.");
                return;
            }
            if (selFecha.SelectedDate == null)
            {
                MessageBox.Show("Selecciona una fecha.");
                return;
            }
            // Si el usuario introduce un numero que no es entero o negativo muestra error y sale
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad < 0)
            {
                MessageBox.Show("Introduce una cantidad entera y positiva.");
                return;
            }

            string fecha = selFecha.SelectedDate.Value.ToString("dd-MM-yy");

            try
            {
                bd.Insertar(nombre, fecha, cantidad);
                MessageBox.Show("Hábito guardado con éxito.");
                LimpiarFormulario();
                CargarDatos();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error al insertar: {ex.Message}");
            }
        }

        //Ver todos
        private void BtnVerTodos_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }

        //Borrar
        private void btnBorrar_click(Object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(txtId.Text, out int id) || id <= 0)
            {
                MessageBox.Show("Introduce un ID válido.");
                return;
            }

            var confirm = MessageBox.Show($"¿Seguro que quieres borrar el registro con ID {id}?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                bd.Borrar(id);
                CargarDatos();
            }catch(Exception ex)
            {
                MessageBox.Show($"Error al borrar: {ex.Message}");
            }
        }

        //Actualizar 
        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(txtId.Text, out int id) || id <= 0)
            {
                MessageBox.Show("Introduce un ID válido");
                return;
            }
            string nuevoNombre = txtNombre.Text.Trim();
            if (String.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show("El nombre no puede estar vacío.");
                return;
            }
            if(selFecha.SelectedDate == null)
            {
                MessageBox.Show("Introdce una fecha.");
                return;
            }
            if(!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad < 0)
            {
                MessageBox.Show("Introduce una cantidad entera y positiva.");
                return;
            }

            string nuevaFecha = selFecha.SelectedDate.Value.ToString("dd-MM-yy");

            try
            {
                bd.Actualizar(id, nuevoNombre, nuevaFecha, cantidad);
                CargarDatos();
            }catch(Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}");
            }
        }

        private void CargarDatos()
        {
            try {
                List<Habito> lista = bd.ObtenerTodos();
                tablaHabitos.ItemsSource = lista;
            }catch(Exception ex)
            {
                MessageBox.Show($"Error al leer la base de datos: {ex.Message}");
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            selFecha.SelectedDate = null;
            txtCantidad.Clear();
            txtId.Clear();
        }



        // ESC cierra la app entera
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        // Doble clic en una fila → abre ventana de detalle
        private void tablaHabitos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tablaHabitos.SelectedItem is Habito habito)
            {
                var detalle = new DetalleHabito(habito);
                detalle.Owner = this;       // que salga centrada sobre la principal
                detalle.ShowDialog();       // modal: no vuelves hasta cerrarla
            }
        }

    }
}