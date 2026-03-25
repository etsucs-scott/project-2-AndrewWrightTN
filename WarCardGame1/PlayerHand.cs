using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame1
{
    /// <summary>
    /// Manages all players' hands
    /// </summary>
    public class PlayerHands
    {
        /// <summary>
        /// Maps player name to their Hand.
        /// </summary>
        public Dictionary<string, Hand> Hands { get; } = new Dictionary<string, Hand>();

        /// <summary>
        /// Registers a new player with an empty hand.
        /// </summary>
        public void AddPlayer(string playerName)
        {
            if (!Hands.ContainsKey(playerName))
                Hands[playerName] = new Hand();
        }

        /// <summary>
        /// Returns the Hand for the given player, or null if not found.
        /// </summary>
        public Hand? GetHand(string playerName) =>
            Hands.TryGetValue(playerName, out var hand) ? hand : null;

        /// <summary>
        /// Returns the names of all players who still have cards.
        /// </summary>
        public IEnumerable<string> ActivePlayers =>
            Hands.Where(kvp => !kvp.Value.IsEmpty).Select(kvp => kvp.Key);
    }
}
