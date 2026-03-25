using WarCardGame1;

namespace WarCardGame1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int playerCount = ParsePlayerCount(args);

            Console.WriteLine("##############################");
            Console.WriteLine("        WAR — Card Game          ");
            Console.WriteLine("##############################");
            Console.WriteLine($"Players  : {playerCount}");
            Console.WriteLine("##############################\n");

            var game = new Game(playerCount);

            // Print initial hand sizes to confirm dealing
            Console.WriteLine("Deal:");
            foreach (var kvp in game.PlayerHands.Hands)
                Console.WriteLine($"  {kvp.Key}: {kvp.Value.Count} cards");
            Console.WriteLine();

            while (!game.IsOver)
            {
                var result = game.PlayRound();
                PrintRound(result);
            }

            //Final result
            Console.WriteLine("##############################");
            if (game.Winner == "Draw")
                Console.WriteLine($"  DRAW after {game.RoundNumber:N0} rounds — no winner.");
            else
                Console.WriteLine($"  {game.Winner} wins after {game.RoundNumber:N0} rounds!");
            Console.WriteLine("##############################");
        }

        /// <summary>
        /// Prints a complete round to the console 
        /// </summary>
        static void PrintRound(RoundResult result)
        {
            Console.WriteLine($"Round {result.RoundNumber}");

            for (int i = 0; i < result.SubRounds.Count; i++)
            {
                var subRound = result.SubRounds[i];
                var tied = result.TiedPlayers[i];

                if (i == 0)
                {
                    // Main round — print every player's card
                    foreach (var kvp in subRound)
                        Console.WriteLine($"  {kvp.Key}: {kvp.Value.Rank}");
                }
                else
                {
                    // Tiebreaker sub-round
                    var tbLine = string.Join(" | ", subRound.Select(kvp => $"{kvp.Key}: {kvp.Value.Rank}"));
                    Console.WriteLine($"  Tiebreaker: {tbLine}");
                }

                // Print tie notification and pot contents before the next sub-round
                if (tied.Count > 1)
                {
                    Console.WriteLine($"  Tie between {string.Join(" and ", tied)}!");
                    var potDisplay = string.Join(", ", result.Pot.Select(c => c.Rank.ToString()));
                    Console.WriteLine($"  Pot includes: {potDisplay}");
                }
            }

            // Winner line with card counts — e.g. "Winner: Player 1 (Cards: P1=26, P2=12)"
            if (result.Winner != null)
            {
                var counts = string.Join(", ", result.CardCounts.Select(kvp =>
                    $"P{kvp.Key.Replace("Player ", "")}={kvp.Value}"));
                Console.WriteLine($"  Winner: {result.Winner} (Cards: {counts})");
            }
            else
            {
                Console.WriteLine("  No winner this round (all tied players eliminated).");
            }
            Console.WriteLine("  Press any key for next round...");
            Console.ReadKey(true); // true = don't show the key they pressed
            Console.WriteLine();

            Console.WriteLine();
        }

        /// <summary>
        /// Reads player count 
        /// </summary>
        static int ParsePlayerCount(string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int argCount)
                && argCount >= 2 && argCount <= 4)
                return argCount;

            int count = 0;
            while (count < 2 || count > 4)
            {
                Console.Write("Enter number of players (2-4): ");
                var input = Console.ReadLine();
                if (!int.TryParse(input, out count) || count < 2 || count > 4)
                {
                    Console.WriteLine("  Please enter 2, 3, or 4.");
                    count = 0;
                }
            }
            return count;
        }
    }
}


