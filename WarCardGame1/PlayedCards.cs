using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame1
{
    /// <summary>
    /// Tracks which player played which card for display and winner determination.
    /// </summary>
    public class PlayedCards
    {
        /// <summary>
        /// Maps player name to the card they played this sub-round.
        /// </summary>
        public Dictionary<string, Card> Cards { get; } = new Dictionary<string, Card>();

        /// <summary>
        /// Records a card played by the given player.
        /// </summary>
        public void Play(string playerName, Card card) => Cards[playerName] = card;

        /// <summary>
        /// Finds the highest rank among all played cards.Returns the names of all players who played that highest rank.
        /// ... and more than one return results in a tiebreaker!!!
        /// </summary>
        public List<string> GetTopPlayers()
        {
            // Find the highest rank played this round
            var highestRank = Cards.Values.Max(c => c.Rank);

            // Collect everyone who matched that rank — more than one = tie
            return Cards
                .Where(kvp => kvp.Value.Rank == highestRank)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        /// <summary>
        /// Clears all played cards for the next sub-round.
        /// </summary>
        public void Clear() => Cards.Clear();
    }
}
