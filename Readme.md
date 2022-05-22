# Domino

> Proyecto de Programación II.
> Facultad de Matemática y Computación - Universidad de La Habana.
> Curso 2022.

El propósito de este proyecto es implementar una biblioteca de clases que permita modelar y experimentar con diferentes variantes del juego Dominó, así como diferentes estrategias de jugadores virtuales.

Usted deberá entregar una solución compuesta, al menos, por dos proyectos en el lenguaje de programación C#:

- Una biblioteca de clases, implementada en C# 10, .NET Core 6, donde se implemente toda la funcionalidad lógica de su solución.
- Una aplicación visual, implementada con tecnologías de libre elección en el lenguaje de programación C#, que permita visualizar el resultado de los juegos.

## Sobre la biblioteca de clases

Su biblioteca de clases debe permitir modelar diferentes variantes del juego de dominó clásico, de forma que sea posible modificar algunos aspectos relevantes de la mecánica del juego. Usted debe permitir modificar al menos **5 aspectos relevantes** de la mecánica del dominó.

Una lista no exhaustiva de los aspectos que pudiera querer personalizar son:

- La cantidad de fichas del juego, de forma que pueda jugarse, por ejemplo, la variante doble-9, la variante doble-6, y potencialmente cualquier otra variante arbitraria con otra numeración de fichas.

- Las condiciones de finalización del juego, por ejemplo, haciendo que un juego termine cuando un jugador se pase dos veces seguidas, o cuando se acumulen un total de puntos, etc.

- La forma inicial de repartir las fichas, por ejemplo, para que en vez de ser totalmente aleatoria, cada jugador pueda ver las fichas que escogió, botar un subconjunto de las mismas, y volver a repartir aleatoriamente.

- El orden de las jugadas, por ejemplo, para que cuando un jugador se pase, cambie el sentido en que se juega.

- La forma de calcular la puntuación final, por ejemplo, para contar las fichas dobles como el doble de puntos, o para penalizar todas las fichas de una misma data.

- La forma de ubicar las fichas, por ejemplo, para impedir que un jugador pueda jugar por la ficha de su pareja, o para que haya una ficha, digamos el doble blanco, que pueda jugarse en cualquier momento.

Note que estas son solo algunos de los múltiples elementos de las reglas del juego que pueden cambiarse. Usted tiene la libertad de decidir hasta dónde desea que sea personalizable su implementación.

Por cada uno de los elementos que son personalizables en su juego, usted debe proveer **al menos dos implementaciones diferentes** de dicho elemento. Por ejemplo, si permite cambiar la forma en que se calcula la puntuación final, usted puede implementar la variante clásica de sumar todos los puntos, y al menos otra variante de su invención.

Además de las reglas del juego, su biblioteca de clases también debe permitir implementar diferentes estrategias de jugadores virtuales. Usted debe implementar **al menos 3 estrategias diferentes**.

Algunos ejemplos posibles son:

- Un jugador que siempre juegue una ficha aleatoria que sea válida.
- Un jugador que siempre juege la ficha que tiene mayor valor de las válidas (el famoso bota-gorda).
- Un jugador que tenga una heurística específica basada en su experiencia personal del dominó.
- Un jugador que simule el juego hasta cierto punto y juege la ficha que tiene mayor probabilidad de ganar.
- Un jugador que en cada turno juegue alguna de las estrategias anteriores, y vaya "aprendiendo" cuál funciona mejor a medida que el juego avanza.

Note que sus jugadores virtuales tienen que funcionar **de forma transparente** para cualquiera que sea su configuración actual. Por ejemplo, si usted implementa un jugador que siempre juega la ficha de mayor puntuación, y además implementa diferentes maneras de calcular la puntuación de las fichas, su jugador debe funcionar con cualquiera de esas variantes de calcular la puntuación, *y cualquier otra variante que sea implementada en el futuro*.

## Sobre la aplicación visual

Para mostrar sus resultados usted debe implementar una aplicación visual que permita ejecutar una partida de dominó con cualquier combinación de todas las variantes diferentes que usted implementó, y visualizar el resultado de la partida.

La aplicación puede ser tan sencilla como una aplicación de línea de comandos (o sea, en la consola prieta), una aplicación web, una aplicación con gráficos en 3D, una aplicación móvil, etc. Queda a su decisión cuánto esfuerzo ponerle a dicha aplicación, pero al menos tiene que tener las siguientes funcionalidades.

