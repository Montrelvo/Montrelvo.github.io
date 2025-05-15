using System;
using System.Threading;
using System.Threading.Tasks;

namespace IncrementalGame
{
    public class GameEngine : IDisposable
    {
        private readonly GameState _gameState;
        private Timer _timer;
        private const double UpdateIntervalMilliseconds = 1000; // Update every 1 second

        public event Action OnGameUpdate;

        public GameEngine(GameState gameState)
        {
            _gameState = gameState;
        }

        public void Start()
        {
            _timer = new Timer(UpdateGame, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(UpdateIntervalMilliseconds));
        }

        private void UpdateGame(object state)
        {
            // Calculate production since the last update
            var production = _gameState.CalculateProductionPerSecond();

            // Add produced resources to the game state
            foreach (var resourceProduction in production)
            {
                _gameState.AddResource(resourceProduction.Key, resourceProduction.Value * (UpdateIntervalMilliseconds / 1000.0));
            }

            // Notify components that the game state has changed
            OnGameUpdate?.Invoke();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}