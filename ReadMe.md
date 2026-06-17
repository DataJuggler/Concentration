Concentration is a Demo of several DataJuggler opensource projects.

1. DataJuggler.Blazor.Components - Contains the ImageButtons and Labels used in this project
2. DataJuggler.PlayingCards - Contains a deck of 52 playing cards, 4 card backs and a joker. 
The Dealer class is used to deal the cards and handle the path to the card images *
3. DataJuggler.RandomShuffler - performs the shuffling
4. DataJuggler.BlazorAudio - plays the sound
5. The Gilded Deck - A deck of 52 playing 

* When you create an instance of the dealer, you set the Platform, either Blazor (web) or WinForms.
WinForms handles the getting the card out of the NuGet package differently than Blazor. For WinForms,
a stream is read using DataJuggler.PixelDatabase (one of my favorite opensource projects) and the 
PictureBox.Image is set to PixelDatabase.DirectBitmap.Bitmap. 

Not included in this project, I also built a new Layout Manager project which handles duplicating the
ImageButtons to set the Top & Left to each component and name the components CardButton1, CardButton2, etc.
Also ButtonNumber is is set for each component. I will be releasing Layout Manager soon after one bug is fixed.

Regionizer2022 or Regionizer 2026 is a Visual Studio Package that wires up the components. If you review the code
on Home.razor.cs, all the private variables and properties and has properties are created using Regionizer's Wire Up
Components feature. It's an interesting workflow code genrating C# code for Blazor in .NET 10 using .NET Framework
code that has to be used for a Visual Studio Package.

# Rules
At the start 52 cards are dealt out face down. Each turn a user clicks at least 2 cards. If the 2nd card matches
the first the user continues. If four cards of the same value (Four Aces or Four Kings, for example) that is considered
a trick. When a trick is captured those four ImageButtons are set to invisible (removed from the board).

This was a fun sample project, and didn't take very long considering all the new code I had to write.
The ImageButton had a new method added SetImageUrl with delay. This is used to turn cards back over if
you do not capture a trick on that inning. The following code example iterates the CardsTurnedOverThisInning
list, which is a collectoin of CardInfo objects. A CardInfo is a simple class that contains the CardName, Suit &
the ImageButton, called a CardButton that was clicked on. This is used so the cards can get turned back over
after a trick wasn't captured. 

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

# Known Issues
If you click the same card twice it will cost you an inning. Nothing will match and you might see a card
get clicked on and nothing happens after. One inning later everything should resume back to normal.
I didn't spend a lot of time user proofing the app, it's just a simple demo using a deck of cards I created.

# Options
On my to do list is a adding a toggle component to enable Playing Sound or not. I will add this feature soon.
I just wanted to release this to see if anyone else finds this game "fun". I find it kind of fun. At the time of this
writing my best score is 161 innings. If I am tired, it can take me over 300 innings. There's probably a strategy
to train your brain. I just like to test how my noggin is working.

Let me know if you find this game fun or useful by leaving a star please.

