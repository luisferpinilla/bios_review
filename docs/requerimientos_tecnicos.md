# Requerimientos técnicos

Esta aplicación de escritorio ha sido desarrollada con C# y WinForms y utiliza un modelo de optimización mixto que maneja hasta 40,000 variables, de las cuales el 70% son enteras. El instalador incluye todas las dependencias necesarias para su ejecución y utiliza la última tecnología basada en .NET 8.0 (net8.0-windows).

## 1. Sistema Operativo

### - Windows 10 (64 bits) o superior (Windows 11 recomendado).

- Edición: Home, Pro, Enterprise o Server con soporte de 64 bits.

### - Requerimientos de .NET:

- La aplicación requiere .NET 8.0 (net8.0-windows) para ejecutarse correctamente. El instalador configurará automáticamente este entorno si no está disponible en el sistema.

## 2.Procesador (CPU)

### Mínimo: 

- Procesador de 4 núcleos, 64 bits (por ejemplo, Intel Core i5 de 8ª generación o AMD Ryzen 3).

### Recomendado:

- Procesador de 8 núcleos o superior (Intel Core i7/i9 o AMD Ryzen 7/9).

- Soporte para instrucciones avanzadas como AVX/AVX2, especialmente si el solver utiliza optimizaciones para cálculos matriciales.

## 3. Memoria RAM

### Mínimo: 

- 16 GB.

### Recomendado:

- 32 GB o más para manejar eficientemente los problemas de optimización grandes (40,000 variables, 70% enteras) y evitar problemas de paginación al usar el solver.

## 4. Espacio en Disco

### Mínimo:

500 MB para la instalación del programa y dependencias.
Espacio adicional para archivos temporales que puedan generarse durante la optimización (hasta 5 GB para grandes instancias de modelos).

### Recomendado:

SSD de al menos 256 GB para acelerar las operaciones de lectura/escritura.

## 5. Solver de Optimización

El solver incluido en la aplicación es Google OR-Tools 9.11.4210, compatible con problemas de programación lineal y mixta.

### 6. Dependencias Incluidas en el Instalador

El instalador ya incluye todas las librerías necesarias para que la aplicación funcione sin configuraciones adicionales:

- Google.OrTools 9.11.4210: Solver principal para la optimización del modelo.
- Accord.MachineLearning 3.8.0: Soporte para análisis y manipulación de datos relacionados con la optimización.
- ExcelDataReader.DataSet 3.7.0: Permite leer archivos de Excel para importar datos.
- SpreadsheetLight 3.5.0: Soporte para generar reportes en formato Excel.

Estas dependencias se instalan automáticamente junto con la aplicación y son compatibles con net8.0-windows.

## 7. Resolución de Pantalla

### Mínimo:

Resolución de 1366 x 768 píxeles.

### Recomendado:

1920 x 1080 píxeles para mejorar la experiencia de usuario en el manejo del modelo y reportes visuales.

## 8. Conectividad

No requiere, salvo para obtener actualizaciones según contrato de mantenimiento.

## 9. Requerimientos Adicionales para Rendimiento

### Multi-threading:

El solver Google OR-Tools está configurado para utilizar múltiples hilos del procesador y mejorar el tiempo de solución.

### Paralelismo:

Asegúrese de ejecutar la aplicación en equipos con CPU que soporten múltiples núcleos para maximizar el rendimiento.