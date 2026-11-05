using System;
using Microsoft.Data.Sqlite;

public class ConexionBD
{
    string cadenaConexion = "Data Source=BaseDeDatosHabitos.db";

    public void CrearTabla()
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();

            var comandoTabla = conexion.CreateCommand();
            comandoTabla.CommandText =
                @"CREATE TABLE IF NOT EXISTS habitos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Fecha TEXT,
                    Cantidad INTEGER
                )";

            comandoTabla.ExecuteNonQuery();
        }
    }

    public void Insertar(string nombre, string fecha, int cantidad)
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            var comando = conexion.CreateCommand();
            comando.CommandText = $"INSERT INTO habitos (Nombre, Fecha, Cantidad) VALUES ('{nombre}', '{fecha}', {cantidad})";
            comando.ExecuteNonQuery();
        }
    }

    public void VerTodos()
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * FROM habitos";

            using (SqliteDataReader lector = comando.ExecuteReader())
            {
                if (lector.HasRows)
                {
                    Console.WriteLine("\n--- TUS REGISTROS ---");
                    while (lector.Read())
                    {
                        Console.WriteLine($"ID: {lector.GetInt32(0)} | Hábito: {lector.GetString(1)} | Fecha: {lector.GetString(2)} | Cantidad: {lector.GetInt32(3)}");
                    }
                    Console.WriteLine("---------------------\n");
                }
                else
                {
                    Console.WriteLine("\nNo hay ningún registro guardado todavía.\n");
                }
            }
        }
    }

    public void Borrar(int id)
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            var comando = conexion.CreateCommand();
            comando.CommandText = $"DELETE FROM habitos WHERE Id = {id}";

            int filasAfectadas = comando.ExecuteNonQuery();

            if (filasAfectadas == 0)
            {
                Console.WriteLine($"\nNo se encontró ningún registro con el ID {id}.\n");
            }
            else
            {
                Console.WriteLine($"\n¡Registro con ID {id} borrado con éxito!\n");
            }
        }
    }

    public void Actualizar(int id, string nuevoNombre, string nuevaFecha, int nuevaCantidad)
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            var comando = conexion.CreateCommand();
            comando.CommandText = $"UPDATE habitos SET Nombre = '{nuevoNombre}', Fecha = '{nuevaFecha}', Cantidad = {nuevaCantidad} WHERE Id = {id}";

            int filasAfectadas = comando.ExecuteNonQuery();

            if (filasAfectadas == 0)
            {
                Console.WriteLine($"\nNo se encontró ningún registro con el ID {id}.\n");
            }
            else
            {
                Console.WriteLine($"\n¡Registro actualizado con éxito!\n");
            }
        }
    }

    public List<Habito> ObtenerTodos()
    {
        var lista = new List<Habito>();
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT Id, Nombre, Fecha, Cantidad FROM habitos";
            using (var lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    lista.Add(new Habito
                    {
                        Id = lector.GetInt32(0),
                        Nombre = lector.GetString(1),
                        Fecha = lector.GetString(2),
                        Cantidad = lector.GetInt32(3)
                    });
                }
            }
        }
        return lista;
    }

}