using System;
using System.Collections.Generic;

namespace MVC6_Five_Card_Draw.GameLogic
{
    public static class Dealer
    {
        private static System.Security.Cryptography.RandomNumberGenerator rngCsp = System.Security.Cryptography.RandomNumberGenerator.Create();

        public static byte DealCard()
        {
            // Create a byte array to hold the random value.
            byte[] randomCard = new byte[1];
            do
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(randomCard);
            }
            while (!(randomCard[0] < 208));

            // Return the random number mod the number
            // of cards.  
            return (byte)((randomCard[0] % 52));
        }

        public static byte[] DealHand(byte numberOfPlayers)
        {
            if (numberOfPlayers > 7 || numberOfPlayers < 2)
                throw new ArgumentException("There must be at least 2 players and at most 7 players.");
            var cards = new List<byte>();
            while (cards.Count < 5 * numberOfPlayers)
            {
                var card = DealCard();
                if (!cards.Contains(card))
                    cards.Add(card);
            }
            return cards.ToArray();
        }

        public static byte[] DealDraw(List<byte> deadCards)
        {
            var cards = new List<byte>();
            while (cards.Count < 5)
            {
                var card = DealCard();
                if (!cards.Contains(card) && !deadCards.Contains(card))
                    cards.Add(card);
            }
            return cards.ToArray();
        }
    }
}
