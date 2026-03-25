using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame1
{
    /// <summary>
    /// Represents a single playing card with a suit and rank.
    /// Cards are comparable by rank only — suits are ignored in War.
    /// </summary>
    public class Card : IComparable<Card>
    {
        /// <summary>
        /// The four standard suits.
        /// </summary>
        public enum SuitType { Heart, Diamond, Club, Spade }

        /// <summary>
        /// Card ranks from lowest (Two=2) to highest (Ace=14). Integer values allow direct comparison via CompareTo.
        /// </summary>
        public enum RankType
        {
            Two = 2, Three, Four, Five, Six, Seven, Eight,
            Nine, Ten, Jack, Queen, King, Ace
        }

        /// <summary>
        /// The suit of this card (ignored during comparisons).
        /// </summary>
        public SuitType Suit { get; }

        /// <summary>
        /// The rank of this card (used for all comparisons).
        /// </summary>
        public RankType Rank { get; }

        /// <summary>
        /// Creates a card with the given suit and rank.
        /// </summary>
        public Card(SuitType suit, RankType rank)
        {
            Suit = suit;
            Rank = rank;
        }

        /// <summary>
        /// Compares this card to another by rank only. Returns positive if this card outranks the other.
        /// </summary>
        public int CompareTo(Card? other)
        {
            if (other == null) return 1;
            return Rank.CompareTo(other.Rank);
        }

        /// <summary>
        /// Returns a human readable card name, like "Ace of Spades".
        /// </summary>
        public override string ToString() => $"{Rank} of {Suit}s";
    }
}
