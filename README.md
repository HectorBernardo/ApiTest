🚀 Descripción
API desarrollada bajo los principios de Clean Architecture, utilizando MediatR para el manejo de comandos/consultas y FluentValidation para las reglas de negocio.

🛠 Requisitos Previos
Docker Desktop instalado y en ejecución.

.NET SDK (opcional, si deseas ejecutar sin Docker).

🐳 Ejecución con Docker (Recomendado)
Para levantar la API junto con la base de datos SQL Server, ejecuta en la raíz del proyecto:

Bash
docker-compose up --build
Una vez que los contenedores estén activos, accede a la documentación:

Swagger UI: http://localhost:8080/swagger

📋 Documentación
Los endpoints están documentados automáticamente mediante Swagger/OpenAPI. Puedes realizar pruebas de petición directamente desde la interfaz de Swagger.

Nota sobre Autenticación (Seguridad JWT):
Debido a la implementación de seguridad en los controladores, los endpoints protegidos requieren un token válido. Para realizar pruebas:

Obtener Token: Realiza una petición POST al endpoint /api/Auth/login con tus credenciales.

Probar Endpoints: Utiliza el archivo test.http incluido en la raíz del proyecto para realizar tus llamadas. Para los endpoints protegidos, añade el encabezado de autorización en tu petición:

⚙️ Configuración
El proyecto utiliza docker-compose para orquestar los servicios.

La cadena de conexión a la base de datos está configurada para apuntar al servicio db dentro de la red de Docker.