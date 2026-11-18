using System;
using System.Collections.Generic;
using InventarioVideojuegos.Data;
using InventarioVideojuegos.Models;

namespace InventarioVideojuegos.Services;

public class VideojuegoServicio
{
    private readonly VideojuegoDAO dao;

    public VideojuegoServicio()
    {
        dao = new VideojuegoDAO();
    }

    public void Registrar(Videojuego videojuego)
    {
        ValidarVideojuego(videojuego);
        dao.Insertar(videojuego);
    }

    public List<Videojuego> ListarTodos()
    {
        return dao.ObtenerTodos();
    }

    public void Modificar(Videojuego videojuego)
    {
        if (videojuego.Id <= 0)
        {
            throw new ArgumentException("El identificador del videojuego no es valido para actualizar.");
        }

        ValidarVideojuego(videojuego);
        dao.Actualizar(videojuego);
    }

    public void Eliminar(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El identificador del videojuego no es valido para eliminar.");
        }

        dao.Eliminar(id);
    }

    private void ValidarVideojuego(Videojuego videojuego)
    {
        if (string.IsNullOrWhiteSpace(videojuego.Titulo))
        {
            throw new ArgumentException("El titulo del videojuego es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(videojuego.Plataforma))
        {
            throw new ArgumentException("La plataforma del videojuego es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(videojuego.Genero))
        {
            throw new ArgumentException("El genero del videojuego es obligatorio.");
        }

        if (videojuego.Precio < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo.");
        }

        if (videojuego.HorasJugadas < 0)
        {
            throw new ArgumentException("Las horas jugadas no pueden ser negativas.");
        }

        if (string.IsNullOrWhiteSpace(videojuego.Estado))
        {
            videojuego.Estado = "Pendiente";
        }
    }
}
