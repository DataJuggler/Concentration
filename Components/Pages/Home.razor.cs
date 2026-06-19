

#region using statements

using DataJuggler.Blazor.Components;
using DataJuggler.Blazor.Components.Interfaces;
using DataJuggler.PlayingCards;
using DataJuggler.PlayingCards.Enumerations;
using DataJuggler.PlayingCards.Objects;
using System.Runtime.Versioning;
using DataJuggler.RandomShuffler.Objects;
using DataJuggler.UltimateHelper;
using DataJuggler.BlazorAudio;

#endregion

namespace Concentration.Components.Pages
{

    #region class Home
    /// <summary>
    /// This class is the main page for this app
    /// </summary>
    [SupportedOSPlatform("Windows")]
    public partial class Home : IBlazorComponentParent
    {
        
        #region Private Variables
        private int consectutiveCards;
        private int inning;
        private int tricks;
        private CardInfo firstCardTurnedOver;
        private List<CardInfo> cardsTurnedOverThisInning;
        private Dealer dealer;
        private string noBorderStyle;
        private ToggleComponent soundOption;
        private ImageButton cardButton1;
        private ImageButton cardButton10;
        private ImageButton cardButton11;
        private ImageButton cardButton12;
        private ImageButton cardButton13;
        private ImageButton cardButton14;
        private ImageButton cardButton15;
        private ImageButton cardButton16;
        private ImageButton cardButton17;
        private ImageButton cardButton18;
        private ImageButton cardButton19;
        private ImageButton cardButton2;
        private ImageButton cardButton20;
        private ImageButton cardButton21;
        private ImageButton cardButton22;
        private ImageButton cardButton23;
        private ImageButton cardButton24;
        private ImageButton cardButton25;
        private ImageButton cardButton26;
        private ImageButton cardButton27;
        private ImageButton cardButton28;
        private ImageButton cardButton29;
        private ImageButton cardButton3;
        private ImageButton cardButton30;
        private ImageButton cardButton31;
        private ImageButton cardButton32;
        private ImageButton cardButton33;
        private ImageButton cardButton34;
        private ImageButton cardButton35;
        private ImageButton cardButton36;
        private ImageButton cardButton37;
        private ImageButton cardButton38;
        private ImageButton cardButton39;
        private ImageButton cardButton4;
        private ImageButton cardButton40;
        private ImageButton cardButton41;
        private ImageButton cardButton42;
        private ImageButton cardButton43;
        private ImageButton cardButton44;
        private ImageButton cardButton45;
        private ImageButton cardButton46;
        private ImageButton cardButton47;
        private ImageButton cardButton48;
        private ImageButton cardButton49;
        private ImageButton cardButton5;
        private ImageButton cardButton50;
        private ImageButton cardButton51;
        private ImageButton cardButton52;
        private ImageButton cardButton6;
        private ImageButton cardButton7;
        private ImageButton cardButton8;
        private ImageButton cardButton9;
        private BlazorAudioPlayer audioPlayer;
        private ImageComponent logo;
        private ImageButton newGameButton;
        private Label inningLabel;
        private Label tricksLabel;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of a Home object
        /// </summary>
        public Home()
        {
            // Create the Dealer object
            Dealer = new Dealer(PlatformEnum.Blazor, DeckEnum.TheGildedDeck, CardBackEnum.EmeraldLabyrinth);

            // Create the list
            CardsTurnedOverThisInning = new List<CardInfo>();
        }
        #endregion

        #region Events

        #region ButtonClicked(int buttonNumber, string buttonText)
        /// <summary>
        /// This event is fired when Button Clicked
        /// </summary>
        public void ButtonClicked(int buttonNumber, string buttonText)
        {
            // if the new game button was cleared
            if (buttonNumber == 100)
            {
                if (HasDealer)
                {
                    // Reset
                    Inning = 0;
                    Tricks = 0;

                    // if the InningLabel exists
                    if (HasInningLabel)
                    {
                        // Display the Inning value
                        InningLabel.SetTextValue("Inning: " + Inning);

                        // Refresh
                        InningLabel.Refresh();
                    }

                    // if the value for HasTricksLabel is true
                    if (HasTricksLabel)
                    {
                        // Display the Tricks value
                        TricksLabel.SetTextValue("Tricks: " + Tricks);

                        // Refresh
                        TricksLabel.Refresh();
                    }

                    // Shuffle the cards
                    Dealer.Shuffle();

                    for (int x = 1; x <= 52; x++)
                    {
                        // get this card
                        Card card = Dealer.Shuffler.Cards[x - 1];

                        // if the card exists
                        if (NullHelper.Exists(card))
                        {
                            // not exposed
                            card.Exposed = false;

                            // find the card button
                            ImageButton cardButton = this.GetCardButton(x);

                            // if the button was found
                            if (NullHelper.Exists(cardButton))
                            {
                                // set the image url
                                cardButton.SetImageUrl(Dealer.CardBackImage.Path);

                                // show this card
                                cardButton.SetVisible(true);                        
                            }
                        }
                    }
                }
            }
            else
            {
                 // find the cardButton
                ImageButton cardButton = GetCardButton(buttonNumber);

                // get the card
                Card card = Dealer.Shuffler.Cards[buttonNumber -1];

                // toggle
                card.Exposed = !card.Exposed;

                // create thisCard
                CardInfo thisCard = new CardInfo();
                thisCard.Card = card.CardName;
                thisCard.Suit = card.Suit;
                thisCard.CardButton = cardButton;

                // if the cardButton exists
                if (NullHelper.Exists(cardButton))
                {
                    // If the value for the property card.Exposed is true
                    if (card.Exposed)
                    {
                        // Load this Card (Sets the Path)
                        Dealer.LoadCard(card);

                        // get the path to this card
                        string path = card.Path;

                        // Set the path
                        cardButton.SetImageUrl(path);

                        // update the card button
                        cardButton.Refresh();
                    }
                    else
                    {
                        // Set the path
                        cardButton.SetImageUrl(Dealer.CardBackImage.Path);
                    }
                }

                // If this is the first card turned over this inning
                // There is not a FirstCardTurnedOver yet.
                if (NullHelper.IsNull(FirstCardTurnedOver))
                {
                    // Increment the value for Inning
                    Inning++;

                    // if the value for HasInningLabel is true
                    if (HasInningLabel)
                    {
                        // Display the Inning Change
                        InningLabel.SetTextValue("Inning: " + Inning);

                        // Update
                        InningLabel.Refresh();
                    }

                    // Recreate the list
                    CardsTurnedOverThisInning = new List<CardInfo>();

                    // Set the FirstCard
                    FirstCardTurnedOver = thisCard;

                    // add this card
                    CardsTurnedOverThisInning.Add(thisCard);
                }
                else if ((card.CardName == FirstCardTurnedOver.Card) && (!CardsTurnedOverThisInning.Any(x => x.Suit == card.Suit)))
                {
                    // continue this inning

                    // add this card
                    CardsTurnedOverThisInning.Add(thisCard);

                    // if all four were added
                    if (CardsTurnedOverThisInning.Count == 4)
                    {
                        // Increment the value for Tricks
                        Tricks++;

                        // if the value for HasTricksLabel is true
                        if (HasTricksLabel)
                        {
                            // Display the Tricks value                                
                            TricksLabel.SetTextValue("Tricks: " + Tricks);

                            // Refresh
                            TricksLabel.Refresh();
                        }

                        // if the Audio Player exists and PlaySound is true
                        if ((HasAudioPlayer) && (PlaySound))
                        {
                            // get the current directory
                            string directory = Environment.CurrentDirectory;

                            // set the audioFile
                            string audioFile = Path.Combine(directory, @"wwwroot\Sounds\Success1.mp3");

                            // Set the AudioUrl
                            AudioPlayer.SetAudioUrl(audioFile);

                            // Play
                            AudioPlayer.PlayOrPause();
                        }

                        // now hide the four cards

                        // Iterate the collection of ImageButton objects
                        foreach (CardInfo cardInfo in CardsTurnedOverThisInning.ToList())
                        {
                            // change back to a card back (delayed)
                            cardInfo.CardButton.SetVisible(false);
                        }

                        // reset
                        FirstCardTurnedOver = null;
                        CardsTurnedOverThisInning = new List<CardInfo>();
                    }
                }
                else
                {
                    //// if the Audio Player exists
                    //if (HasAudioPlayer)
                    //{
                    //    // Set the AudioUrl
                    //    AudioPlayer.SetAudioUrl(@"C:\Projects\GitHub\Concentration\wwwroot\Failure1.mp3");

                    //    // Play
                    //    AudioPlayer.PlayOrPause();
                    //}

                    // add this card
                    CardsTurnedOverThisInning.Add(thisCard);

                    // Display the new Inning
                    this.InningLabel.SetTextValue("Inning: " + Inning);

                    // If the CardsTurnedOverThisInning collection exists and has one or more items
                    if (ListHelper.HasOneOrMoreItems(CardsTurnedOverThisInning))
                    {
                        // Iterate the collection of ImageButton objects
                        foreach (CardInfo cardInfo in CardsTurnedOverThisInning.ToList())
                        {
                            // We must find the card for this cardButton and set it to Exposed = false;
                            Card tempCard = Dealer.Shuffler.Cards[cardInfo.CardButton.ButtonNumber - 1];

                            // reset
                            tempCard.Exposed = false;

                            // change back to a card back (delayed)
                            _ = cardInfo.CardButton.SetImageUrlWithDelay(Dealer.CardBackImage.Path, 1600);
                        }
                    }

                    // Start a new inning, by setting this to null
                    FirstCardTurnedOver = null;

                    // reset
                    CardsTurnedOverThisInning = new List<CardInfo>();
                }

                
            }
        }
        #endregion
            
