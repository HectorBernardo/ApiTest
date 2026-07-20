===========================================
1. Uso de Inteligencia Artificial (IA Generativa)
===========================================

Durante el desarrollo de esta prueba, utilicé herramientas de IA (Gemini) como copiloto para agilizar tareas repetitivas y resolver bloqueos técnicos, manteniendo siempre la supervisión de la arquitectura.

Arquitectura
Solicité estructura de Clean Architecture para una Web API en .NET 8.
Acepté la separación de capas (Domain, Application, Infrastructure, Api) propuesta, pero ajusté las dependencias para evitar acoplamiento excesivo.

Configuración Docker
"Configurar un docker-compose para SQL Server y la API".
La IA propuso una configuración base, la cual ajusté para incluir volúmenes persistentes y variables de entorno seguras.

Seguridad JWT
"Implementar autenticación JWT en una Web API con .NET".
La IA generó la estructura base de los Middlewares. Modifiqué la lógica para asegurar que el Auth/login fuera el único punto público.


Spec-Driven Development
Antes de generar cualquier bloque de código, definí primero las entidades en la capa Domain y los contratos (interfaces) en la capa Application. Solo después de tener el "contrato" claro, solicité a la IA la implementación lógica de los repositorios y controladores.

Diseño de Arquitectura
La IA sugirió inicialmente usar un patrón de Unit of Work muy complejo. Decidí simplificarlo hacia un enfoque de Repository Pattern más directo, ya que, para el alcance de esta prueba, el mantenimiento del código es prioritario sobre la abstracción excesiva.

===========================================
2. Toma de decisiones técnicas
===========================================

Decisión con información incompleta:
Al decidir cómo manejar la persistencia de datos con las migraciones de EF Core, no tenía claro el ciclo de vida exacto del contenedor SQL Server en el entorno del evaluador. Decidí avanzar utilizando migraciones automáticas al arrancar la aplicación (migrate en Program.cs) en lugar de scripts manuales, priorizando la facilidad de uso para quien evalúa, aunque esto suponga un pequeño riesgo de race condition en entornos de alta concurrencia.

Criterios para elegir entre dos enfoques:
Cuando elijo entre dos soluciones técnicas, mi criterio es:

Mantenibilidad: ¿Qué tan difícil será para otro desarrollador entender esto en 6 meses?
Desacoplamiento: ¿Puedo cambiar la base de datos o la librería sin reescribir todo el proyecto?
Estabilidad: Prefiero una solución simple y probada sobre una "innovadora" que dependa de librerías inestables.

Cambio de decisión técnica propia:
El caso más reciente fue la documentación con Swagger. Inicialmente, intenté integrar la seguridad completa usando Microsoft.OpenApi.Models para que apareciera el candado en la UI. Sin embargo, al encontrar conflictos de versiones entre dependencias que comprometían la compilación del proyecto, decidí revertir esa implementación.

===========================================
Aprendizaje: Aprendí que el "código decorativo" no debe comprometer la integridad funcional del proyecto. Opté por documentar la autenticación vía test.http, resultando en una solución más robusta y profesional para pruebas de API.
===========================================