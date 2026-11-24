# Whack-A-Mole
Es una versión simple del clásico juego Whack-A-Mole

## Design Patterns
El proyecto contiene varios patrones de diseño:
- **Event Bus:** Lo utiliza para comunicar clases alrededor de todo el proyecto sin romper la modularidad y el encapsulamiento. Se usa mayormente en la UI, para recibir cambios en el juego que puedan afectarla.
- **TaskScheduler:** El LevelManager depende del TaskScheduler para facilitar la aparición de las Mole. Funciona como una manera mucho más simple y centralizada de poner tareas en la lista de espera sin tener que escribir nuevas funciones para usar Corrutinas (o Awaitables).
- **Bootstrapper:** Se encarga de instanciar todos los elementos necesarios para que funcione el juego, como el SceneLoader o el EventBus.
- **ServiceProvider:** Cumple la función de darnos acceso a varias funcionalidades del programa de manera global y sin tener que depender de Singletons (que pueden causar problemas en el cambio de escena, al tener referencias a elementos de escenas anteriores). Lo usamos en cada llamada que hacemos a un servicio, entre los que está el EventBus, SceneLoader, TaskScheduler, entre otros.
- **Decorator:** La interfaz IMoleProperty que se usa en Mole, y se implementa en FrozenProperty (la propiedad que hace que una rana esté congelada), sirve como base para la implementación del patrón Decorator en la Mole. En esta, pueden ser agregadas varias properties mientras estas implementen esta interfaz. Por falta de tiempo, solo llegué a integar una property, pero crear una nueva es tan fácil como crear una clase con IMoleProperty.