        #endregion
        
        #region Methods
            
            #region GetImageButton()
            /// <summary>
            /// returns the Image Button for the buttonNumber
            /// </summary>
            public ImageButton GetCardButton(int buttonNumber)
            {
                // initial value
                ImageButton cardButton = null;

                switch (buttonNumber)
                {
                    case 1:

                        // set the return value
                        cardButton = CardButton1;

                        // required
                        break;

                    case 2:

                        // set the return value
                        cardButton = CardButton2;

                        // required
                        break;

                    case 3:

                        // set the return value
                        cardButton = CardButton3;

                        // required
                        break;

                    case 4:

                        // set the return value
                        cardButton = CardButton4;

                        // required
                        break;

                    case 5:

                        // set the return value
                        cardButton = CardButton5;

                        // required
                        break;

                    case 6:

                        // set the return value
                        cardButton = CardButton6;

                        // required
                        break;

                    case 7:

                        // set the return value
                        cardButton = CardButton7;

                        // required
                        break;

                    case 8:

                        // set the return value
                        cardButton = CardButton8;

                        // required
                        break;

                    case 9:

                        // set the return value
                        cardButton = CardButton9;

                        // required
                        break;

                    case 10:

                        // set the return value
                        cardButton = CardButton10;

                        // required
                        break;

                    case 11:

                        // set the return value
                        cardButton = CardButton11;

                        // required
                        break;

                    case 12:

                        // set the return value
                        cardButton = CardButton12;

                        // required
                        break;

                    case 13:

                        // set the return value
                        cardButton = CardButton13;

                        // required
                        break;

                    case 14:

                        // set the return value
                        cardButton = CardButton14;

                        // required
                        break;

                    case 15:

                        // set the return value
                        cardButton = CardButton15;

                        // required
                        break;

                    case 16:

                        // set the return value
                        cardButton = CardButton16;

                        // required
                        break;

                    case 17:

                        // set the return value
                        cardButton = CardButton17;

                        // required
                        break;

                    case 18:

                        // set the return value
                        cardButton = CardButton18;

                        // required
                        break;

                    case 19:

                        // set the return value
                        cardButton = CardButton19;

                        // required
                        break;

                    case 20:

                        // set the return value
                        cardButton = CardButton20;

                        // required
                        break;

                    case 21:

                        // set the return value
                        cardButton = CardButton21;

                        // required
                        break;

                    case 22:

                        // set the return value
                        cardButton = CardButton22;

                        // required
                        break;

                    case 23:

                        // set the return value
                        cardButton = CardButton23;

                        // required
                        break;

                    case 24:

                        // set the return value
                        cardButton = CardButton24;

                        // required
                        break;

                    case 25:

                        // set the return value
                        cardButton = CardButton25;

                        // required
                        break;

                    case 26:

                        // set the return value
                        cardButton = CardButton26;

                        // required
                        break;

                    case 27:

                        // set the return value
                        cardButton = CardButton27;

                        // required
                        break;

                    case 28:

                        // set the return value
                        cardButton = CardButton28;

                        // required
                        break;

                    case 29:

                        // set the return value
                        cardButton = CardButton29;

                        // required
                        break;

                    case 30:

                        // set the return value
                        cardButton = CardButton30;

                        // required
                        break;

                    case 31:

                        // set the return value
                        cardButton = CardButton31;

                        // required
                        break;

                    case 32:

                        // set the return value
                        cardButton = CardButton32;

                        // required
                        break;

                    case 33:

                        // set the return value
                        cardButton = CardButton33;

                        // required
                        break;

                    case 34:

                        // set the return value
                        cardButton = CardButton34;

                        // required
                        break;

                    case 35:

                        // set the return value
                        cardButton = CardButton35;

                        // required
                        break;

                    case 36:

                        // set the return value
                        cardButton = CardButton36;

                        // required
                        break;

                    case 37:

                        // set the return value
                        cardButton = CardButton37;

                        // required
                        break;

                    case 38:

                        // set the return value
                        cardButton = CardButton38;

                        // required
                        break;

                    case 39:

                        // set the return value
                        cardButton = CardButton39;

                        // required
                        break;

                    case 40:

                        // set the return value
                        cardButton = CardButton40;

                        // required
                        break;

                    case 41:

                        // set the return value
                        cardButton = CardButton41;

                        // required
                        break;

                    case 42:

                        // set the return value
                        cardButton = CardButton42;

                        // required
                        break;

                    case 43:

                        // set the return value
                        cardButton = CardButton43;

                        // required
                        break;

                    case 44:

                        // set the return value
                        cardButton = CardButton44;

                        // required
                        break;

                    case 45:

                        // set the return value
                        cardButton = CardButton45;

                        // required
                        break;

                    case 46:

                        // set the return value
                        cardButton = CardButton46;

                        // required
                        break;

                    case 47:

                        // set the return value
                        cardButton = CardButton47;

                        // required
                        break;

                    case 48:

                        // set the return value
                        cardButton = CardButton48;

                        // required
                        break;

                    case 49:

                        // set the return value
                        cardButton = CardButton49;

                        // required
                        break;

                    case 50:

                        // set the return value
                        cardButton = CardButton50;

                        // required
                        break;

                    case 51:

                        // set the return value
                        cardButton = CardButton51;

                        // required
                        break;

                    case 52:

                        // set the return value
                        cardButton = CardButton52;

                        // required
                        break;
                }

                // return value
                return cardButton;
            }
            #endregion
            
