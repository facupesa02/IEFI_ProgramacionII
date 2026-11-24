Para la parte de backend:

La arquitectura a utilizar sera de 4 capas:
- Api: contiene los controllers de las entidades.
- Entities: contiene las entidades y los DTo correspondientes.
- Business: Contiene la logica de negocio(Services).
- Data: contiene las interacciones y la conexion con la base de datos.

Todo lo anterior debera estar dividido y subdividido en sus respectivas carpetas.

Realizar toda la conexion e instalacion de entity framework core y realizar los dbset de cada entidad en la capa data para luego realizar las migrations correspondientes.

Para la parte de frontend:

Desarrollar un frontend

Requisitos técnicos obligatorios:

• Backend: Web API desarrollada en .NET 8. 
• Arquitectura: arquitectura por capas trabajada durante la cursada. 
• Base de datos: SQL Server con persistencia de datos. 
• Acceso a datos: Entity Framework Core. 
• Frontend: HTML, CSS y JavaScript vanilla, sin frameworks. 
• Comunicación: consumo de la API REST desde el frontend. 

Idea del proyecto:

Sistema de gestión de espacios para mascotas
Para guarderías caninas que necesitan organizar estadías y controlar la capacidad de sus instalaciones.
Registrar mascotas.
Registrar espacios.
Gestionar estadías.
Lógica: asignar espacios según tamaño y características de cada mascota.

Prompts:     
    1.Teniendo en cuenta el siguiente archivo necesito que me hagas la estructura del proyecto respetando las reglas establecidas en el mismo, por ahora no tengas en cuenta la parte de frontend.
    2. En la capa de entities necesito que me crees el model de estadia con los atributos mascota, espacio, fecha de ingreso y de salida con su constructor correspondiente. En la capa data necesito que hagas el dbset y la migracion.
    3. Necesito que me crees el controller correspondiente para este model, solo la estructura, sin endpoints.
    4. Necesito que me crees el service y el repository de estadia con las injecciones correspondientes en cada uno, en el program y en su controller.
    5. necesito armar el frontend dentro de la carpeta frontend de un sistema web de guardería de mascotas siguendo todo lo desarrollado en el backend. Tiene que ser solo HTML, CSS y JavaScript vainilla, sin frameworks ni librerías. Armame una estructura de carpetas simple: un index.html, una carpeta css, y una carpeta js con archivos separados. Quiero un menú de navegación arriba para pasar entre Espacios, Mascotas y Estadías, sin que se recargue la página.
    armame un archivo js/api.js con funciones genéricas para consumir mi Web API en .NET 8 usando fetch (GET, POST, PUT y DELETE). Dejá la URL base en una constante arriba para que yo la cambie fácil. Y que maneje los errores: si la API devuelve un error, quiero poder mostrarle el mensaje al usuario.
    Hagamos la pantalla de Espacios. Tiene que listar todos los espacios en una tabla y tener un formulario para crear uno nuevo llamando al post del controller. En cada fila quiero botones para editar y eliminar llamndo a los respectivos endpoints. Al editar, que cargue los datos en el mismo formulario. Antes de eliminar que pida confirmación.
    Seguí con la pantalla de Mascotas, con la misma lógica que la de Espacios: tabla, formulario, editar y eliminar. Usá el mismo estilo de código así queda todo parejo. Los campos van a ser Los que tiene el model mascota. Ahora lo más importante, la pantalla de Estadías. Tiene que permitir registrar una estadía eligiendo una mascota y un espacio de dos listas desplegables que se cargan desde la API, más fecha de ingreso y fecha de salida. Si la API responde que el espacio ya está ocupado en esas fechas, mostrar ese mensaje de forma clara.
    Agregá validaciones en todos los formularios antes de mandar nada a la API: campos obligatorios, números mayores a 0 y que la fecha de salida no sea anterior a la de ingreso. Los errores que aparezcan debajo de cada campo, en rojo, no con alert().
    Ahora dale estilo con CSS. Quiero algo moderno, limpio y simple, con colores suaves, tablas legibles, botones diferenciados para editar y eliminar, y que se vea bien también en el celular. Nada de frameworks como Bootstrap, todo con mi propio CSS.
    6.Usando de base la entidad mascota, necesito que me crees los dtos que creas necesarios, 
    el repository de mascota utilizando los dto y el context, el service y el Iservice, y 
    el controller con el crud basico utilizando linq y try y catch sin funciones de mas.
    7. Usando la entidad Espacio como base, necesito que me hagas los DTOs que hagan falta, el repository usando los dto y el context, el service y su Iservice. También quiero que me armes el controller con el CRUD básico para poder crear, ver, modificar y eliminar espacios. Usá LINQ y try-catch, y no agregues funciones de más ni cosas que no sean necesarias.