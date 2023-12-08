using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day07
    {
        class HandPart1
        {
            private static List<string> CardValues => new() { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };
            public string Cards { get; set; } = "";
            public int Bid { get; set; }
            public bool IsFiveOfKind { get; private set; }
            public bool IsFourOfKind { get; private set; }
            public bool IsFullHouse { get; private set; }
            public bool IsThreeOfKind { get; private set; }
            public bool IsTwoPairs { get; private set; }
            public bool IsOnePair { get; private set; }
            public int HandStrength { get; private set; }
            public List<int> CardsStrength { get; private set; } = new List<int>();

            protected HandPart1() { }

            public HandPart1(string cards, int bid)
            {
                Cards = cards;
                Bid = bid;

                EvaluateHandStrength(Cards);
                EvaluateCardsStrength(Cards, CardValues);
            }

            protected void EvaluateHandStrength(string cards)
            {
                var _cardGroups = cards.ToCharArray().GroupBy(c => c).ToList();

                IsFiveOfKind = _cardGroups.Any(g => g.Count() == 5);
                IsFourOfKind = !IsFiveOfKind && _cardGroups.Any(g => g.Count() == 4);
                IsFullHouse = !IsFourOfKind && _cardGroups.Count(g => g.Count() == 3) == 1 && _cardGroups.Count(g => g.Count() == 2) == 1;
                IsThreeOfKind = !IsFullHouse && _cardGroups.Any(g => g.Count() == 3);
                IsTwoPairs = !IsThreeOfKind && _cardGroups.Count(g => g.Count() == 2) == 2;
                IsOnePair = !IsTwoPairs && _cardGroups.Count(g => g.Count() == 2) == 1 && _cardGroups.Count(g => g.Count() == 1) == 3;

                HandStrength = IsFiveOfKind ? 7
                    : IsFourOfKind ? 6
                    : IsFullHouse ? 5
                    : IsThreeOfKind ? 4
                    : IsTwoPairs ? 3
                    : IsOnePair ? 2
                    : 1;
            }

            protected void EvaluateCardsStrength(string cards, List<string> cardValues)
            {
                CardsStrength = cards.ToCharArray().Select(x => cardValues.IndexOf(x.ToString())).ToList();
            }
        }

        class HandPart2 : HandPart1
        {
            private static List<string> CardValuesPart2 => new() { "J", "2", "3", "4", "5", "6", "7", "8", "9", "T", "Q", "K", "A" };
            public string OriginalCards { get; set; } = "";

            public HandPart2(string cards, int bid)
            {
                OriginalCards = cards;
                Bid = bid;

                var strongestCardValue = GetStrongestCardValue();
                Cards = OriginalCards.Replace('J', strongestCardValue);

                EvaluateHandStrength(Cards);
                EvaluateCardsStrength(OriginalCards, CardValuesPart2);
            }

            private char GetStrongestCardValue()
            {
                var cardGroups = OriginalCards.ToCharArray().Where(c => c != 'J').GroupBy(c => c).ToList();
                var orderByCount = cardGroups.OrderBy(c => c.Count());
                var orderByStrength = orderByCount.ThenBy(c => CardValuesPart2.IndexOf(c.Key.ToString()));
                return orderByStrength.LastOrDefault()?.Key ?? 'A';
            }
        }

        class HandComparer : IComparer<HandPart1>
        {
            public int Compare(HandPart1? x, HandPart1? y)
            {
                if (x == null) return -1;
                if (y == null) return 1;

                var strength = x.HandStrength.CompareTo(y.HandStrength);

                if (strength != 0) return strength;

                for (int i = 0; i < x.CardsStrength.Count; i++)
                {
                    var cardStrength = x.CardsStrength[i].CompareTo(y.CardsStrength[i]);

                    if (cardStrength != 0) return cardStrength;
                }

                return 0;
            }
        }

        [Fact]
        public void Part1()
        {
            var lines = File.ReadLines("./inputs/day07/input.txt").ToList();
            var hands = lines.Select(x => x.Split(' ')).Select(x => new HandPart1(x[0], int.Parse(x[1]))).ToList();

            hands.Sort(new HandComparer());

            var total = 0;

            for (int i = 1; i <= hands.Count; i++)
            {
                total += i * hands[i - 1].Bid;
            }

            Assert.Equal(255048101, total);
        }

        [Fact]
        public void Part2()
        {
            var lines = File.ReadLines("./inputs/day07/input.txt").ToList();
            var hands = lines.Select(x => x.Split(' ')).Select(x => new HandPart2(x[0], int.Parse(x[1]))).ToList();

            hands.Sort(new HandComparer());

            var total = 0;

            for (int i = 1; i <= hands.Count; i++)
            {
                total += i * hands[i - 1].Bid;
            }

            Assert.Equal(253718286, total);
        }
    }
}
