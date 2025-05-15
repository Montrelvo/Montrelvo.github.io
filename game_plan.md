# Project Plan: Extensible Blazor WebAssembly Idle Production Game

**Goal:** Create a web page for an incremental idle production game using C# and Blazor WebAssembly, starting with Food and Water resources produced by Farms and Wells, with a focus on making it easy to add new resources, producers, and features later.

**Key Concepts:**

1. **Game State:** A central object or service holding all current game data (resource amounts, number of producers, upgrades, etc.). This is the single source of truth.
2. **Resources:** Items the player collects and spends (e.g., Food, Water).
3. **Producers:** Entities that automatically generate resources over time (e.g., Farm produces Food, Well produces Water).
4. **Game Loop:** A mechanism (like a timer) that periodically updates the game state, calculating production and handling other time-based events.
5. **Persistence:** Saving and loading the game state so players can continue where they left off.

**Proposed Structure and Implementation Steps:**

1. **Project Setup:**
    * Create a new Blazor WebAssembly project in Visual Studio or using the .NET CLI (`dotnet new blazorwasm`).

2. **Core Game State Management:**
    * Create a C# class, e.g., [`GameState.cs`](GameState.cs), to hold the game's state.
    * Inside `GameState`, use dictionaries or lists to store resources and producers. This makes adding new types easy.
        * `Dictionary<string, double> Resources;` (e.g., "Food" -> 0, "Water" -> 0)
        * `Dictionary<string, int> Producers;` (e.g., "Farm" -> 0, "Well" -> 0)
    * Implement methods in `GameState` to handle:
        * Adding/removing resources.
        * Buying producers (checking resource costs, decreasing resources, increasing producer count).
        * Calculating total production per resource type based on owned producers.
    * Consider making `GameState` a Blazor service (`@inject GameState State`) for easy access across components.

3. **Defining Resources and Producers (Extensibility):**
    * Create base classes or interfaces, e.g., [`ResourceDefinition.cs`](ResourceDefinition.cs) and [`ProducerDefinition.cs`](ProducerDefinition.cs).
    * `ResourceDefinition` could have properties like `Id` (string), `Name` (string), etc.
    * `ProducerDefinition` could have properties like `Id` (string), `Name` (string), `BaseCost` (Dictionary<string, double>), `BaseProduction` (Dictionary<string, double>), etc.
    * Create concrete classes for each resource and producer type inheriting from these bases (e.g., [`FoodResource.cs`](FoodResource.cs), [`WaterResource.cs`](WaterResource.cs), [`FarmProducer.cs`](FarmProducer.cs), [`WellProducer.cs`](WellProducer.cs)).
    * Maintain a central registry or list of all available `ResourceDefinition` and `ProducerDefinition` objects, perhaps in a static class or another service. This registry is where you'll add new types later.

4. **Production Logic (Game Loop):**
    * Implement a game loop, likely in the `GameState` service or a dedicated `GameEngine` service.
    * Use a `System.Timers.Timer` or `System.Threading.Timer` to trigger updates at a fixed interval (e.g., every 100ms or 1 second).
    * In the timer's elapsed event handler:
        * Calculate the time elapsed since the last update.
        * Iterate through all owned producers.
        * For each producer, calculate the resources produced during the elapsed time based on its production rate.
        * Add the produced resources to the `GameState.Resources`.
        * Notify components that the state has changed (e.g., by calling `StateHasChanged` on relevant components or using an event/callback pattern in the `GameState` service).

5. **Offline Production:**
    * When the game loads, calculate the time elapsed since the last save.
    * Simulate production for that entire offline duration based on the saved state.
    * Add the calculated offline production to the current resources.

6. **User Interface (Blazor Components):**
    * Create Blazor components (`.razor` files) to display game information and handle user input.
    * [`ResourceDisplay.razor`](ResourceDisplay.razor): Displays the current amount of each resource. Could iterate through `GameState.Resources`.
    * [`ProducerItem.razor`](ProducerItem.razor): Displays information about a single producer type (name, cost, production, number owned) and a button to buy more. This component would take a `ProducerDefinition` and the `GameState` as parameters.
    * [`ProducerList.razor`](ProducerList.razor): Displays a list of `ProducerItem` components, one for each available producer type from the registry.
    * [`Index.razor`](Index.razor): The main page component. Inject the `GameState` service and include instances of `ResourceDisplay` and `ProducerList`.

7. **Persistence (Saving/Loading):**
    * Implement save functionality:
        * Serialize the relevant parts of the `GameState` object (e.g., using `System.Text.Json`).
        * Use Blazor's JavaScript interop (`IJSRuntime`) to save the serialized string to the browser's `localStorage`.
        * Save periodically (e.g., every minute) and when the user closes the page (using the `beforeunload` event via JS interop).
    * Implement load functionality:
        * When the application starts, use JS interop to read the saved state from `localStorage`.
        * Deserialize the string back into a `GameState` object.
        * Initialize the game state with the loaded data.

8. **Extensibility Points:**
    * To add a new resource: Create a new `ResourceDefinition` class and add an instance to the central registry. The `ResourceDisplay` component should automatically pick it up if it iterates through the `GameState.Resources` dictionary.
    * To add a new producer: Create a new `ProducerDefinition` class and add an instance to the central registry. Create a corresponding concrete class. The `ProducerList` component should automatically create a `ProducerItem` for it. Update the `GameState`'s buying logic if needed (e.g., for more complex costs).
    * Upgrades, achievements, etc., can be added by extending the `GameState` and creating new component types.

# Mermaid Diagram: Core Component Interaction

```mermaid
graph TD
    A[Index.razor Page] --> B(GameState Service)
    B --> C(Resource Definitions)
    B --> D(Producer Definitions)
    A --> E[ResourceDisplay Component]
    A --> F[ProducerList Component]
    F --> G[ProducerItem Component]
    B --> E
    B --> F
    B --> G
    D --> G
    C --> E
