# Aplicación de Optimización para BIOS

Este repositorio contiene el código fuente de una aplicación de optimización logística desarrollada exclusivamente para su uso en la empresa del cliente. **No es software de uso libre ni está destinado para distribución o uso fuera del entorno autorizado del cliente.**

El acceso a este repositorio está limitado al Departamento de Tecnología de la empresa cliente con el propósito exclusivo de **revisar el código fuente** y validar su seguridad e integridad.

---

## 🚀 **Características**

- **Modelos de Optimización Avanzados:** Capacidad para resolver problemas mixtos de hasta 40,000 variables (70% enteras).

- **Interfaz Gráfica Intuitiva:** Desarrollada en Windows Forms para un uso sencillo por usuarios finales no técnicos.

- **Procesamiento Multihilo:** Optimizado para aprovechar procesadores modernos.

- **Exportación de Resultados:** Permite guardar los resultados en formato csv para análisis adicionales.

---

## 🛠️ **Tecnologías y Dependencias**

El proyecto utiliza las siguientes tecnologías y librerías:

- **Framework:** .NET 8.0 para Windows.

- **Optimizador:** [Google OR-Tools](https://developers.google.com/optimization) v9.11.4210.

- **Manejo de datos:**

  - [Accord.MachineLearning](https://www.nuget.org/packages/Accord.MachineLearning/) v3.8.0.

  - [ExcelDataReader.DataSet](https://www.nuget.org/packages/ExcelDataReader.DataSet/) v3.7.0.

  - [SpreadsheetLight](https://spreadsheetlight.com/) v3.5.0.

---

## 📋 **Requisitos del Sistema**

### **Hardware**

- Procesador: Intel Core i7 o superior (recomendado i9-14900HX).

- Memoria RAM: 16 GB (mínimo), 32 GB (recomendado).

- Espacio en disco: Al menos 500 MB disponibles para el código y dependencias.


### **Software**

- Sistema Operativo: Windows 11 (64 bits).

- IDE: [Visual Studio Community Edition](https://visualstudio.microsoft.com/vs/community/), versión 2022 o superior.

---

## 🔧 **Cómo Descargar y Revisar el Código**

1. **Clonar el repositorio:**

   Solicita acceso al repositorio proporcionado por el desarrollador. Una vez autorizado:

   ```bash
   git clone https://github.com/tu_usuario/tu_repositorio.git
   ```

2. **Abrir el proyecto en Visual Studio Community Edition:** (Recomendado)

    - Descarge e instale Visual Studio Community Edition si no lo tienes.
    - Abra Visual Studio y seleccione la opción Abrir un proyecto o solución.
    - Navege a la carpeta donde se ha clonado el repositorio y selecciona el archivo de solución (.sln).
    - Revise el código:

Acceda a los archivos de código fuente desde el explorador de soluciones.

Revise las dependencias en el archivo packages.config o mediante el administrador de paquetes NuGet.

## 🛡️ Propósito del Repositorio

- **Propósito exclusivo:** Este repositorio tiene como objetivo permitir al Departamento de Tecnología del cliente validar el código fuente de la aplicación.

- **Seguridad:** El código fue desarrollado en un entorno controlado con herramientas y dependencias obtenidas exclusivamente de fuentes confiables.

- Revisiones:** Se recomienda usar las herramientas integradas en Visual Studio Community Edition para inspeccionar el código y verificar su integridad.

## Documentacion Adicional:

- [Configuracion del Equipo de Desarrollo](./docs/configuracion_equipo_desarrollo.md)
- [Requerimientos técnicos de la Aplicacion](./docs/requerimientos_tecnicos.md)
- [Documentación del procedimiento de Solución](./docs/documentacion_tecnica_modelo.md)
-

## ⚠️ Limitaciones de Uso

Este software no es de uso libre y no debe ser modificado, distribuido ni utilizado con otros fines que no sean los expresamente autorizados por la empresa cliente.

El acceso al código está restringido exclusivamente al personal autorizado por la empresa.

## 📧 Contacto
Si tiene preguntas o necesita soporte durante la revisión, por favor contacta al desarrollador:

- **Correo electrónico:** luisferpinilla@gmail.com

- **Teléfono:** +57 312-4161731

- **GitHub:** https://github.com/luisferpinilla

## 📝 Licencia

Este código no está licenciado para uso general. Es propiedad exclusiva del desarrollador y su uso está limitado al propósito mencionado.