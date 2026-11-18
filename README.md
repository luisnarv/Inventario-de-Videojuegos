# Inventario Personal de Videojuegos

## 1. Informacion general

* Proyecto: Inventario Personal de Videojuegos
* Estudiante: Luis Narvaez
* Asignatura: Programacion con C# y .NET (Electiva de Profundizacion I)
* Semestre: Cuarto Semestre
* Evaluacion: Parcial Final - Desarrollo de Software con .NET

### Descripcion del software
Aplicacion de escritorio construida en C# y .NET 8 que permite a los usuarios gestionar su coleccion personal de videojuegos, llevando el control de titulos, plataformas, generos, precios, horas de juego dedicadas y estado actual (Pendiente, Jugando, Completado).

### Problema que resuelve
Los aficionados a los videojuegos suelen acumular titulos en multiples tiendas digitales y consolas fisicas, perdiendo el control de que juegos tienen pendientes, cuanto tiempo les han dedicado o que compras han realizado. Esta aplicacion centraliza la coleccion en una base de datos local y permite administrarla de manera visual e intuitiva.

### Objetivo principal
Demostrar la aplicacion practica de los principios de Programacion Orientada a Objetos (POO), la construccion de interfaces graficas de escritorio modernas con Avalonia UI y la persistencia de datos relacionales mediante ADO.NET conectandose a una base de datos SQLite.

## 2. Funcionalidades

* Registrar videojuego: Permite ingresar nuevos juegos mediante un formulario con validacion de datos en tiempo real.
* Consultar catalogo: Muestra la lista completa de videojuegos registrados con sus detalles, identificador y estado.
* Filtrado en tiempo real: Cuadro de busqueda que filtra instantaneamente por titulo, plataforma o genero.
* Actualizar videojuego: Al seleccionar un elemento de la lista, se cargan sus datos en el formulario para modificar precio, horas, estado o plataforma y guardar los cambios.
* Eliminar videojuego: Permite retirar un juego de la coleccion seleccionandolo y presionando el boton de eliminacion.
* Limpiar formulario: Restablece todos los campos para iniciar una nueva captura de datos.
* Validacion preventiva: Notificaciones visuales informativas en caso de campos vacios, numeros negativos o valores incorrectos sin interrumpir la ejecucion del software.
* Persistencia local: Al cerrar y reabrir la aplicacion, todos los registros permanecen almacenados de forma segura en la base de datos SQLite.

## 3. Tecnologias utilizadas

* Lenguaje de programacion: C# 12
* Plataforma de ejecucion: .NET SDK 8.0.424
* Framework de interfaz grafica: Avalonia UI 11.1.3 (con FluentTheme y fuentes Inter)
* Acceso a datos: ADO.NET puro con la libreria Microsoft.Data.Sqlite 8.0.8
* Motor de base de datos: SQLite 3 embebido (archivo local videojuegos.db)
* Control de versiones: Git y GitHub

## 4. Estructura del proyecto y Programacion Orientada a Objetos

El proyecto aplica una arquitectura en capas con separacion clara de responsabilidades:

```
InventarioVideojuegos/
│
├── App.axaml
├── App.axaml.cs
├── app.manifest
├── InventarioVideojuegos.csproj
├── MainWindow.axaml
├── MainWindow.axaml.cs
├── Program.cs
│
├── Models/
│   └── Videojuego.cs
│
├── Data/
│   ├── DatabaseHelper.cs
│   └── VideojuegoDAO.cs
│
├── Services/
│   └── VideojuegoServicio.cs
│
└── evidencias/
    ├── 01_registro.png
    ├── 02_consulta.png
    ├── 03_actualizacion.png
    ├── 04_eliminacion.png
    ├── 05_validacion.png
    └── 06_persistencia.png
```

### Responsabilidad de cada clase

* Models/Videojuego.cs: Representa la entidad central del dominio. Contiene propiedades encapsuladas (Id, Titulo, Plataforma, Genero, Precio, HorasJugadas, Estado), constructores sobrecargados y validacion de encapsulamiento para evitar valores negativos en precios y horas.
* Data/DatabaseHelper.cs: Clase estatica utilitaria que gestiona la cadena de conexion hacia SQLite y ejecuta la creacion automatica de la tabla videojuegos y datos de prueba al iniciar el programa.
* Data/VideojuegoDAO.cs: Objeto de Acceso a Datos (DAO). Implementa las cuatro operaciones CRUD utilizando comandos ADO.NET (SqliteConnection, SqliteCommand, SqliteDataReader) con consultas estrictamente parametrizadas para proteger la integridad del sistema.
* Services/VideojuegoServicio.cs: Capa de logica de negocio. Contiene las reglas del sistema y valida campos obligatorios antes de delegar la persistencia al DAO.
* MainWindow.axaml y MainWindow.axaml.cs: Capa de presentacion en Avalonia UI. Declara los controles en XAML y atiende los eventos de usuario (clicks en botones, cambios de texto y seleccion en la lista), comunicandose exclusivamente con la capa de servicio.

