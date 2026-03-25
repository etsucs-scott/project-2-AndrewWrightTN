using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame1
{
    /// <summary>
    /// Holds the result of one round 
    /// </summary>
    public class RoundResult
    {
        /// <summary>Round number (1-based).</summary>
        public int RoundNumber { get; init; }

        /// <summary>
        /// A regular round is classified as 0 and a tiebreaker is 1
        /// </summary>
        public List<Dictionary<string, Card>> SubRounds { get; init; } = new();

        /// <summary>
        /// Players involved in each tie, parallel to SubRounds.
        /// </summary>
        public List<List<string>> TiedPlayers { get; init; } = new();

        /// <summary>
        /// Name of the player who won this round and collected the pot.
        /// </summary>
        public string? Winner { get; init; }

        /// <summary>
        /// Card counts for each player after the round resolves.
        /// </summary>
        public Dictionary<string, int> CardCounts { get; init; } = new();

        /// <summary>
        /// All cards that went into the pot this round.
        /// </summary>
        public List<Card> Pot { get; init; } = new();
    }

  
    public class Game
    {
        /// <summary>
        /// Hard cap on rounds to prevent infinite games. I'm still running into a lot of this
        /// </summary>
        public const int MaxRounds = 10_000;

        /// <summary>
        /// Tracks each player's hand.
        /// </summary>
        public PlayerHands PlayerHands { get; }

        /// <summary>Current round number (incremented each call to PlayRound).</summary>
        public int RoundNumber { get; private set; }

        /// <summary>Set when the game ends. Empty string means game is still in progress.</summary>
        public string Winner { get; private set; } = string.Empty;

        /// <summary>True once the game has a winner or the round cap is hit.</summary>
        public bool IsOver { get; private set; }

        private readonly int playerCount;

        /// <summary>
        /// Initializes a new game: creates the deck, registers players,
        /// and deals cards round-robin so early players get any extras.
        /// </summary>
        /// <param name="playerCount">Number of players (2-4).</param>
        public Game(int playerCount)
        {
            this.playerCount = playerCount;
            PlayerHands = new PlayerHands();

            // Register players with default names
            for (int i = 1; i <= playerCount; i++)
                PlayerHands.AddPlayer($"Player {i}");

            DealCards(new Deck());
        }

        /// <summary>
        /// Deals all 52 cards round-robin: one card at a time to each player in order.
        /// With uneven counts (e.g. 52 / 3), the first players receive the extra cards
        /// naturally because the loop simply continues until the deck runs out.
        /// </summary>
        private void DealCards(Deck deck)
        {
            var players = PlayerHands.Hands.Keys.ToList();
            int i = 0;

            // Keep dealing one card at a time in round-robin order until deck is empty
            Card? card;
            while ((card = deck.DrawCard()) != null)
            {
                PlayerHands.GetHand(players[i % playerCount])!.AddCard(card);
                i++;
            }
        }

        /// <summary>
        /// Plays one full round, including any tiebreaker sub-rounds.
        /// All cards played go into the pot; the eventual winner takes it all.
        /// Returns a RoundResult describing everything that happened.
        /// </summary>
        public RoundResult PlayRound()
        {
            RoundNumber++;

            // The pot accumulates ALL cards played across the main round and any tiebreakers
            var pot = new List<Card>();
            var subRounds = new List<Dictionary<string, Card>>();
            var tiedPlayerSets = new List<List<string>>();

            // Start with all active players; narrow down to tied players each sub-round
            var contestants = PlayerHands.ActivePlayers.ToList();

            string? roundWinner = null;

            while (roundWinner == null)
            {
                var played = new PlayedCards();

                // Each contestant plays one card; players who run out are eliminated now
                foreach (var player in contestants.ToList())
                {
                    var card = PlayerHands.GetHand(player)!.PlayCard();
                    if (card == null)
                    {
                        // Player has no tiebreaker card — eliminated immediately
                        contestants.Remove(player);
                    }
                    else
                    {
                        played.Play(player, card);
                        pot.Add(card); // every played card goes into the pot right away
                    }
                }

                // Snapshot what was played this sub-round for display
                subRounds.Add(new Dictionary<string, Card>(played.Cards));

                // If only one contestant remains after eliminations, they win by default
                if (played.Cards.Count == 1)
                {
                    roundWinner = played.Cards.Keys.First();
                    tiedPlayerSets.Add(new List<string>());
                    break;
                }

                // Extremely rare: all remaining tied players ran out simultaneously
                if (played.Cards.Count == 0)
                {
                    tiedPlayerSets.Add(new List<string>());
                    break;
                }

                var topPlayers = played.GetTopPlayers();

                if (topPlayers.Count == 1)
                {
                    // Clear winner this sub-round
                    roundWinner = topPlayers[0];
                    tiedPlayerSets.Add(new List<string>());
                }
                else
                {
                    // Tie — record which players are tied and loop for a tiebreaker
                    tiedPlayerSets.Add(topPlayers);
                    // Only the tied players play the next sub-round
                    contestants = topPlayers;
                }
            }

            // Award the entire pot to the winner
            if (roundWinner != null)
                PlayerHands.GetHand(roundWinner)!.AddCards(pot);

            // Check win condition after awarding pot
            CheckWinCondition();

            return new RoundResult
            {
                RoundNumber = RoundNumber,
                SubRounds = subRounds,
                TiedPlayers = tiedPlayerSets,
                Winner = roundWinner,
                Pot = pot,
                CardCounts = PlayerHands.Hands.ToDictionary(
                                  kvp => kvp.Key, kvp => kvp.Value.Count)
            };
        }

        /// <summary>
        /// Checks whether the game has ended.
        /// Ends when only one player still has cards, or the round cap is reached.
        /// </summary>
        private void CheckWinCondition()
        {
            var active = PlayerHands.ActivePlayers.ToList();

            if (active.Count == 1)
            {
                Winner = active[0];
                IsOver = true;
                return;
            }

            if (RoundNumber >= MaxRounds)
            {
                // Round cap reached — player with most cards wins; draw if still tied
                int maxCards = PlayerHands.Hands.Values.Max(h => h.Count);
                var leaders = PlayerHands.Hands
                    .Where(kvp => kvp.Value.Count == maxCards)
                    .Select(kvp => kvp.Key)
                    .ToList();

                Winner = leaders.Count == 1 ? leaders[0] : "Draw";
                IsOver = true;
            }
        }
    }
}