            #region ReceiveData(Message message)
            /// <summary>
            /// This method is used to receive messages from other components or pages
            /// </summary>
            public void ReceiveData(Message message)
            {

            }
            #endregion
            
            #region Refresh()
            /// <summary>
            /// This method is used to update the UI after changes
            /// </summary>
            public void Refresh()
            {
                InvokeAsync(() =>
                {
                    // Refresh
                    StateHasChanged();
                });
            }
            #endregion
            
            #region Register(IBlazorComponent component)
            /// <summary>
            /// This method is used to store child controls that are on this component or page
            /// </summary>
            public void Register(IBlazorComponent component)
            { 
                if (component is ToggleComponent tempToggleComponent)
                {
                    // Set the SoundOption
                    SoundOption = tempToggleComponent;    
                }
                if (component is BlazorAudioPlayer tempAudioPlayer)
                {
                    // Set the AudioPlayer
                    AudioPlayer = tempAudioPlayer;

                    // hide
                    AudioPlayer.SetVisible(false);
                }
                if (component is ImageButton tempImageButton)
                {
                    if (tempImageButton.ButtonNumber < 100)
                    {
                        // hide all the card buttons for now
                        tempImageButton.SetVisible(false);
                    }

                    // store the ImageButton components
                    if (component.Name == "NewGameButton")
                    {
                        NewGameButton = tempImageButton;
                    }
                    else if (component.Name == "CardButton9")
                    {
                        CardButton9 = tempImageButton;
                    }
                    else if (component.Name == "CardButton8")
                    {
                        CardButton8 = tempImageButton;
                    }
                    else if (component.Name == "CardButton7")
                    {
                        CardButton7 = tempImageButton;
                    }
                    else if (component.Name == "CardButton6")
                    {
                        CardButton6 = tempImageButton;
                    }
                    else if (component.Name == "CardButton51")
                    {
                        CardButton51 = tempImageButton;
                    }
                    else if (component.Name == "CardButton52")
                    {
                        CardButton52 = tempImageButton;
                    }
                    else if (component.Name == "CardButton50")
                    {
                        CardButton50 = tempImageButton;
                    }
                    else if (component.Name == "CardButton5")
                    {
                        CardButton5 = tempImageButton;
                    }
                    else if (component.Name == "CardButton49")
                    {
                        CardButton49 = tempImageButton;
                    }
                    else if (component.Name == "CardButton48")
                    {
                        CardButton48 = tempImageButton;
                    }
                    else if (component.Name == "CardButton47")
                    {
                        CardButton47 = tempImageButton;
                    }
                    else if (component.Name == "CardButton46")
                    {
                        CardButton46 = tempImageButton;
                    }
                    else if (component.Name == "CardButton45")
                    {
                        CardButton45 = tempImageButton;
                    }
                    else if (component.Name == "CardButton44")
                    {
                        CardButton44 = tempImageButton;
                    }
                    else if (component.Name == "CardButton43")
                    {
                        CardButton43 = tempImageButton;
                    }
                    else if (component.Name == "CardButton42")
                    {
                        CardButton42 = tempImageButton;
                    }
                    else if (component.Name == "CardButton41")
                    {
                        CardButton41 = tempImageButton;
                    }
                    else if (component.Name == "CardButton40")
                    {
                        CardButton40 = tempImageButton;
                    }
                    else if (component.Name == "CardButton4")
                    {
                        CardButton4 = tempImageButton;
                    }
                    else if (component.Name == "CardButton39")
                    {
                        CardButton39 = tempImageButton;
                    }
                    else if (component.Name == "CardButton38")
                    {
                        CardButton38 = tempImageButton;
                    }
                    else if (component.Name == "CardButton37")
                    {
                        CardButton37 = tempImageButton;
                    }
                    else if (component.Name == "CardButton36")
                    {
                        CardButton36 = tempImageButton;
                    }
                    else if (component.Name == "CardButton35")
                    {
                        CardButton35 = tempImageButton;
                    }
                    else if (component.Name == "CardButton34")
                    {
                        CardButton34 = tempImageButton;
                    }
                    else if (component.Name == "CardButton33")
                    {
                        CardButton33 = tempImageButton;
                    }
                    else if (component.Name == "CardButton32")
                    {
                        CardButton32 = tempImageButton;
                    }
                    else if (component.Name == "CardButton31")
                    {
                        CardButton31 = tempImageButton;
                    }
                    else if (component.Name == "CardButton30")
                    {
                        CardButton30 = tempImageButton;
                    }
                    else if (component.Name == "CardButton3")
                    {
                        CardButton3 = tempImageButton;
                    }
                    else if (component.Name == "CardButton29")
                    {
                        CardButton29 = tempImageButton;
                    }
                    else if (component.Name == "CardButton28")
                    {
                        CardButton28 = tempImageButton;
                    }
                    else if (component.Name == "CardButton27")
                    {
                        CardButton27 = tempImageButton;
                    }
                    else if (component.Name == "CardButton26")
                    {
                        CardButton26 = tempImageButton;
                    }
                    else if (component.Name == "CardButton25")
                    {
                        CardButton25 = tempImageButton;
                    }
                    else if (component.Name == "CardButton24")
                    {
                        CardButton24 = tempImageButton;
                    }
                    else if (component.Name == "CardButton23")
                    {
                        CardButton23 = tempImageButton;
                    }
                    else if (component.Name == "CardButton22")
                    {
                        CardButton22 = tempImageButton;
                    }
                    else if (component.Name == "CardButton21")
                    {
                        CardButton21 = tempImageButton;
                    }
                    else if (component.Name == "CardButton20")
                    {
                        CardButton20 = tempImageButton;
                    }
                    else if (component.Name == "CardButton2")
                    {
                        CardButton2 = tempImageButton;
                    }
                    else if (component.Name == "CardButton19")
                    {
                        CardButton19 = tempImageButton;
                    }
                    else if (component.Name == "CardButton18")
                    {
                        CardButton18 = tempImageButton;
                    }
                    else if (component.Name == "CardButton17")
                    {
                        CardButton17 = tempImageButton;
                    }
                    else if (component.Name == "CardButton16")
                    {
                        CardButton16 = tempImageButton;
                    }
                    else if (component.Name == "CardButton15")
                    {
                        CardButton15 = tempImageButton;
                    }
                    else if (component.Name == "CardButton14")
                    {
                        CardButton14 = tempImageButton;
                    }
                    else if (component.Name == "CardButton13")
                    {
                        CardButton13 = tempImageButton;
                    }
                    else if (component.Name == "CardButton12")
                    {
                        CardButton12 = tempImageButton;
                    }
                    else if (component.Name == "CardButton11")
                    {
                        CardButton11 = tempImageButton;
                    }
                    else if (component.Name == "CardButton10")
                    {
                        CardButton10 = tempImageButton;
                    }
                    else if (component.Name == "CardButton1")
                    {
                        CardButton1 = tempImageButton;
                    }
                }
                else if (component is ImageComponent tempImageComponent)
                {
                    // store the ImageComponent component
                    if (component.Name == "Logo")
                    {
                        Logo = tempImageComponent;
                    }
                }
                else if (component is Label tempLabel)
                {
                    // If the InningLabel component
                    if (component.Name == "InningLabel")
                    {
                        // store the InningLabel
                        InningLabel = tempLabel;
                    }
                    // If the InningLabel component
                    else if (component.Name == "TricksLabel")
                    {
                        // store the TricksLabel
                        TricksLabel = tempLabel;
                    }
                }
            }
            #endregion

