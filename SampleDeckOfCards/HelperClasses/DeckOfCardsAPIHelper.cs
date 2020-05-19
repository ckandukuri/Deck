using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Newtonsoft.Json;
using SampleDeckOfCards.Model;

namespace SampleDeckOfCards.HelperClasses
{
    public class DeckOfCardsAPIHelper
    {
        string baseURL = "https://deckofcardsapi.com/";
        BaseClient baseClient = new BaseClient();

        public async Task<DeckOfCards> GetDeckOfCards(bool shuffled, int numberOfDecks, bool includeJokers)
        {
            string strUri = string.Empty;
            if (shuffled)
            {
                 strUri = string.Format("api/deck/new/shuffle/?deck_count={0}",numberOfDecks);
            }
            else
            {
                 strUri = string.Format("api/deck/new/?deck_count={0}", numberOfDecks);
            }

            if(includeJokers)
            {
                strUri = strUri + "&jokers_enabled=true";
            }

            var response = await baseClient.GetCallAsync(baseURL, strUri);
            if (response.StatusCode != (int)HttpStatusCode.OK)
            {
                Assert.Fail("Failed To Get the Deck of cards"); 
            }

            JsonSerializer jsonSerializer = new JsonSerializer();
            return JsonConvert.DeserializeObject<DeckOfCards>(response.ResponseMessage);
        }

        public async Task<DrawCard> DrawCardFromDeck(string deck_id, int numberOfCards)
        {

            //https://deckofcardsapi.com/api/deck/tm14ncsuw9oy/draw/?count=2

            string strUri = string.Format("api/deck/{0}/draw/?count={1}", deck_id, numberOfCards);

         
            var response = await baseClient.GetCallAsync("https://deckofcardsapi.com/", strUri);
            if (response.StatusCode != (int)HttpStatusCode.OK)
            {
                Assert.Fail("Failed To Get the Deck of cards");
            }

            JsonSerializer jsonSerializer = new JsonSerializer();
            return JsonConvert.DeserializeObject<DrawCard>(response.ResponseMessage);
        }

        public async Task<DeckOfCards> PostDeckOfCards(int numberOfDecks, bool includeJokers)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            var deckOfCards = new GetDeckOfCardPost()
            {
                deck_count = numberOfDecks,
                //  jokers_enabled = includeJokers
            };

            var response = await baseClient.PostCallAsync(baseURL, "api/deck/new/shuffle/", JsonConvert.SerializeObject(deckOfCards));

            if (response.StatusCode != (int)HttpStatusCode.OK)
            {
                Assert.Fail("Failed To Get the Deck of cards");
            }
            return JsonConvert.DeserializeObject<DeckOfCards>(response.ResponseMessage);
        }
    }
}
