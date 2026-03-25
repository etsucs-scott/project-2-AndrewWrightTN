using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame1
{
    /// <summary>
    /// Represents a player's hand of cards as a Queue.
    /// </summary>
    public class Hand
    {
        /// <summary>
        /// The player's cards in play order. Queue enforces that the oldest cards are played first.
        /// </summary>
        private Queue<Card> Cards { get; } = new Queue<Card>();

        /// <summary>
        /// Number of cards currently in the hand.
        /// </summary>
        public int Count => Cards.Count;

        /// <summary>
        /// Returns true if the hand has no cards left.
        /// </summary>
        public bool IsEmpty => Cards.Count == 0;

        /// <summary>
        /// Adds a single card to the back of the hand.
        /// </summary>
        public void AddCard(Card card) => Cards.Enqueue(card);

        /// <summary>
        /// Adds a collection of cards to the back of the hand in order.
        /// </summary>
        public void AddCards(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
                Cards.Enqueue(card);
        }

        /// <summary>
        /// Removes and returns the front card of the hand, and returns null if the hand is empty
        /// </summary>
        public Card? PlayCard() => Cards.Count > 0 ? Cards.Dequeue() : null;
    }
}