### Principios de POO aplicados

* Encapsulamiento: Los atributos de la clase Videojuego protegen su estado interno validando que precios y horas no tomen valores negativos.
* Abstraccion: La interfaz de usuario no conoce las sentencias SQL ni la estructura interna de la base de datos; solo interactua con metodos del servicio (Registrar, Modificar, Eliminar, ListarTodos).
* Separacion de responsabilidades: La UI no accede directamente a la base de datos y la capa de datos no manipula controles visuales.

## 5. Instalacion de dependencias

### Requisitos previos
* Sistema operativo Windows 10/11, Linux o macOS.
* .NET SDK 8.0 instalado en el equipo.

### Pasos de instalacion
1. Clonar el repositorio desde GitHub:
```bash
git clone https://github.com/usuario/InventarioVideojuegos.git
cd InventarioVideojuegos
```

2. Restaurar los paquetes NuGet requeridos:
```bash
dotnet restore
```

## 6. Ejecucion del programa

Para compilar y ejecutar la aplicacion de escritorio, situarse en la raiz del proyecto y ejecutar:

```bash
dotnet run
```

La base de datos SQLite se creara e inicializara automaticamente en la primera ejecucion si no existe, insertando registros de prueba para comprobar el funcionamiento inmediato.

## 7. Base de datos y persistencia

* Motor utilizado: SQLite 3.
* Archivo de base de datos: videojuegos.db (creado automaticamente en el directorio de ejecucion).
* Nombre de la tabla: videojuegos.

### Estructura de la tabla

* id: INTEGER, Clave Primaria, Auto incremento.
* titulo: TEXT, obligatorio.
* plataforma: TEXT, obligatorio.
* genero: TEXT, obligatorio.
* precio: REAL, obligatorio.
* horas: INTEGER, obligatorio.
* estado: TEXT, obligatorio.

### Operaciones CRUD con ADO.NET

* Create: Ejecuta INSERT INTO videojuegos (titulo, plataforma, genero, precio, horas, estado) VALUES (@titulo, @plataforma, @genero, @precio, @horas, @estado).
* Read: Ejecuta SELECT id, titulo, plataforma, genero, precio, horas, estado FROM videojuegos ORDER BY id DESC recuperando los registros con SqliteDataReader.
* Update: Ejecuta UPDATE videojuegos SET titulo=@titulo, plataforma=@plataforma, genero=@genero, precio=@precio, horas=@horas, estado=@estado WHERE id=@id.
* Delete: Ejecuta DELETE FROM videojuegos WHERE id=@id.

Todas las consultas utilizan parametros tipados mediante comando.Parameters.AddWithValue() para garantizar la seguridad frente a inyecciones SQL.

## 8. Evidencias de funcionamiento

En la carpeta evidencias se encuentran almacenadas las capturas de pantalla que verifican el cumplimiento de los 6 escenarios requeridos:

1. Registro (evidencias/01_registro.png):
Captura de pantalla agregando un nuevo videojuego mediante el formulario y visualizando la notificacion de confirmacion verde junto con la adicion inmediata del elemento a la lista.

2. Consulta (evidencias/02_consulta.png):
Captura de pantalla visualizando la coleccion completa de videojuegos cargada desde SQLite con el contador total en la barra superior.

3. Actualizacion (evidencias/03_actualizacion.png):
Captura de pantalla seleccionando un videojuego existente, modificando sus horas jugadas y guardando los cambios con mensaje de actualizacion exitosa.

4. Eliminacion (evidencias/04_eliminacion.png):
Captura de pantalla seleccionando un registro y ejecutando su borrado, reflejando el decremento en el contador y la desaparicion del elemento en la lista.

5. Validacion (evidencias/05_validacion.png):
Captura de pantalla intentando guardar con campos obligatorios vacios; el sistema despliega una alerta en rojo indicando el error sin cerrar la aplicacion.

6. Persistencia (evidencias/06_persistencia.png):
Captura de pantalla tras cerrar y volver a ejecutar la aplicacion, demostrando que los datos guardados en SQLite se conservan intactos.

## 9. Problemas encontrados y soluciones

* Problema 1: Diferencia en la conversion de tipos numericos entre SQLite y C#. SQLite almacena los valores de punto flotante como tipo REAL (equivalente a double en C#), mientras que en C# el modelo utilizaba decimal para el precio monetario.
Solucion: En el lector de datos (SqliteDataReader) se implemento la conversion explicita Convert.ToDecimal(lector.GetDouble(4)) y al enviar parametros se realizo el casteo a double, garantizando precision y compatibilidad sin errores de casteo en tiempo de ejecucion.

* Problema 2: Actualizacion dinamica de la vista al agregar o modificar registros sin reiniciar el programa.
Solucion: Se implemento el metodo centralizado CargarDatos() en MainWindow.axaml.cs que vuelve a consultar la capa de servicio y actualiza la coleccion ItemsSource del control ListBox tras cada operacion CRUD confirmada.
