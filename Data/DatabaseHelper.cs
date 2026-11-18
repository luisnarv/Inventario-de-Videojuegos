using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace InventarioVideojuegos.Data;

public static class DatabaseHelper
{
    private static readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "videojuegos.db");

    public static string ConnectionString => $"Data Source={dbPath}";

    public static void InicializarBaseDatos()
    {
        using var conexion = new SqliteConnection(ConnectionString);
        conexion.Open();

        var queryTabla = @"
            CREATE TABLE IF NOT EXISTS videojuegos (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                titulo TEXT NOT NULL,
                plataforma TEXT NOT NULL,
                genero TEXT NOT NULL,
                precio REAL NOT NULL,
                horas INTEGER NOT NULL,
                estado TEXT NOT NULL
            );
        ";

        using (var comando = new SqliteCommand(queryTabla, conexion))
        {
            comando.ExecuteNonQuery();
        }

        var queryContar = "SELECT COUNT(*) FROM videojuegos;";
        using (var comandoContar = new SqliteCommand(queryContar, conexion))
        {
            long cantidad = (long)(comandoContar.ExecuteScalar() ?? 0L);
            if (cantidad == 0)
            {
                var queryInsertInicial = @"
                    INSERT INTO videojuegos (titulo, plataforma, genero, precio, horas, estado)
                    VALUES 
                    ('The Legend of Zelda: Tears of the Kingdom', 'Nintendo Switch', 'Aventura', 69.99, 85, 'Completado'),
                    ('God of War Ragnarok', 'PlayStation 5', 'Accion', 59.99, 42, 'Completado'),
                    ('Halo Infinite', 'Xbox Series X', 'Disparos', 49.99, 15, 'Jugando'),
                    ('Cyberpunk 2077', 'PC', 'RPG', 39.99, 60, 'Pendiente');
                ";
                using var comandoInsert = new SqliteCommand(queryInsertInicial, conexion);
                comandoInsert.ExecuteNonQuery();
            }
        }
    }
}
