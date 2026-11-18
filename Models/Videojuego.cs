namespace InventarioVideojuegos.Models;

public class Videojuego
{
    private decimal precio;
    private int horasJugadas;

    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Plataforma { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;

    public decimal Precio
    {
        get => precio;
        set => precio = value < 0 ? 0 : value;
    }

    public int HorasJugadas
    {
        get => horasJugadas;
        set => horasJugadas = value < 0 ? 0 : value;
    }

    public string Estado { get; set; } = "Pendiente";

    public Videojuego()
    {
    }

    public Videojuego(int id, string titulo, string plataforma, string genero, decimal precio, int horasJugadas, string estado)
    {
        Id = id;
        Titulo = titulo;
        Plataforma = plataforma;
        Genero = genero;
        Precio = precio;
        HorasJugadas = horasJugadas;
        Estado = estado;
    }

    public override string ToString()
    {
        return $"{Titulo} - {Plataforma} ({Estado})";
    }
}
