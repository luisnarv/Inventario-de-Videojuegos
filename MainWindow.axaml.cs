using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using InventarioVideojuegos.Models;
using InventarioVideojuegos.Services;

namespace InventarioVideojuegos;

public partial class MainWindow : Window
{
    private readonly VideojuegoServicio servicio;
    private List<Videojuego> listaCompleta;
    private Videojuego? videojuegoSeleccionado;

    public MainWindow()
    {
        InitializeComponent();
        servicio = new VideojuegoServicio();
        listaCompleta = new List<Videojuego>();

        cmbPlataforma.SelectedIndex = 0;
        cmbEstado.SelectedIndex = 0;

        CargarDatos();
    }

    private void CargarDatos()
    {
        try
        {
            listaCompleta = servicio.ListarTodos();
            AplicarFiltro();
            ActualizarContador();
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error al cargar datos: {ex.Message}", true);
        }
    }

    private void AplicarFiltro()
    {
        var termino = txtBuscar?.Text?.Trim().ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(termino))
        {
            lstVideojuegos.ItemsSource = listaCompleta.ToList();
        }
        else
        {
            var filtrados = listaCompleta.Where(v =>
                v.Titulo.ToLower().Contains(termino) ||
                v.Plataforma.ToLower().Contains(termino) ||
                v.Genero.ToLower().Contains(termino)
            ).ToList();

            lstVideojuegos.ItemsSource = filtrados;
        }
    }

    private void ActualizarContador()
    {
        var total = listaCompleta.Count;
        txtTotalJuegos.Text = $"Total: {total} {(total == 1 ? "juego" : "juegos")}";
    }

    private void OnGuardarClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var nuevoJuego = ConstruirDesdeFormulario();
            servicio.Registrar(nuevoJuego);

            MostrarMensaje($"Videojuego '{nuevoJuego.Titulo}' registrado con exito.", false);
            LimpiarFormulario();
            CargarDatos();
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, true);
        }
    }

    private void OnActualizarClick(object? sender, RoutedEventArgs e)
    {
        if (videojuegoSeleccionado == null)
        {
            MostrarMensaje("Debe seleccionar un videojuego de la lista para actualizar.", true);
            return;
        }

        try
        {
            var juegoEditado = ConstruirDesdeFormulario();
            juegoEditado.Id = videojuegoSeleccionado.Id;

            servicio.Modificar(juegoEditado);

            MostrarMensaje($"Videojuego #{juegoEditado.Id} actualizado correctamente.", false);
            LimpiarFormulario();
            CargarDatos();
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, true);
        }
    }

    private void OnEliminarClick(object? sender, RoutedEventArgs e)
    {
        if (videojuegoSeleccionado == null)
        {
            MostrarMensaje("Debe seleccionar un videojuego de la lista para eliminar.", true);
            return;
        }

        try
        {
            var idAEliminar = videojuegoSeleccionado.Id;
            var titulo = videojuegoSeleccionado.Titulo;

            servicio.Eliminar(idAEliminar);

            MostrarMensaje($"Videojuego '{titulo}' (ID: {idAEliminar}) eliminado exitosamente.", false);
            LimpiarFormulario();
            CargarDatos();
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, true);
        }
    }

    private void OnLimpiarClick(object? sender, RoutedEventArgs e)
    {
        LimpiarFormulario();
        MostrarMensaje("Formulario restablecido.", false);
    }

    private void OnRefrescarClick(object? sender, RoutedEventArgs e)
    {
        txtBuscar.Text = string.Empty;
        CargarDatos();
        MostrarMensaje("Lista de videojuegos actualizada.", false);
    }

    private void OnBuscarTextChanged(object? sender, TextChangedEventArgs e)
    {
        AplicarFiltro();
    }

    private void OnSeleccionJuegoChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (lstVideojuegos.SelectedItem is Videojuego seleccionado)
        {
            videojuegoSeleccionado = seleccionado;
            txtTitulo.Text = seleccionado.Titulo;
            txtGenero.Text = seleccionado.Genero;
            txtPrecio.Text = seleccionado.Precio.ToString("F2", CultureInfo.InvariantCulture);
            txtHoras.Text = seleccionado.HorasJugadas.ToString();

            SeleccionarItemComboBox(cmbPlataforma, seleccionado.Plataforma);
            SeleccionarItemComboBox(cmbEstado, seleccionado.Estado);

            MostrarMensaje($"Seleccionado: #{seleccionado.Id} - {seleccionado.Titulo}", false);
        }
    }

    private Videojuego ConstruirDesdeFormulario()
    {
        var titulo = txtTitulo.Text?.Trim() ?? string.Empty;
        var genero = txtGenero.Text?.Trim() ?? string.Empty;

        var plataformaItem = cmbPlataforma.SelectedItem as ComboBoxItem;
        var plataforma = plataformaItem?.Content?.ToString() ?? "PC";

        var estadoItem = cmbEstado.SelectedItem as ComboBoxItem;
        var estado = estadoItem?.Content?.ToString() ?? "Pendiente";

        if (!decimal.TryParse(txtPrecio.Text?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
        {
            throw new ArgumentException("El precio debe ser un numero decimal valido (ej. 49.99).");
        }

        if (!int.TryParse(txtHoras.Text?.Trim(), out var horas))
        {
            throw new ArgumentException("Las horas jugadas deben ser un numero entero valido.");
        }

        return new Videojuego
        {
            Titulo = titulo,
            Plataforma = plataforma,
            Genero = genero,
            Precio = precio,
            HorasJugadas = horas,
            Estado = estado
        };
    }

    private void SeleccionarItemComboBox(ComboBox comboBox, string valor)
    {
        for (int i = 0; i < comboBox.ItemCount; i++)
        {
            if (comboBox.Items[i] is ComboBoxItem item && string.Equals(item.Content?.ToString(), valor, StringComparison.OrdinalIgnoreCase))
            {
                comboBox.SelectedIndex = i;
                return;
            }
        }
        comboBox.SelectedIndex = 0;
    }

    private void LimpiarFormulario()
    {
        videojuegoSeleccionado = null;
        lstVideojuegos.SelectedItem = null;
        txtTitulo.Text = string.Empty;
        txtGenero.Text = string.Empty;
        txtPrecio.Text = "0.00";
        txtHoras.Text = "0";
        cmbPlataforma.SelectedIndex = 0;
        cmbEstado.SelectedIndex = 0;
    }

    private void MostrarMensaje(string mensaje, bool esError)
    {
        bannerMensaje.IsVisible = true;
        txtMensaje.Text = mensaje;

        if (esError)
        {
            bannerMensaje.Background = new SolidColorBrush(Color.Parse("#452530"));
            txtMensaje.Foreground = new SolidColorBrush(Color.Parse("#F38BA8"));
        }
        else
        {
            bannerMensaje.Background = new SolidColorBrush(Color.Parse("#263D31"));
            txtMensaje.Foreground = new SolidColorBrush(Color.Parse("#A6E3A1"));
        }
    }
}
