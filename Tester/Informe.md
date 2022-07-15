# Domino PWA
Proyecto de Programación II. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2022

> Omar Rivero Gómez-Wangüemert. C-211

> Alex Sierra Alcalá. C-211

Domino PWA es una variación del juego de mesa clásico del domino, que como sabemos se compone de una serie de jugadores que juegan en una secuencia de turnos dada, ganando aquel que se quede sin fichas, o en caso de que no se pueda jugar más, ganando  aquel cuya mano suma menos puntos. En nuestra implementación, intentamos abstraer este concepto de juego, manteniendo la idea de un juego de mesa con fichas de dos caras, pero con regla modificables, pudiéndose obtener alguna variante bien diferente del juego clásico. Entre las reglas modificables se encuentran:

* La forma en la que se generan las fichas del juego.

* La forma en la que se reparten las fichas a cada jugador.

* El orden del turno en que se realizan las jugadas.

* La forma en la que matchea una ficha con otra.

* La forma de puntuar las fichas y decicidir quien gana.

* Las condiciones de fin de juego.

Contamos también con un sistema de personalización de torneos que comentaremos mejor más adelante. Tenemos además la implementación de una interfaz gráfica para el juego, que permite la interacción con el usuario...

## Detalles de la implementación

Para nuestra implementación, dividimos nuestro código en dos carpertas: `DominoLogic` y `MyDominoPWA`. En `DominoLogic` se encuentran las clases que implementan el juego, y en `MyDominoPWA` se encuentran las clases que implementan la interfaz gráfica. `DominoEngine` tiene a su vez las classlib `DominoEngine` y `Players`.

### DominoLogic

Aquí se encuentra toda la jerarquía de clases que nos permite armar la estructura sobre la cual descansa el juego. Es válido aclarar que, aunque solo implementamos una variante de domino donde las fichas tienen valores enteros en sus caras, la estructura está creada para correr sobre un juego totalmente genérico. Comenzemos explicando qué objetos usamos para nuestro juego:

#### `Token`
Representa una ficha del juego. Es una tupla de `(T,T)` donde cada valor está asigando a una de las caras. Sobre este objeto están redefinidos los métodos `Equals()` y `GetHashCode()`, ya que la ficha `(Head,Tail)` es exactamente igual a la ficha `(Tail,Head)`. Las propiedades `Head` y `Tail` nos permiten obtener el valor de la cara correspondiente, con un único accesor `get` para que sean inmutables.

#### `Move` 
Representa una jugada. Es una tupla de `(int,bool,int,T,T)`. En la primera posición se encuentra el Id del player que realiza la jugada. En la segunda posición se encuentra un booleano, por default true, que indica si la jugada es o no un pase. En la tercera posición se encuentra el turno por cual se jugará la ficha, por default -2, considerando que la salida apunta al turno -1, de esta forma, un `Move` cuyo `Turn` sea n, se refiere a un movimiento que se desea hacer matcheando el `Head` de una ficha con el `Tail` de la ficha jugada en el turno n. En la cuarta posición se encuentra el `Head` de la ficha jugada, y en la quinta posición se encuentra el `Tail` de la ficha jugada.

#### `Board` 
Representa el tablero del juego. Es una lista de `Move`, donde indexar en -1 es equivalente a pregunatar por el `Head` de la ficha jugada en la salida.

#### `Hand`
Representa las manos de los jugadores. Esta clase implementa la interfaz `ICollection<Token<T>>`, con un método `Clone()` que nos permite clonar una mano para evitar que nos cambien los valores originales al pasarlos por referencia.

#### `Player`
Es una clase abstracta que representa en esencia a los jugadores. Tiene un método `Play()` que recibe un `IEnumerbale<Move<T>>` y unos cuantos delegados que le brindan al jugador toda la información que pudiera necesitar sobre el juego:

```c#
public abstract Move<T> Play(IEnumerable<Move<T>> possibleMoves, Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, 
		Func<int, int> inHand, Func<Move<T>, double> scorer, Func<int, int, bool> partner);
```
* `possibleMoves`: Es un `IEnumerable<Move<T>>` que contiene todas las jugadas que puede hacer el jugador.
* `passesInfo`: Es una función que recibe un `int`, el Id de un player, y devuelve un `IEnumerable<int>` que contiene los turnos en que el jugador con dicho Id se pasó.
* `board`: Es una lista de `Move<T>` que representa el tablero del juego.
* `inHand`: Es una función que recibe un `int`, el Id de un player, y devuelve el número de fichas que tiene en su mano.
* `scorer`: Es una función que recibe un `Move<T>` y devuelve un `double` que representa la puntuación de la jugada.
* `partner`: Es una función que recibe una tupla `(int,int)`, el Id de dos players, y devuelve un `bool` que indica si ambos jugadores están en el mismo equipo.

#### `Team`
Es una lista de `Player` que representa a los jugadores que pertenecen a un mismo equipo. La idea de esta clase surge de la posibilidad del juego en equipo, y no hacer 



* `DominoSet`: Representa un conjunto de fichas del juego.
* `DominoPlayer`: Representa un jugador del juego.
* `DominoEngine`: Representa el motor del juego.
* `Players`: Representa una lista de jugadores del juego.







cómo abstraímos nuestra idea de reglas. Definimos el comportamiento de cada una de nuestras reglas mediante el uso de `Interfaces`