        #endregion
        
        #region Properties
            
            #region AudioPlayer
            /// <summary>
            /// This property gets or sets the value for 'AudioPlayer'.
            /// </summary>
            public BlazorAudioPlayer AudioPlayer
            {
                get { return audioPlayer; }
                set { audioPlayer = value; }
            }
            #endregion
            
            #region CardButton1
            /// <summary>
            /// This property gets or sets the value for 'Card1Button'.
            /// </summary>
            public ImageButton CardButton1
            {
                get { return cardButton1; }
                set { cardButton1 = value; }
            }
            #endregion
            
            #region CardButton10
            /// <summary>
            /// This property gets or sets the value for 'CardButton10'.
            /// </summary>
            public ImageButton CardButton10
            {
                get { return cardButton10; }
                set { cardButton10 = value; }
            }
            #endregion
            
            #region CardButton11
            /// <summary>
            /// This property gets or sets the value for 'CardButton11'.
            /// </summary>
            public ImageButton CardButton11
            {
                get { return cardButton11; }
                set { cardButton11 = value; }
            }
            #endregion
            
            #region CardButton12
            /// <summary>
            /// This property gets or sets the value for 'CardButton12'.
            /// </summary>
            public ImageButton CardButton12
            {
                get { return cardButton12; }
                set { cardButton12 = value; }
            }
            #endregion
            
            #region CardButton13
            /// <summary>
            /// This property gets or sets the value for 'CardButton13'.
            /// </summary>
            public ImageButton CardButton13
            {
                get { return cardButton13; }
                set { cardButton13 = value; }
            }
            #endregion
            
            #region CardButton14
            /// <summary>
            /// This property gets or sets the value for 'CardButton14'.
            /// </summary>
            public ImageButton CardButton14
            {
                get { return cardButton14; }
                set { cardButton14 = value; }
            }
            #endregion
            
            #region CardButton15
            /// <summary>
            /// This property gets or sets the value for 'CardButton15'.
            /// </summary>
            public ImageButton CardButton15
            {
                get { return cardButton15; }
                set { cardButton15 = value; }
            }
            #endregion
            
            #region CardButton16
            /// <summary>
            /// This property gets or sets the value for 'CardButton16'.
            /// </summary>
            public ImageButton CardButton16
            {
                get { return cardButton16; }
                set { cardButton16 = value; }
            }
            #endregion
            
            #region CardButton17
            /// <summary>
            /// This property gets or sets the value for 'CardButton17'.
            /// </summary>
            public ImageButton CardButton17
            {
                get { return cardButton17; }
                set { cardButton17 = value; }
            }
            #endregion
            
            #region CardButton18
            /// <summary>
            /// This property gets or sets the value for 'CardButton18'.
            /// </summary>
            public ImageButton CardButton18
            {
                get { return cardButton18; }
                set { cardButton18 = value; }
            }
            #endregion
            
            #region CardButton19
            /// <summary>
            /// This property gets or sets the value for 'CardButton19'.
            /// </summary>
            public ImageButton CardButton19
            {
                get { return cardButton19; }
                set { cardButton19 = value; }
            }
            #endregion
            
            #region CardButton2
            /// <summary>
            /// This property gets or sets the value for 'CardButton2'.
            /// </summary>
            public ImageButton CardButton2
            {
                get { return cardButton2; }
                set { cardButton2 = value; }
            }
            #endregion
            
            #region CardButton20
            /// <summary>
            /// This property gets or sets the value for 'CardButton20'.
            /// </summary>
            public ImageButton CardButton20
            {
                get { return cardButton20; }
                set { cardButton20 = value; }
            }
            #endregion
            
            #region CardButton21
            /// <summary>
            /// This property gets or sets the value for 'CardButton21'.
            /// </summary>
            public ImageButton CardButton21
            {
                get { return cardButton21; }
                set { cardButton21 = value; }
            }
            #endregion
            
            #region CardButton22
            /// <summary>
            /// This property gets or sets the value for 'CardButton22'.
            /// </summary>
            public ImageButton CardButton22
            {
                get { return cardButton22; }
                set { cardButton22 = value; }
            }
            #endregion
            
            #region CardButton23
            /// <summary>
            /// This property gets or sets the value for 'CardButton23'.
            /// </summary>
            public ImageButton CardButton23
            {
                get { return cardButton23; }
                set { cardButton23 = value; }
            }
            #endregion
            
            #region CardButton24
            /// <summary>
            /// This property gets or sets the value for 'CardButton24'.
            /// </summary>
            public ImageButton CardButton24
            {
                get { return cardButton24; }
                set { cardButton24 = value; }
            }
            #endregion
            
            #region CardButton25
            /// <summary>
            /// This property gets or sets the value for 'CardButton25'.
            /// </summary>
            public ImageButton CardButton25
            {
                get { return cardButton25; }
                set { cardButton25 = value; }
            }
            #endregion
            
            #region CardButton26
            /// <summary>
            /// This property gets or sets the value for 'CardButton26'.
            /// </summary>
            public ImageButton CardButton26
            {
                get { return cardButton26; }
                set { cardButton26 = value; }
            }
            #endregion
            
            #region CardButton27
            /// <summary>
            /// This property gets or sets the value for 'CardButton27'.
            /// </summary>
            public ImageButton CardButton27
            {
                get { return cardButton27; }
                set { cardButton27 = value; }
            }
            #endregion
            
            #region CardButton28
            /// <summary>
            /// This property gets or sets the value for 'CardButton28'.
            /// </summary>
            public ImageButton CardButton28
            {
                get { return cardButton28; }
                set { cardButton28 = value; }
            }
            #endregion
            
            #region CardButton29
            /// <summary>
            /// This property gets or sets the value for 'CardButton29'.
            /// </summary>
            public ImageButton CardButton29
            {
                get { return cardButton29; }
                set { cardButton29 = value; }
            }
            #endregion
            
            #region CardButton3
            /// <summary>
            /// This property gets or sets the value for 'CardButton3'.
            /// </summary>
            public ImageButton CardButton3
            {
                get { return cardButton3; }
                set { cardButton3 = value; }
            }
            #endregion
            
            #region CardButton30
            /// <summary>
            /// This property gets or sets the value for 'CardButton30'.
            /// </summary>
            public ImageButton CardButton30
            {
                get { return cardButton30; }
                set { cardButton30 = value; }
            }
            #endregion
            
            #region CardButton31
            /// <summary>
            /// This property gets or sets the value for 'CardButton31'.
            /// </summary>
            public ImageButton CardButton31
            {
                get { return cardButton31; }
                set { cardButton31 = value; }
            }
            #endregion
            
            #region CardButton32
            /// <summary>
            /// This property gets or sets the value for 'CardButton32'.
            /// </summary>
            public ImageButton CardButton32
            {
                get { return cardButton32; }
                set { cardButton32 = value; }
            }
            #endregion
            
            #region CardButton33
            /// <summary>
            /// This property gets or sets the value for 'CardButton33'.
            /// </summary>
            public ImageButton CardButton33
            {
                get { return cardButton33; }
                set { cardButton33 = value; }
            }
            #endregion
            
