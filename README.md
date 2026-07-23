🚀 Descripción
Solución integral desarrollada bajo los principios de Clean Architecture, estructurada en microservicios/contenedores con una Web API en .NET (MediatR y FluentValidation) y una interfaz frontend en ASP.NET Core MVC para la gestión de inventarios.

🛠 Requisitos Previos
* Docker Desktop instalado y en ejecución.
* .NET SDK (opcional, si deseas ejecutar servicios fuera de Docker).

🐳 Ejecución con Docker (Recomendado)
Para levantar todo el entorno de forma orquestada (Base de datos SQL Server, Web API y la Interfaz Web Frontend), ejecuta en la raíz del proyecto:

```bash
docker-compose up --build
```

Una vez que los contenedores estén activos, puedes acceder a:
* **Interfaz Web (Frontend):** [http://localhost:5000](http://localhost:5000)
* **Documentación Swagger UI (API):** [http://localhost:8080/swagger](http://localhost:8080/swagger)

📋 Documentación y Uso
Los endpoints de la API están documentados automáticamente mediante Swagger/OpenAPI. 

Nota sobre Autenticación (Seguridad JWT):
La aplicación web maneja el flujo de autenticación contra la API mediante sesiones y tokens Bearer. Si deseas consumir los endpoints protegidos directamente desde Swagger o herramientas externas:
1. Realiza una petición POST al endpoint `/api/Auth/login` con tus credenciales.
2. Utiliza el token devuelto añadiéndolo en los encabezados de autorización (`Authorization: Bearer <tu-token>`).

⚙️ Configuración de Red
El proyecto utiliza un `docker-compose.yml` unificado bajo una red interna, asegurando la comunicación automática y segura entre el contenedor web, la API y la base de datos SQL Server.