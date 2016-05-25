using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC6_Five_Card_Draw.GameLogic
{
    public class HandEvaluator
    {
        private byte[] Cards;
        private byte[] RankSet;
        private byte uniqueRanks;
        public uint calculatedHandValue { get; private set; }
        public string handName { get; private set; }
        private static readonly string[] cardRankNames = new string[13] { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
        private static readonly string[] cardRankNamesPlural = new string[13] { "Twos", "Threes", "Fours", "Fives", "Sixes", "Sevens", "Eights", "Nines", "Tens", "Jacks", "Queens", "Kings", "Aces" };
        private static readonly string[] suitNames = new string[4] { "Clubs", "Diamonds", "Hearts", "Spades" };

        public HandEvaluator(byte card1, byte card2, byte card3, byte card4, byte card5)
        {
            Cards = new byte[5] { card1, card2, card3, card4, card5 };
            foreach (var card in Cards)
                if (card < 0 || card > 51)
                    throw new ArgumentOutOfRangeException("Card values must be in range, 0-51 (0 = 2 of Clubs, 51 = Ace of Spades.)");
            if (Cards.Distinct().Count() != 5)
                throw new ArgumentException("All cards must be unique.");
            Array.Sort(Cards);

            RankSet = new byte[5] { Rank(0), Rank(1), Rank(2), Rank(3), Rank(4) };
            uniqueRanks = (byte)RankSet.Distinct().Count();
            CalculateHand();
        }

        private byte Suit(byte cardNumber)
        {
            return (byte)(Cards[cardNumber] % 4);
        }

        private byte Rank(byte cardNumber)
        {
            return (byte)(Cards[cardNumber] / 4);
        }

        //private bool isRoyalFlush()
        //{
        //    if (RankSet[0] == 8 && isFlush() && isStraight())
        //        return true;
        //    return false;
        //    }

        private bool isStraightFlush()
        {
            if (isStraight() && isFlush())
                return true;
            return false;
        }

        private bool isFlush()
        {
            var suit = Suit(0);
            if (Suit(1) == suit && Suit(2) == suit && Suit(3) == suit && Suit(4) == suit)
                return true;
            return false;
        }

        private bool isStraight()
        {
            if (RankSet[0] > 8)
                return false;
            if (RankSet[0] == 0 && RankSet[1] == 1 && RankSet[2] == 2 && RankSet[3] == 3 && (RankSet[4] == 4 || RankSet[4] == 12))
                return true;
            if (RankSet[1] == RankSet[0] + 1 && RankSet[2] == RankSet[0] + 2 && RankSet[3] == RankSet[0] + 3 && RankSet[4] == RankSet[0] + 4)
                return true;
            return false;
        }

        private bool isPair()
        {
            if (uniqueRanks == 4)
                return true;
            return false;
        }

        private bool isTwoPair()
        {
            if (uniqueRanks == 3 && RankSet[0] != RankSet[2] && RankSet[1] != RankSet[3] && RankSet[2] != RankSet[4])
                return true;
            return false;
        }

        private bool isFullHouse()
        {
            if (uniqueRanks == 2 && RankSet[1] != RankSet[3])
                return true;
            return false;
        }

        private bool isFourOfAKind()
        {
            if (uniqueRanks == 2 && RankSet[1] == RankSet[3])
                return true;
            return false;
        }

        private bool isThreeOfAKind()
        {
            if (uniqueRanks == 3 && (RankSet[0] == RankSet[2] || RankSet[1] == RankSet[3] || RankSet[2] == RankSet[4]))
                return true;
            return false;
        }

        private void CalculateHand()
        {
            byte handRank, rank1, rank2, rank3, rank4, rank5;
            handRank = rank1 = rank2 = rank3 = rank4 = rank5 = 0;
            if (isStraightFlush())
            {
                if (RankSet[0] == 8)
                {
                    handRank = 9;
                    handName = "Royal Flush of " + suitNames[Suit(0)];
                }
                else
                {
                    handRank = 8;
                    if (RankSet[4] == 14 && RankSet[0] == 2)
                        rank1 = 5;
                    else
                        rank1 = RankSet[4];

                    handName = cardRankNames[rank1] + " High Straight Flush of " + suitNames[Suit(0)];
                }
            }
            else if (isFourOfAKind())
            {
                handRank = 7;
                if (RankSet[0] == RankSet[1])
                {
                    rank1 = RankSet[0];
                    rank2 = RankSet[4];
                }
                else
                {
                    rank1 = RankSet[4];
                    rank2 = RankSet[0];
                }
                handName = "Four of a Kind, " + cardRankNamesPlural[rank1];
            }
            else if (isFullHouse())
            {
                handRank = 6;
                if (RankSet[0] == RankSet[2])
                {
                    rank1 = RankSet[0];
                    rank2 = RankSet[4];
                }
                else
                {
                    rank1 = RankSet[4];
                    rank2 = RankSet[0];
                }
                handName = cardRankNames[rank1] + "s Full of " + cardRankNames[rank2] + "s";
            }
            else if (isFlush())
            {
                handRank = 5;
                rank1 = RankSet[4];
                rank2 = RankSet[3];
                rank3 = RankSet[2];
                rank4 = RankSet[1];
                rank5 = RankSet[0];

                handName = cardRankNames[rank1] + " High Flush of " + suitNames[Suit(0)];
            }
            else if (isStraight())
            {
                handRank = 4;
                if (RankSet[4] == 12 && RankSet[0] == 0)
                    rank1 = 5;
                else
                    rank1 = RankSet[4];
                handName = cardRankNames[rank1] + " High Straight";
            }
            else if (isThreeOfAKind())
            {
                handRank = 3;
                if (RankSet[0] == RankSet[2])
                {
                    rank1 = RankSet[0];
                    rank2 = RankSet[4];
                    rank3 = RankSet[3];
                }
                else if (RankSet[1] == RankSet[3])
                {
                    rank1 = RankSet[1];
                    rank2 = RankSet[4];
                    rank2 = RankSet[0];
                }
                else
                {
                    rank1 = RankSet[4];
                    rank2 = RankSet[1];
                    rank3 = RankSet[0];
                }

                handName = "Three of a Kind, " + cardRankNamesPlural[rank1];


            }
            else if (isTwoPair())
            {
                handRank = 2;
                if (RankSet[0] != RankSet[1])
                {
                    rank1 = RankSet[4];
                    rank2 = RankSet[2];
                    rank3 = RankSet[0];
                }
                else if (RankSet[2] != RankSet[3])
                {
                    rank1 = RankSet[4];
                    rank2 = RankSet[0];
                    rank3 = RankSet[2];
                }
                else
                {
                    rank1 = RankSet[2];
                    rank2 = RankSet[0];
                    rank3 = RankSet[4];
                }
                handName = "Two Pair, " + cardRankNamesPlural[rank1] + "and" + cardRankNamesPlural[rank2];
            }
            else if (isPair())
            {
                handRank = 1;
                if (RankSet[0] == RankSet[1])
                {
                    rank1 = RankSet[0];
                    rank2 = RankSet[4];
                    rank3 = RankSet[3];
                    rank4 = RankSet[2];
                }
                else if (RankSet[1] == RankSet[2])
                {
                    rank1 = RankSet[1];
                    rank2 = RankSet[4];
                    rank3 = RankSet[3];
                    rank4 = RankSet[0];
                }
                else if (RankSet[2] == RankSet[3])
                {
                    rank1 = RankSet[2];
                    rank2 = RankSet[4];
                    rank3 = RankSet[1];
                    rank4 = RankSet[0];
                }
                else
                {
                    rank1 = RankSet[3];
                    rank2 = RankSet[2];
                    rank3 = RankSet[1];
                    rank4 = RankSet[0];
                }

                handName = "Pair of " + cardRankNamesPlural[rank1];
            }
            else
            {
                handRank = 0;
                rank1 = RankSet[4];
                rank2 = RankSet[3];
                rank3 = RankSet[2];
                rank4 = RankSet[1];
                rank5 = RankSet[0];

                handName = "High Card, " + cardRankNames[rank1];
            }

            calculatedHandValue = (UInt32)((handRank << 20) | (rank1 << 16) | (rank2 << 12) | (rank3 << 8) | (rank4 << 4) | rank5);

        }
    }
}