            #region CardButton34
            /// <summary>
            /// This property gets or sets the value for 'CardButton34'.
            /// </summary>
            public ImageButton CardButton34
            {
                get { return cardButton34; }
                set { cardButton34 = value; }
            }
            #endregion
            
            #region CardButton35
            /// <summary>
            /// This property gets or sets the value for 'CardButton35'.
            /// </summary>
            public ImageButton CardButton35
            {
                get { return cardButton35; }
                set { cardButton35 = value; }
            }
            #endregion
            
            #region CardButton36
            /// <summary>
            /// This property gets or sets the value for 'CardButton36'.
            /// </summary>
            public ImageButton CardButton36
            {
                get { return cardButton36; }
                set { cardButton36 = value; }
            }
            #endregion
            
            #region CardButton37
            /// <summary>
            /// This property gets or sets the value for 'CardButton37'.
            /// </summary>
            public ImageButton CardButton37
            {
                get { return cardButton37; }
                set { cardButton37 = value; }
            }
            #endregion
            
            #region CardButton38
            /// <summary>
            /// This property gets or sets the value for 'CardButton38'.
            /// </summary>
            public ImageButton CardButton38
            {
                get { return cardButton38; }
                set { cardButton38 = value; }
            }
            #endregion
            
            #region CardButton39
            /// <summary>
            /// This property gets or sets the value for 'CardButton39'.
            /// </summary>
            public ImageButton CardButton39
            {
                get { return cardButton39; }
                set { cardButton39 = value; }
            }
            #endregion
            
            #region CardButton4
            /// <summary>
            /// This property gets or sets the value for 'CardButton4'.
            /// </summary>
            public ImageButton CardButton4
            {
                get { return cardButton4; }
                set { cardButton4 = value; }
            }
            #endregion
            
            #region CardButton40
            /// <summary>
            /// This property gets or sets the value for 'CardButton40'.
            /// </summary>
            public ImageButton CardButton40
            {
                get { return cardButton40; }
                set { cardButton40 = value; }
            }
            #endregion
            
            #region CardButton41
            /// <summary>
            /// This property gets or sets the value for 'CardButton41'.
            /// </summary>
            public ImageButton CardButton41
            {
                get { return cardButton41; }
                set { cardButton41 = value; }
            }
            #endregion
            
            #region CardButton42
            /// <summary>
            /// This property gets or sets the value for 'CardButton42'.
            /// </summary>
            public ImageButton CardButton42
            {
                get { return cardButton42; }
                set { cardButton42 = value; }
            }
            #endregion
            
            #region CardButton43
            /// <summary>
            /// This property gets or sets the value for 'CardButton43'.
            /// </summary>
            public ImageButton CardButton43
            {
                get { return cardButton43; }
                set { cardButton43 = value; }
            }
            #endregion
            
            #region CardButton44
            /// <summary>
            /// This property gets or sets the value for 'CardButton44'.
            /// </summary>
            public ImageButton CardButton44
            {
                get { return cardButton44; }
                set { cardButton44 = value; }
            }
            #endregion
            
            #region CardButton45
            /// <summary>
            /// This property gets or sets the value for 'CardButton45'.
            /// </summary>
            public ImageButton CardButton45
            {
                get { return cardButton45; }
                set { cardButton45 = value; }
            }
            #endregion
            
            #region CardButton46
            /// <summary>
            /// This property gets or sets the value for 'CardButton46'.
            /// </summary>
            public ImageButton CardButton46
            {
                get { return cardButton46; }
                set { cardButton46 = value; }
            }
            #endregion
            
            #region CardButton47
            /// <summary>
            /// This property gets or sets the value for 'CardButton47'.
            /// </summary>
            public ImageButton CardButton47
            {
                get { return cardButton47; }
                set { cardButton47 = value; }
            }
            #endregion
            
            #region CardButton48
            /// <summary>
            /// This property gets or sets the value for 'CardButton48'.
            /// </summary>
            public ImageButton CardButton48
            {
                get { return cardButton48; }
                set { cardButton48 = value; }
            }
            #endregion
            
            #region CardButton49
            /// <summary>
            /// This property gets or sets the value for 'CardButton49'.
            /// </summary>
            public ImageButton CardButton49
            {
                get { return cardButton49; }
                set { cardButton49 = value; }
            }
            #endregion
            
            #region CardButton5
            /// <summary>
            /// This property gets or sets the value for 'CardButton5'.
            /// </summary>
            public ImageButton CardButton5
            {
                get { return cardButton5; }
                set { cardButton5 = value; }
            }
            #endregion
            
            #region CardButton50
            /// <summary>
            /// This property gets or sets the value for 'CardButton50'.
            /// </summary>
            public ImageButton CardButton50
            {
                get { return cardButton50; }
                set { cardButton50 = value; }
            }
            #endregion
            
            #region CardButton51
            /// <summary>
            /// This property gets or sets the value for 'CardButton51'.
            /// </summary>
            public ImageButton CardButton51
            {
                get { return cardButton51; }
                set { cardButton51 = value; }
            }
            #endregion
            
            #region CardButton52
            /// <summary>
            /// This property gets or sets the value for 'CardButton52'.
            /// </summary>
            public ImageButton CardButton52
            {
                get { return cardButton52; }
                set { cardButton52 = value; }
            }
            #endregion
            
            #region CardButton6
            /// <summary>
            /// This property gets or sets the value for 'CardButton6'.
            /// </summary>
            public ImageButton CardButton6
            {
                get { return cardButton6; }
                set { cardButton6 = value; }
            }
            #endregion
            
            #region CardButton7
            /// <summary>
            /// This property gets or sets the value for 'CardButton7'.
            /// </summary>
            public ImageButton CardButton7
            {
                get { return cardButton7; }
                set { cardButton7 = value; }
            }
            #endregion
            
            #region CardButton8
            /// <summary>
            /// This property gets or sets the value for 'CardButton8'.
            /// </summary>
            public ImageButton CardButton8
            {
                get { return cardButton8; }
                set { cardButton8 = value; }
            }
            #endregion
            
            #region CardButton9
            /// <summary>
            /// This property gets or sets the value for 'CardButton9'.
            /// </summary>
            public ImageButton CardButton9
            {
                get { return cardButton9; }
                set { cardButton9 = value; }
            }
            #endregion
            
            #region CardsTurnedOverThisInning
            /// <summary>
            /// This property gets or sets the value for 'CardsTurnedOverThisInning'.
            /// </summary>
            public List<CardInfo> CardsTurnedOverThisInning
            {
                get { return cardsTurnedOverThisInning; }
                set { cardsTurnedOverThisInning = value; }
            }
            #endregion
            
            #region ConsectutiveCards
            /// <summary>
            /// This property gets or sets the value for 'ConsectutiveCards'.
            /// </summary>
            public int ConsectutiveCards
            {
                get { return consectutiveCards; }
                set { consectutiveCards = value; }
            }
            #endregion
            
            #region Dealer
            /// <summary>
            /// This property gets or sets the value for 'Dealer'.
            /// </summary>
            public Dealer Dealer
            {
                get { return dealer; }
                set { dealer = value; }
            }
            #endregion
            
            #region FirstCardTurnedOver
            /// <summary>
            /// This property gets or sets the value for 'FirstCardTurnedOver'.
            /// </summary>
            public CardInfo FirstCardTurnedOver
            {
                get { return firstCardTurnedOver; }
                set { firstCardTurnedOver = value; }
            }
            #endregion
            
