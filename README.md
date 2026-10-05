# Práctica de Métodos Sobrecargados — Frecuencias, Sobrecarga y Recursividad

Resolución de tres casos de estudio en C# sobre contadores de frecuencia, sobrecarga de métodos y recursividad, desarrollados como aplicaciones de consola con .NET en Visual Studio Code.

**Universidad Tecnológica de Panamá** — Facultad de Ingeniería de Sistemas Computacionales
Herramientas de Programación Aplicada III (.Net) · Grupo 1IL133 · II Semestre 2026

- **Estudiante:** Diego Sanjur
- **Facilitadora:** Ing. Irina Fong

---

## Descripción

Este repositorio contiene tres aplicaciones de consola independientes, una por cada caso de estudio de la práctica. La primera simula 6000 lanzamientos de un dado y cuenta cuántas veces sale cada cara. La segunda demuestra la sobrecarga de métodos con dos versiones del método `Cuadrado`, una para enteros y otra para valores de punto flotante. La tercera calcula el factorial de los números del 0 al 10 mediante un método recursivo.

Cada caso se implementa como un proyecto separado porque un proyecto de consola admite un solo punto de entrada: los dos primeros usan instrucciones de nivel superior (*top-level statements*), mientras que el tercero declara su propio método `Main`.

## Estructura del repositorio

```
.
├── Frecuencias/
│   └── Program.cs            Simulación de 6000 tiros de un dado
├── SobreCargaMetodos/
│   ├── SobreCarga.cs         Clase con los métodos Cuadrado sobrecargados
│   └── Program.cs            Instancia la clase y prueba los métodos
└── Factorial/
    └── Program.cs            Cálculo recursivo del factorial del 0 al 10
```

---

## Caso 1: Frecuencias (Contadores)

El programa crea un objeto `Random` y declara seis variables contadoras, una por cada cara del dado, todas inicializadas en cero. Un ciclo `for` se repite 6000 veces; en cada vuelta, `Next(1, 7)` genera un número entre 1 y 6 (el límite superior es exclusivo) que se guarda en la variable `cara`. Una estructura `switch` evalúa ese valor e incrementa el contador correspondiente; el caso `default` nunca debería ejecutarse, pero queda como protección ante un valor fuera de rango.

Al terminar el ciclo se imprime una tabla con cada cara y su frecuencia usando formato compuesto, con `\t` para tabular y `\n` para separar las filas. Como los números son aleatorios, los resultados cambian en cada ejecución, aunque cada cara tiende a aparecer cerca de 1000 veces.

**Conceptos:** generación de números aleatorios con `Random`, contadores, ciclo `for`, estructura `switch`, operador de incremento `++`, secuencias de escape, formato compuesto.

## Caso 2: Sobrecarga de Métodos

La clase `SobreCarga` declara dos métodos con el mismo nombre, `Cuadrado`, que se diferencian por el tipo de su parámetro: uno recibe un `int` y devuelve un `int`, y el otro recibe un `double` y devuelve un `double`. Cada versión imprime un mensaje indicando cuál fue llamada antes de devolver el resultado.

El método `ProbarMetodosSobreCargados` llama a `Cuadrado(7)` y a `Cuadrado(7.5)`. El compilador decide qué versión ejecutar según el tipo del argumento: el literal `7` es entero y selecciona la versión `int`, mientras que `7.5` es un `double` y selecciona la otra. Esta decisión se toma en tiempo de compilación a partir de la firma del método (su nombre junto con el número, tipo y orden de los parámetros); el tipo de retorno no forma parte de la firma.

En `Program.cs` se crea un objeto `SobreCarga`, se ejecuta el método de prueba y luego se llama directamente a `Cuadrado(8)` y `Cuadrado(9)`. La llamada con 8 muestra el mensaje del método, pero su resultado (64) se descarta porque no se asigna ni se imprime; la llamada con 9 sí muestra el resultado, 81.

**Conceptos:** sobrecarga de métodos, firma de un método, resolución de sobrecarga en tiempo de compilación, métodos de instancia, espacios de nombres con ámbito de archivo, directiva `using`.

**Diagrama UML**

```
┌────────────────────────────────────────────┐
│                SobreCarga                  │
├────────────────────────────────────────────┤
├────────────────────────────────────────────┤
│ + ProbarMetodosSobreCargados()             │
│ + Cuadrado(valorInt: int): int             │
│ + Cuadrado(valorDouble: double): double    │
└────────────────────────────────────────────┘
```

## Caso 3: Recursividad (Factorial)

La clase `Program` contiene el método `Main`, que recorre los valores del 0 al 10 con un ciclo `for` y muestra el factorial de cada uno. El cálculo lo realiza el método estático `Factorial`, que se llama a sí mismo.

El método tiene un caso base: si el número es menor o igual a 1, devuelve 1 sin hacer más llamadas, lo que cubre tanto 0! como 1!. En cualquier otro caso aplica el paso de recursividad, devolviendo el número multiplicado por el factorial del número anterior. Cada llamada reduce el problema hasta llegar al caso base, y entonces los resultados se multiplican al regresar por la pila de llamadas. Se usa el tipo `long` porque los factoriales crecen muy rápido: 10! ya vale 3 628 800.

**Conceptos:** recursividad, caso base, paso recursivo, pila de llamadas, métodos estáticos, tipo `long`.

**Diagrama UML**

```
┌────────────────────────────────────────────┐
│                 Program                    │
├────────────────────────────────────────────┤
├────────────────────────────────────────────┤
│ + <<static>> Main(args: string[])          │
│ + <<static>> Factorial(numero: long): long │
└────────────────────────────────────────────┘
```
