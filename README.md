# MyAgentWebApp

MyAgentWebApp es una aplicación web basada en .NET que utiliza Microsoft Semantic Kernel para implementar agentes inteligentes con capacidades de memoria vectorial y procesamiento de lenguaje natural. Este proyecto incluye integración con OpenAI y Qdrant para almacenamiento y recuperación de datos vectorizados.

## Características

- **Integración con OpenAI**: Utiliza modelos de lenguaje como `chatgp-4o` para procesamiento de lenguaje natural.
- **Memoria Vectorial**: Implementación de Qdrant para almacenar y recuperar datos vectorizados.
- **Plugins Personalizados**: Incluye plugins como `InventoryPlugin`, `ProductionPlugin` y `MemoryPlugin` para extender las capacidades del agente.
- **Scraping y Vectorización**: Permite importar contenido web y documentos para enriquecer la base de conocimiento del agente.
- **API REST**: Proporciona endpoints para interactuar con los agentes.

- ## Uso

- **Vectorización de páginas web**:
La aplicación puede vectorizar contenido web automáticamente al inicio si `VectorizeAtStartup` está habilitado.
- **Importar documentos**:
  Puedes habilitar la importación de documentos descomentando la línea correspondiente en `Program.cs`:

  ## Estructura del Proyecto

- **Controllers**: Contiene los controladores de la API.
- **Plugins**: Plugins personalizados para extender las capacidades del agente.
- **Services**: Servicios que encapsulan la lógica de negocio.
- **Program.cs**: Configuración principal de la aplicación.