- Permitir al usuario configurar una partida de dominó personalizando todos los elementos que su biblioteca de clases permite variar, incluyendo los jugadores virtuales a utilizar.

- Que al implementar una nueva variante de cualquiera de los elementos configurables, la aplicación visual necesite *el menor cambio posible* para incluir esta nueva variante en su interfaz (idealmente ningún cambio, y como mucho, adicionar una instancia de una clase en un *array*).

- Mostrar el desarrollo de la partida paso a paso de forma interactiva, o directamente hasta el final. Dependiendo de la tecnología que escoja, esto puede ser desde imprimir en la terminal la ficha jugada (e.j., `[2:4]`) o mostrar una ficha en 3D.

Independientemente de la tecnología utilizada, su interfaz debe ser suficientemente flexible para que cuando cambien la estructura de las fichas, o la longitud del juego, o cualquier otro aspecto que influya en cómo se visualiza, **la interfaz gráfica se adapte de forma automática**.

## Sobre la ingeniería de software

El propósito fundamental de este proyecto es evaluar el uso de las buenas prácticas de desarrollo de software que se han enseñado en el curso. Por tal motivo, se pondrá especial énfasis en evaluar el diseño de las clases, interfaces, y métodos, y se profundizará durante la evaluación en la justificación que usted tenga para las decisiones de diseño de software que ha tomado.

Esto tendrá mayor peso en la evaluación final que cualquier consideración sobre la funcionalidad en sí. La calidad de la interfaz gráfica, los efectos de sonido, la inteligencia artificial de los jugadores, todo eso aporta y será considerado, pero es secundario ante un buen diseño de clases que sea extensible y mantenible.

Algunos consejos:

- Recuerde los principios SOLID y diseñe en consecuencia.
- Priorice las interfaces sobre las clases abstractas, y la composición sobre la herencia.
- Recuerde que las abstracciones se diseñan encima de las funcionalidades a implementar, y no solamente encima de los "conceptos" o "entidades" convencionales del dominio.
- Recuerde el principio DRY y las estrategias de encapsulamiento existentes para reutilizar código y evitar la duplicidad de funcionalidades.
- Utilice nombres de clases y variables descriptivos, y adicione comentarios en su código para explicar las motivaciones detrás de su diseño.
- Y sobre todo, recuerde que ningún diseño pensado a priori sobrevive la prueba del tiempo. Empiece con el diseño más sencillo posible e introduzca abstracciones a medida que descubre que las necesita.

## Sobre la entrega

Dada la complejidad del proyecto, el mismo se hará en dúos. Usted tiene la posibilidad de hacerlo individualmente, pero tenga en cuenta que la complejidad será mucho mayor.

La entrega del proyecto será exclusivamente mediante un repositorio en Github.

En la raíz de su proyecto debe haber un archivo `Readme.md` con **todas las instrucciones necesarias** para ejecutar su código, así como todos los detalles de instalación (si necesitara algo más allá de .NET Core 6) y el uso de su interfaz gráfica.

Además, usted debe tener en la raíz de su repositorio un archivo de nombre `Report.pdf` que contendrá un reporte técnico de su proyecto. Este reporte tendrá una extensión mínima de 5 cuartillas, y ahí debe detallar su diseño de clases, exponiendo las motivaciones detrás de cada abstracción (clase, interfaz, método, etc.) así como todos los detalles de implementación que sean necesarios. Incluya fragmentos de código y capturas de pantalla para ayudar en la explicación.

Para entregar su proyecto, usted debe [**abrir en este repositorio un *issue***](https://github.com/matcom/domino/issues/new?assignees=&labels=&template=proyecto.md&title=Proyecto+de+Nombre+y+Nombre), donde incluirá el nombre de los miembros del equipo, el link a su repositorio, y el link directo a su archivo `Report.pdf`.

La evaluación se realizará en dos partes. Primero, su mentor evaluará el reporte y hará los comentarios que considere necesarios en el *issue* correspondiente a su proyecto. Una vez que el mentor considere que su proyecto está en condiciones de ser expuesto, le dará una cita para exponer en persona en la Facultad. El resultado de esa exposición quedará plasmado en el *issue* correspondiente.

La nota final será decidida por un tribunal que tendrá en cuenta las consideraciones de su mentor así como el código fuente y el reporte entregado.
