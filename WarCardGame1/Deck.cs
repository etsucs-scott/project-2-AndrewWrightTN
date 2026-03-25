using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame1
{
    /// <summary>
    /// Represents a standard 52-card deck stored as a Stack and shuffled before dealt.
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// Internal stack of cards. Stack gives O(1) draw from the top.
        /// </summary>
        private Stack<Card> Cards { get; }

        /// <summary>
        /// Initializes the deck by generating all 52 cards and shuffling them.
        /// </summary>
        public Deck()
        {
            Cards = new Stack<Card>(GenerateDeck());
        }

        /// <summary>
        /// Builds a complete 52-card set and shuffles it. Uses OrderBy with a random key 
        /// </summary>
        private static List<Card> GenerateDeck()
        {
            var random = new Random();
            var cards = new List<Card>(52);

            // Iterate the combinations of suits and ranks to guarantee exactly 52 unique cards
            foreach (Card.SuitType suit in Enum.GetValues<Card.SuitType>())
                foreach (Card.RankType rank in Enum.GetValues<Card.RankType>())
                    cards.Add(new Card(suit, rank));

            // Shuffle by assigning each card a random sort key
            return cards.OrderBy(_ => random.Next()).ToList();
        }

        /// <summary>
        /// Draws and returns the top card. Returns null if the deck is empty.
        /// </summary>
        public Card? DrawCard() => Cards.Count > 0 ? Cards.Pop() : null;

        /// <summary>
        /// How many cards remain in the deck.
        /// </summary>
        public int Count => Cards.Count;
    }
}