            #region HasAudioPlayer
            /// <summary>
            /// This property returns true if this object has an 'AudioPlayer'.
            /// </summary>
            public bool HasAudioPlayer
            {
                get
                {
                    // initial value
                    bool hasAudioPlayer = (AudioPlayer != null);

                    // return value
                    return hasAudioPlayer;
                }
            }
            #endregion
            
            #region HasCardButton1
            /// <summary>
            /// This property returns true if this object has a 'HasCardButton1'.
            /// </summary>
            public bool HasCardButton1
            {
                get
                {
                    // initial value
                    bool hasCard1Button = (CardButton1 != null);

                    // return value
                    return hasCard1Button;
                }
            }
            #endregion
            
            #region HasCardButton10
            /// <summary>
            /// This property returns true if this object has a 'CardButton10'.
            /// </summary>
            public bool HasCardButton10
            {
                get
                {
                    // initial value
                    bool hasCardButton10 = (CardButton10 != null);

                    // return value
                    return hasCardButton10;
                }
            }
            #endregion
            
            #region HasCardButton11
            /// <summary>
            /// This property returns true if this object has a 'CardButton11'.
            /// </summary>
            public bool HasCardButton11
            {
                get
                {
                    // initial value
                    bool hasCardButton11 = (CardButton11 != null);

                    // return value
                    return hasCardButton11;
                }
            }
            #endregion
            
            #region HasCardButton12
            /// <summary>
            /// This property returns true if this object has a 'CardButton12'.
            /// </summary>
            public bool HasCardButton12
            {
                get
                {
                    // initial value
                    bool hasCardButton12 = (CardButton12 != null);

                    // return value
                    return hasCardButton12;
                }
            }
            #endregion
            
            #region HasCardButton13
            /// <summary>
            /// This property returns true if this object has a 'CardButton13'.
            /// </summary>
            public bool HasCardButton13
            {
                get
                {
                    // initial value
                    bool hasCardButton13 = (CardButton13 != null);

                    // return value
                    return hasCardButton13;
                }
            }
            #endregion
            
            #region HasCardButton14
            /// <summary>
            /// This property returns true if this object has a 'CardButton14'.
            /// </summary>
            public bool HasCardButton14
            {
                get
                {
                    // initial value
                    bool hasCardButton14 = (CardButton14 != null);

                    // return value
                    return hasCardButton14;
                }
            }
            #endregion
            
            #region HasCardButton15
            /// <summary>
            /// This property returns true if this object has a 'CardButton15'.
            /// </summary>
            public bool HasCardButton15
            {
                get
                {
                    // initial value
                    bool hasCardButton15 = (CardButton15 != null);

                    // return value
                    return hasCardButton15;
                }
            }
            #endregion
            
            #region HasCardButton16
            /// <summary>
            /// This property returns true if this object has a 'CardButton16'.
            /// </summary>
            public bool HasCardButton16
            {
                get
                {
                    // initial value
                    bool hasCardButton16 = (CardButton16 != null);

                    // return value
                    return hasCardButton16;
                }
            }
            #endregion
            
            #region HasCardButton17
            /// <summary>
            /// This property returns true if this object has a 'CardButton17'.
            /// </summary>
            public bool HasCardButton17
            {
                get
                {
                    // initial value
                    bool hasCardButton17 = (CardButton17 != null);

                    // return value
                    return hasCardButton17;
                }
            }
            #endregion
            
            #region HasCardButton18
            /// <summary>
            /// This property returns true if this object has a 'CardButton18'.
            /// </summary>
            public bool HasCardButton18
            {
                get
                {
                    // initial value
                    bool hasCardButton18 = (CardButton18 != null);

                    // return value
                    return hasCardButton18;
                }
            }
            #endregion
            
            #region HasCardButton19
            /// <summary>
            /// This property returns true if this object has a 'CardButton19'.
            /// </summary>
            public bool HasCardButton19
            {
                get
                {
                    // initial value
                    bool hasCardButton19 = (CardButton19 != null);

                    // return value
                    return hasCardButton19;
                }
            }
            #endregion
            
            #region HasCardButton2
            /// <summary>
            /// This property returns true if this object has a 'CardButton2'.
            /// </summary>
            public bool HasCardButton2
            {
                get
                {
                    // initial value
                    bool hasCardButton2 = (CardButton2 != null);

                    // return value
                    return hasCardButton2;
                }
            }
            #endregion
            
            #region HasCardButton20
            /// <summary>
            /// This property returns true if this object has a 'CardButton20'.
            /// </summary>
            public bool HasCardButton20
            {
                get
                {
                    // initial value
                    bool hasCardButton20 = (CardButton20 != null);

                    // return value
                    return hasCardButton20;
                }
            }
            #endregion
            
            #region HasCardButton21
            /// <summary>
            /// This property returns true if this object has a 'CardButton21'.
            /// </summary>
            public bool HasCardButton21
            {
                get
                {
                    // initial value
                    bool hasCardButton21 = (CardButton21 != null);

                    // return value
                    return hasCardButton21;
                }
            }
            #endregion
            
            #region HasCardButton22
            /// <summary>
            /// This property returns true if this object has a 'CardButton22'.
            /// </summary>
            public bool HasCardButton22
            {
                get
                {
                    // initial value
                    bool hasCardButton22 = (CardButton22 != null);

                    // return value
                    return hasCardButton22;
                }
            }
            #endregion
            
            #region HasCardButton23
            /// <summary>
            /// This property returns true if this object has a 'CardButton23'.
            /// </summary>
            public bool HasCardButton23
            {
                get
                {
                    // initial value
                    bool hasCardButton23 = (CardButton23 != null);

                    // return value
                    return hasCardButton23;
                }
            }
            #endregion
            
            #region HasCardButton24
            /// <summary>
            /// This property returns true if this object has a 'CardButton24'.
            /// </summary>
            public bool HasCardButton24
            {
                get
                {
                    // initial value
                    bool hasCardButton24 = (CardButton24 != null);

                    // return value
                    return hasCardButton24;
                }
            }
            #endregion
            
            #region HasCardButton25
            /// <summary>
            /// This property returns true if this object has a 'CardButton25'.
            /// </summary>
            public bool HasCardButton25
            {
                get
                {
                    // initial value
                    bool hasCardButton25 = (CardButton25 != null);

                    // return value
                    return hasCardButton25;
                }
            }
            #endregion
            
            #region HasCardButton26
            /// <summary>
            /// This property returns true if this object has a 'CardButton26'.
            /// </summary>
            public bool HasCardButton26
            {
                get
                {
                    // initial value
                    bool hasCardButton26 = (CardButton26 != null);

                    // return value
                    return hasCardButton26;
                }
            }
            #endregion
            
            #region HasCardButton27
            /// <summary>
            /// This property returns true if this object has a 'CardButton27'.
            /// </summary>
            public bool HasCardButton27
            {
                get
                {
                    // initial value
                    bool hasCardButton27 = (CardButton27 != null);

                    // return value
                    return hasCardButton27;
                }
            }
            #endregion
            
            #region HasCardButton28
            /// <summary>
            /// This property returns true if this object has a 'CardButton28'.
            /// </summary>
            public bool HasCardButton28
            {
                get
                {
                    // initial value
                    bool hasCardButton28 = (CardButton28 != null);

                    // return value
                    return hasCardButton28;
                }
            }
            #endregion
            
