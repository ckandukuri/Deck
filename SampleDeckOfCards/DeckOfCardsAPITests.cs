using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SampleDeckOfCards.HelperClasses;

namespace DeckOfCardsAPITestAutomation
{
    [TestClass]
    public class DeckOfCardsAPITests
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shuffled">If shuffled is true deck is returned as shuffled deck or else Non Shuffled deck is returned</param>
        /// <param name="numberOfDecks">number of Decks to retrieve</param>
        /// <param name="includeJokers">If true jokes are included in Deck : If Jokers inlcuded One deck returns 54 cards which includes
        /// two jokers else it one deck returns 52 cards only withouth jokers</param>
        [DataTestMethod]
        [DataRow(true,0,true)]
        [DataRow(true,1,true)]
        [DataRow(true, 20,true)]
        [DataRow(true, 0, false)]
        [DataRow(true, 1, false)]
        [DataRow(true, 20, false)]
        [DataRow(false, 0,true)]
        [DataRow(false, 1,true)]
        [DataRow(false, 20,true)]
        [DataRow(false, 0, false)]
        [DataRow(false, 1, false)]
        [DataRow(false, 20, false)]
        [DataRow(true, -1, false)]
        public void ValidateCreateNewDeckDeckOfCards(bool shuffled,int numberOfDecks,bool includeJokers)
        {
            Console.WriteLine(String.Format("Validate Create new Deck of Cards using Shuffled as {0} ,Number of Decks as {1} and includeJokes as {2} ",shuffled,numberOfDecks,includeJokers));
            DeckOfCardsAPIHelper deckofCardsAPI = new DeckOfCardsAPIHelper();
            var task = deckofCardsAPI.GetDeckOfCards(shuffled,numberOfDecks,includeJokers);
            task.Wait();
            var result = task.Result;

            //Validate New Deck Data
            Assert.IsNotNull(result, "Get Deck of API cards is not null.");
            Assert.AreEqual<bool>(result.success, true);
            if (numberOfDecks >= 0)
                Assert.AreEqual<int>(result.remaining, includeJokers ? ((numberOfDecks * 52) + (numberOfDecks * 2)) : (numberOfDecks * 52));
            else
                Assert.AreEqual<int>(result.remaining, 0);
            Assert.IsNotNull(result.deck_id);
            Assert.AreEqual<bool>(result.shuffled, shuffled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shuffled">If shuffled is true deck is returned as shuffled deck or else Non Shuffled deck is returned</param>
        /// <param name="numberOfDecks">number of Decks to retrieve</param>
        /// <param name="includeJokers">If true jokes are included in Deck : If Jokers inlcuded One deck returns 54 cards which includes
        /// two jokers else it one deck returns 52 cards only withouth jokers</param>
        /// <param name="draw1">First set of count to Draw</param>
        /// <param name="draw2">Second set of count to Draw</param>
        [DataTestMethod]
        [DataRow(true, 1,true,2,5)]
        [DataRow(true, 2, true, 3, 8)]
        [DataRow(false, 3, true, 0, 8)]
        [DataRow(true, 1, true, 2, 0)]
        [DataRow(true, 1, false, 2, 5)]
        [DataRow(true, 2, false, 3, 8)]
        [DataRow(false, 3, false, 0, 8)]
        [DataRow(true, 1, false, 2, 0)]
        public void ValidateDrawCardsFromDeck(bool shuffled, int numberOfDecks, bool includeJokers,int draw1,int draw2)
        {
            Console.WriteLine(String.Format("Validate Draw a card from Deck of Cards using Shuffled as {0} ,Number of Decks as {1} , includeJokes as {2} ,First draw with {3} and Second draw with {4}", shuffled, numberOfDecks, includeJokers,draw1,draw2));

            int totalNumberOfExpectedCards = (numberOfDecks * 52) + (includeJokers ? (numberOfDecks * 2):0);
            DeckOfCardsAPIHelper deckofCardsAPI = new DeckOfCardsAPIHelper();
            var task = deckofCardsAPI.GetDeckOfCards(shuffled, numberOfDecks,includeJokers);
            task.Wait();
            var result = task.Result;
            Assert.IsNotNull(result, "Get Deck of API cards is not null.");
            Assert.AreEqual<bool>(result.success, true);
            
            //Retreive first set of cards from Deck
            var drawCard = deckofCardsAPI.DrawCardFromDeck(result.deck_id, draw1);
            drawCard.Wait();
            var drawCardResult = drawCard.Result;
            Assert.IsNotNull(drawCardResult,String.Format("API returned null when Tried to Draw Card from Deck {0}",result.deck_id));
            Assert.AreEqual(drawCardResult.success,true);
            Assert.IsNotNull(drawCardResult.cards,"API returned no cards when tried to draw card from Deck {0}",result.deck_id);
            Assert.AreEqual(drawCardResult.deck_id,result.deck_id);
            Assert.AreEqual(drawCardResult.remaining,totalNumberOfExpectedCards-draw1);
            
            //Retreive first set of cards from Deck
            drawCard = deckofCardsAPI.DrawCardFromDeck(result.deck_id, draw2);
            drawCard.Wait();
            drawCardResult = drawCard.Result;
            Assert.IsNotNull(drawCardResult, String.Format("API returned null when Tried to Draw Card from Deck {0}", result.deck_id));
            Assert.AreEqual(drawCardResult.success, true);
            Assert.IsNotNull(drawCardResult.cards, "API returned no cards when tried to draw card from Deck {0}", result.deck_id);
            Assert.AreEqual(drawCardResult.deck_id, result.deck_id);
            Assert.AreEqual(drawCardResult.remaining, totalNumberOfExpectedCards - draw1-draw2);

        }

        [TestMethod]
        public void ValidateCreateNewDeckDeckOfCards_POST()
        {
            DeckOfCardsAPIHelper deckofCardsAPI = new DeckOfCardsAPIHelper();
            var task = deckofCardsAPI.PostDeckOfCards(1, true);
            task.Wait();
            var result = task.Result;

            Assert.IsNotNull(result, "Get Deck of API cards is not null.");
            Assert.AreEqual<int>(result.remaining, 52);
            Assert.IsNotNull(result.deck_id);
            Assert.AreEqual<bool>(result.shuffled, true);
            Assert.AreEqual<bool>(result.success, true);
        }
    }
}

