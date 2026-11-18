using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using InventarioVideojuegos.Models;

namespace InventarioVideojuegos.Data;

public class VideojuegoDAO
{
    public void Insertar(Videojuego videojuego)
    {
        using var conexion = new SqliteConnection(DatabaseHelper.ConnectionString);
        conexion.Open();

        var query = @"
            INSERT INTO videojuegos (titulo, plataforma, genero, precio, horas, estado)
            VALUES (@titulo, @plataforma, @genero, @precio, @horas, @estado);
        ";

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.AddWithValue("@titulo", videojuego.Titulo);
        comando.Parameters.AddWithValue("@plataforma", videojuego.Plataforma);
        comando.Parameters.AddWithValue("@genero", videojuego.Genero);
        comando.Parameters.AddWithValue("@precio", (double)videojuego.Precio);
        comando.Parameters.AddWithValue("@horas", videojuego.HorasJugadas);
        comando.Parameters.AddWithValue("@estado", videojuego.Estado);

        comando.ExecuteNonQuery();
    }

    public List<Videojuego> ObtenerTodos()
    {
        var lista = new List<Videojuego>();

        using var conexion = new SqliteConnection(DatabaseHelper.ConnectionString);
        conexion.Open();

        var query = "SELECT id, titulo, plataforma, genero, precio, horas, estado FROM videojuegos ORDER BY id DESC;";

        using var comando = new SqliteCommand(query, conexion);
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            var videojuego = new Videojuego
            {
                Id = lector.GetInt32(0),
                Titulo = lector.GetString(1),
                Plataforma = lector.GetString(2),
                Genero = lector.GetString(3),
                Precio = Convert.ToDecimal(lector.GetDouble(4)),
                HorasJugadas = lector.GetInt32(5),
                Estado = lector.GetString(6)
            };
            lista.Add(videojuego);
        }

        return lista;
    }

    public Videojuego? ObtenerPorId(int id)
    {
        using var conexion = new SqliteConnection(DatabaseHelper.ConnectionString);
        conexion.Open();

        var query = "SELECT id, titulo, plataforma, genero, precio, horas, estado FROM videojuegos WHERE id = @id;";

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.AddWithValue("@id", id);

        using var lector = comando.ExecuteReader();
        if (lector.Read())
        {
            return new Videojuego
            {
                Id = lector.GetInt32(0),
                Titulo = lector.GetString(1),
                Plataforma = lector.GetString(2),
                Genero = lector.GetString(3),
                Precio = Convert.ToDecimal(lector.GetDouble(4)),
                HorasJugadas = lector.GetInt32(5),
                Estado = lector.GetString(6)
            };
        }

        return null;
    }

    public void Actualizar(Videojuego videojuego)
    {
        using var conexion = new SqliteConnection(DatabaseHelper.ConnectionString);
        conexion.Open();

        var query = @"
            UPDATE videojuegos 
            SET titulo = @titulo,
                plataforma = @plataforma,
                genero = @genero,
                precio = @precio,
                horas = @horas,
                estado = @estado
            WHERE id = @id;
        ";

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.AddWithValue("@titulo", videojuego.Titulo);
        comando.Parameters.AddWithValue("@plataforma", videojuego.Plataforma);
        comando.Parameters.AddWithValue("@genero", videojuego.Genero);
        comando.Parameters.AddWithValue("@precio", (double)videojuego.Precio);
        comando.Parameters.AddWithValue("@horas", videojuego.HorasJugadas);
        comando.Parameters.AddWithValue("@estado", videojuego.Estado);
        comando.Parameters.AddWithValue("@id", videojuego.Id);

        comando.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var conexion = new SqliteConnection(DatabaseHelper.ConnectionString);
        conexion.Open();

        var query = "DELETE FROM videojuegos WHERE id = @id;";

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }
}