            #region HasCardButton29
            /// <summary>
            /// This property returns true if this object has a 'CardButton29'.
            /// </summary>
            public bool HasCardButton29
            {
                get
                {
                    // initial value
                    bool hasCardButton29 = (CardButton29 != null);

                    // return value
                    return hasCardButton29;
                }
            }
            #endregion
            
            #region HasCardButton3
            /// <summary>
            /// This property returns true if this object has a 'CardButton3'.
            /// </summary>
            public bool HasCardButton3
            {
                get
                {
                    // initial value
                    bool hasCardButton3 = (CardButton3 != null);

                    // return value
                    return hasCardButton3;
                }
            }
            #endregion
            
            #region HasCardButton30
            /// <summary>
            /// This property returns true if this object has a 'CardButton30'.
            /// </summary>
            public bool HasCardButton30
            {
                get
                {
                    // initial value
                    bool hasCardButton30 = (CardButton30 != null);

                    // return value
                    return hasCardButton30;
                }
            }
            #endregion
            
            #region HasCardButton31
            /// <summary>
            /// This property returns true if this object has a 'CardButton31'.
            /// </summary>
            public bool HasCardButton31
            {
                get
                {
                    // initial value
                    bool hasCardButton31 = (CardButton31 != null);

                    // return value
                    return hasCardButton31;
                }
            }
            #endregion
            
            #region HasCardButton32
            /// <summary>
            /// This property returns true if this object has a 'CardButton32'.
            /// </summary>
            public bool HasCardButton32
            {
                get
                {
                    // initial value
                    bool hasCardButton32 = (CardButton32 != null);

                    // return value
                    return hasCardButton32;
                }
            }
            #endregion
            
            #region HasCardButton33
            /// <summary>
            /// This property returns true if this object has a 'CardButton33'.
            /// </summary>
            public bool HasCardButton33
            {
                get
                {
                    // initial value
                    bool hasCardButton33 = (CardButton33 != null);

                    // return value
                    return hasCardButton33;
                }
            }
            #endregion
            
            #region HasCardButton34
            /// <summary>
            /// This property returns true if this object has a 'CardButton34'.
            /// </summary>
            public bool HasCardButton34
            {
                get
                {
                    // initial value
                    bool hasCardButton34 = (CardButton34 != null);

                    // return value
                    return hasCardButton34;
                }
            }
            #endregion
            
            #region HasCardButton35
            /// <summary>
            /// This property returns true if this object has a 'CardButton35'.
            /// </summary>
            public bool HasCardButton35
            {
                get
                {
                    // initial value
                    bool hasCardButton35 = (CardButton35 != null);

                    // return value
                    return hasCardButton35;
                }
            }
            #endregion
            
            #region HasCardButton36
            /// <summary>
            /// This property returns true if this object has a 'CardButton36'.
            /// </summary>
            public bool HasCardButton36
            {
                get
                {
                    // initial value
                    bool hasCardButton36 = (CardButton36 != null);

                    // return value
                    return hasCardButton36;
                }
            }
            #endregion
            
            #region HasCardButton37
            /// <summary>
            /// This property returns true if this object has a 'CardButton37'.
            /// </summary>
            public bool HasCardButton37
            {
                get
                {
                    // initial value
                    bool hasCardButton37 = (CardButton37 != null);

                    // return value
                    return hasCardButton37;
                }
            }
            #endregion
            
            #region HasCardButton38
            /// <summary>
            /// This property returns true if this object has a 'CardButton38'.
            /// </summary>
            public bool HasCardButton38
            {
                get
                {
                    // initial value
                    bool hasCardButton38 = (CardButton38 != null);

                    // return value
                    return hasCardButton38;
                }
            }
            #endregion
            
            #region HasCardButton39
            /// <summary>
            /// This property returns true if this object has a 'CardButton39'.
            /// </summary>
            public bool HasCardButton39
            {
                get
                {
                    // initial value
                    bool hasCardButton39 = (CardButton39 != null);

                    // return value
                    return hasCardButton39;
                }
            }
            #endregion
            
            #region HasCardButton4
            /// <summary>
            /// This property returns true if this object has a 'CardButton4'.
            /// </summary>
            public bool HasCardButton4
            {
                get
                {
                    // initial value
                    bool hasCardButton4 = (CardButton4 != null);

                    // return value
                    return hasCardButton4;
                }
            }
            #endregion
            
            #region HasCardButton40
            /// <summary>
            /// This property returns true if this object has a 'CardButton40'.
            /// </summary>
            public bool HasCardButton40
            {
                get
                {
                    // initial value
                    bool hasCardButton40 = (CardButton40 != null);

                    // return value
                    return hasCardButton40;
                }
            }
            #endregion
            
            #region HasCardButton41
            /// <summary>
            /// This property returns true if this object has a 'CardButton41'.
            /// </summary>
            public bool HasCardButton41
            {
                get
                {
                    // initial value
                    bool hasCardButton41 = (CardButton41 != null);

                    // return value
                    return hasCardButton41;
                }
            }
            #endregion
            
            #region HasCardButton42
            /// <summary>
            /// This property returns true if this object has a 'CardButton42'.
            /// </summary>
            public bool HasCardButton42
            {
                get
                {
                    // initial value
                    bool hasCardButton42 = (CardButton42 != null);

                    // return value
                    return hasCardButton42;
                }
            }
            #endregion
            
            #region HasCardButton43
            /// <summary>
            /// This property returns true if this object has a 'CardButton43'.
            /// </summary>
            public bool HasCardButton43
            {
                get
                {
                    // initial value
                    bool hasCardButton43 = (CardButton43 != null);

                    // return value
                    return hasCardButton43;
                }
            }
            #endregion
            
            #region HasCardButton44
            /// <summary>
            /// This property returns true if this object has a 'CardButton44'.
            /// </summary>
            public bool HasCardButton44
            {
                get
                {
                    // initial value
                    bool hasCardButton44 = (CardButton44 != null);

                    // return value
                    return hasCardButton44;
                }
            }
            #endregion
            
            #region HasCardButton45
            /// <summary>
            /// This property returns true if this object has a 'CardButton45'.
            /// </summary>
            public bool HasCardButton45
            {
                get
                {
                    // initial value
                    bool hasCardButton45 = (CardButton45 != null);

                    // return value
                    return hasCardButton45;
                }
            }
            #endregion
            
            #region HasCardButton46
            /// <summary>
            /// This property returns true if this object has a 'CardButton46'.
            /// </summary>
            public bool HasCardButton46
            {
                get
                {
                    // initial value
                    bool hasCardButton46 = (CardButton46 != null);

                    // return value
                    return hasCardButton46;
                }
            }
            #endregion
            
            #region HasCardButton47
            /// <summary>
            /// This property returns true if this object has a 'CardButton47'.
            /// </summary>
            public bool HasCardButton47
            {
                get
                {
                    // initial value
                    bool hasCardButton47 = (CardButton47 != null);

                    // return value
                    return hasCardButton47;
                }
            }
            #endregion
            
            #region HasCardButton48
            /// <summary>
            /// This property returns true if this object has a 'CardButton48'.
            /// </summary>
            public bool HasCardButton48
            {
                get
                {
                    // initial value
                    bool hasCardButton48 = (CardButton48 != null);

                    // return value
                    return hasCardButton48;
                }
            }
            #endregion
            
            #region HasCardButton49
            /// <summary>
            /// This property returns true if this object has a 'CardButton49'.
            /// </summary>
            public bool HasCardButton49
            {
                get
                {
                    // initial value
                    bool hasCardButton49 = (CardButton49 != null);

                    // return value
                    return hasCardButton49;
                }
            }
            #endregion
            
            #region HasCardButton5
            /// <summary>
            /// This property returns true if this object has a 'CardButton5'.
            /// </summary>
            public bool HasCardButton5
            {
                get
                {
                    // initial value
                    bool hasCardButton5 = (CardButton5 != null);

                    // return value
                    return hasCardButton5;
                }
            }
            #endregion
            
            #region HasCardButton50
            /// <summary>
            /// This property returns true if this object has a 'CardButton50'.
            /// </summary>
            public bool HasCardButton50
            {
                get
                {
                    // initial value
                    bool hasCardButton50 = (CardButton50 != null);

                    // return value
                    return hasCardButton50;
                }
            }
            #endregion
            
            #region HasCardButton51
            /// <summary>
            /// This property returns true if this object has a 'CardButton51'.
            /// </summary>
            public bool HasCardButton51
            {
                get
                {
                    // initial value
                    bool hasCardButton51 = (CardButton51 != null);

                    // return value
                    return hasCardButton51;
                }
            }
            #endregion
            
            #region HasCardButton52
            /// <summary>
            /// This property returns true if this object has a 'CardButton52'.
            /// </summary>
            public bool HasCardButton52
            {
                get
                {
                    // initial value
                    bool hasCardButton52 = (CardButton52 != null);

                    // return value
                    return hasCardButton52;
                }
            }
            #endregion
            
            #region HasCardButton6
            /// <summary>
            /// This property returns true if this object has a 'CardButton6'.
            /// </summary>
            public bool HasCardButton6
            {
                get
                {
                    // initial value
                    bool hasCardButton6 = (CardButton6 != null);

                    // return value
                    return hasCardButton6;
                }
            }
            #endregion
            
            #region HasCardButton7
            /// <summary>
            /// This property returns true if this object has a 'CardButton7'.
            /// </summary>
            public bool HasCardButton7
            {
                get
                {
                    // initial value
                    bool hasCardButton7 = (CardButton7 != null);

                    // return value
                    return hasCardButton7;
                }
            }
            #endregion
            
            #region HasCardButton8
            /// <summary>
            /// This property returns true if this object has a 'CardButton8'.
            /// </summary>
            public bool HasCardButton8
            {
                get
                {
                    // initial value
                    bool hasCardButton8 = (CardButton8 != null);

                    // return value
                    return hasCardButton8;
                }
            }
            #endregion
            
            #region HasCardButton9
            /// <summary>
            /// This property returns true if this object has a 'CardButton9'.
            /// </summary>
            public bool HasCardButton9
            {
                get
                {
                    // initial value
                    bool hasCardButton9 = (CardButton9 != null);

                    // return value
                    return hasCardButton9;
                }
            }
            #endregion
            
            #region HasCardsTurnedOverThisInning
            /// <summary>
            /// This property returns true if this object has a 'CardsTurnedOverThisInning'.
            /// </summary>
            public bool HasCardsTurnedOverThisInning
            {
                get
                {
                    // initial value
                    bool hasCardsTurnedOverThisInning = (CardsTurnedOverThisInning != null);

                    // return value
                    return hasCardsTurnedOverThisInning;
                }
            }
            #endregion
            
            #region HasDealer
            /// <summary>
            /// This property returns true if this object has a 'Dealer'.
            /// </summary>
            public bool HasDealer
            {
                get
                {
                    // initial value
                    bool hasDealer = (Dealer != null);

                    // return value
                    return hasDealer;
                }
            }
            #endregion
            
            #region HasInningLabel
            /// <summary>
            /// This property returns true if this object has an 'InningLabel'.
            /// </summary>
            public bool HasInningLabel
            {
                get
                {
                    // initial value
                    bool hasInningLabel = (InningLabel != null);

                    // return value
                    return hasInningLabel;
                }
            }
            #endregion
            
            #region HasLogo
            /// <summary>
            /// This property returns true if this object has a 'Logo'.
            /// </summary>
            public bool HasLogo
            {
                get
                {
                    // initial value
                    bool hasLogo = (Logo != null);

                    // return value
                    return hasLogo;
                }
            }
            #endregion
            
            #region HasSoundOption
            /// <summary>
            /// This property returns true if this object has a 'SoundOption'.
            /// </summary>
            public bool HasSoundOption
            {
                get
                {
                    // initial value
                    bool hasSoundOption = (SoundOption != null);

                    // return value
                    return hasSoundOption;
                }
            }
            #endregion
            
            #region HasTricksLabel
            /// <summary>
            /// This property returns true if this object has a 'TricksLabel'.
            /// </summary>
            public bool HasTricksLabel
            {
                get
                {
                    // initial value
                    bool hasTricksLabel = (TricksLabel != null);

                    // return value
                    return hasTricksLabel;
                }
            }
            #endregion
            
            #region Inning
            /// <summary>
            /// This property gets or sets the value for 'Inning'.
            /// </summary>
            public int Inning
            {
                get { return inning; }
                set { inning = value; }
            }
            #endregion
            
            #region InningLabel
            /// <summary>
            /// This property gets or sets the value for 'InningLabel'.
            /// </summary>
            public Label InningLabel
            {
                get { return inningLabel; }
                set { inningLabel = value; }
            }
            #endregion
            
            #region Logo
            /// <summary>
            /// This property gets or sets the value for 'Logo'.
            /// </summary>
            public ImageComponent Logo
            {
                get { return logo; }
                set { logo = value; }
            }
            #endregion
            
            #region NewGameButton
            /// <summary>
            /// This property gets or sets the value for 'NewGameButton'.
            /// </summary>
            public ImageButton NewGameButton
            {
                get { return newGameButton; }
                set { newGameButton = value; }
            }
            #endregion
            
            #region NoBorderStyle
            /// <summary>
            /// This property gets or sets the value for 'NoBorderStyle'.
            /// </summary>
            public string NoBorderStyle
            {
                get { return noBorderStyle; }
                set { noBorderStyle = value; }
            }
            #endregion
            
            #region PlaySound
            /// <summary>
            /// This read only property returns the value of PlaySound from the object SoundOption.
            /// </summary>
            public bool PlaySound
            {

                get
                {
                    // initial value
                    bool playSound = false;

                    // if SoundOption exists
                    if (SoundOption != null)
                    {
                        // set the return value
                        playSound = SoundOption.On;
                    }

                    // return value
                    return playSound;
                }
            }
            #endregion

            #region SoundOption
            /// <summary>
            /// This property gets or sets the value for 'SoundOption'.
            /// </summary>
            public ToggleComponent SoundOption
            {
                get { return soundOption; }
                set { soundOption = value; }
            }
            #endregion
            
            #region Tricks
            /// <summary>
            /// This property gets or sets the value for 'Tricks'.
            /// </summary>
            public int Tricks
            {
                get { return tricks; }
                set { tricks = value; }
            }
            #endregion
            
            #region TricksLabel
            /// <summary>
            /// This property gets or sets the value for 'TricksLabel'.
            /// </summary>
            public Label TricksLabel
            {
                get { return tricksLabel; }
                set { tricksLabel = value; }
            }
            #endregion
            
        #endregion
        
    }
    #endregion

}

