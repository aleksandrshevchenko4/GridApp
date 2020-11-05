using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripsTrapsTrull : ContentPage
    {
        public bool First;
        int[,] map = new int[3, 3];
        Label WhoFirst;
        BoxView box;
        public TripsTrapsTrull()
        {
            FirstPlayer();
            NewGame();
            WHO();
        }

        void NewGame()
        {
            Grid grid = new Grid();

            for (int i = 0; i < 3; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Star) });
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    box = new BoxView { Color = Color.FromRgb(150, 150, 150) };
                    map[i, j] = 170173;
                    grid.Children.Add(box, i, j);
                    var tap = new TapGestureRecognizer();
                    tap.Tapped += Tap_Tapped;
                    box.GestureRecognizers.Add(tap);
                }
            }

            AbsoluteLayout absolute = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            Button newGame = new Button
            {
                BackgroundColor = Color.Black,
                TextColor = Color.White,
                Text = "Uus mäng"
            };
            AbsoluteLayout.SetLayoutBounds(newGame, new Rectangle(0.9, 0.02, 150, 50));
            AbsoluteLayout.SetLayoutFlags(newGame, AbsoluteLayoutFlags.PositionProportional);
            newGame.Clicked += new EventHandler(NewGame_Clicked);
            absolute.Children.Add(newGame);

            Button XO = new Button
            {
                BackgroundColor = Color.Black,
                TextColor = Color.White,
                Text = "Pun või sin"
            };
            AbsoluteLayout.SetLayoutBounds(XO, new Rectangle(0.1, 0.02, 150, 50));
            AbsoluteLayout.SetLayoutFlags(XO, AbsoluteLayoutFlags.PositionProportional);
            XO.Clicked += XO_Clicked;
            absolute.Children.Add(XO);

            WhoFirst = new Label
            {
                TextColor = Color.Black,
                Text = "..."
            };
            AbsoluteLayout.SetLayoutBounds(WhoFirst, new Rectangle(0.7, 1.3, 150, 50));
            AbsoluteLayout.SetLayoutFlags(WhoFirst, AbsoluteLayoutFlags.PositionProportional);
            absolute.Children.Add(WhoFirst);

            StackLayout stack = new StackLayout()
            {
                Children = { grid, absolute }
            };
            Content = stack;
        }
        public async void FirstPlayer()
        {
            string FirstPlayer = await DisplayPromptAsync("Kes on esimene?", "Punane - 1, Sinine - 2",
                initialValue: "1",
                maxLength: 1,
                keyboard: Keyboard.Numeric);
            if (FirstPlayer == "1")
            {
                First = true;
            }
            else if (FirstPlayer == "2")
            {
                First = false;
            }
        }
        Random rnd = new Random();
        private bool First_playerRND()
        {
            int player = rnd.Next(0, 2);
            if (player == 1)
            {
                First = true;
            }
            else if (player == 2)
            {
                First = false;
            }
            return First;
        }
        private void XO_Clicked(object sender, EventArgs e)
        {
            First_playerRND();
            NewGame();
            WHO();
        }
        private void NewGame_Clicked(object sender, EventArgs e)
        {
            NewGame();
            WHO();
        }
        private void Tap_Tapped(object sender, EventArgs e)
        {
            BoxView box = sender as BoxView;

            if (box.Color == Color.FromRgb(150, 150, 150) && First)
            {
                box.Color = Color.FromRgb(150, 0, 0);
                First = false;
                map[Grid.GetRow(box), Grid.GetColumn(box)] = 0;
            }
            else if (box.Color == Color.FromRgb(150, 150, 150) && !First)
            {
                box.Color = Color.FromRgb(0, 0, 125);
                First = true;
                map[Grid.GetRow(box), Grid.GetColumn(box)] = 1;
            }
            else
            {
                DisplayAlert("Hoiatus!", "Vaenlane on selle põllu juba ära lõiganud.", "OK");
            }
            WHO();
            CheckWinner();
        }
        private void WHO()
        {
            if (First == true)
            {
                WhoFirst.Text = "Punane liikub";
            }
            else if (First == false)
            {
                WhoFirst.Text = "Sinine liikub";
            }
        }

        public string Winner()
        {
            string winner = "";

            // Вертикально слево
            if (map[0, 0] == 1 && map[1, 0] == 1 && map[2, 0] == 1)
            {
                winner = "pun";
            }
            else if (map[0, 0] == 0 && map[1, 0] == 0 && map[2, 0] == 0)
            {
                winner = "sin";
            }

            // Вертикально центер
            if (map[0, 1] == 1 && map[1, 1] == 1 && map[2, 1] == 1)
            {
                winner = "pun";
            }
            else if (map[0, 1] == 0 && map[1, 1] == 0 && map[2, 1] == 0)
            {
                winner = "sin";
            }

            // Вертикально справа
            if (map[0, 2] == 1 && map[1, 2] == 1 && map[2, 2] == 1)
            {
                winner = "pun";
            }
            else if (map[0, 2] == 0 && map[1, 2] == 0 && map[2, 2] == 0)
            {
                winner = "sin";
            }

            // Горизонтально верх
            if (map[0, 0] == 1 && map[0, 1] == 1 && map[0, 2] == 1)
            {
                winner = "pun";
            }
            else if (map[0, 0] == 0 && map[0, 1] == 0 && map[0, 2] == 0)
            {
                winner = "sin";
            }

            // Горизонтально центр
            if (map[1, 0] == 1 && map[1, 1] == 1 && map[1, 2] == 1)
            {
                winner = "pun";
            }
            else if (map[1, 0] == 0 && map[1, 1] == 0 && map[1, 2] == 0)
            {
                winner = "sin";
            }

            // Горизонтально низ
            if (map[2, 0] == 1 && map[2, 1] == 1 && map[2, 2] == 1)
            {
                winner = "pun";
            }
            else if (map[2, 0] == 0 && map[2, 1] == 0 && map[2, 2] == 0)
            {
                winner = "sin";
            }

            // Диагонально с верхнего левого края до нижнего правого края
            if (map[0, 0] == 1 && map[1, 1] == 1 && map[2, 2] == 1)
            {
                winner = "pun";
            }
            else if (map[0, 0] == 0 && map[1, 1] == 0 && map[2, 2] == 0)
            {
                winner = "sin";
            }

            // Диагонально с верхнего правого края до нижнего левого
            if (map[2, 0] == 1 && map[1, 1] == 1 && map[0, 2] == 1)
            {
                winner = "pun";
            }
            else if (map[2, 0] == 0 && map[1, 1] == 0 && map[0, 2] == 0)
            {
                winner = "sin";
            }

            return winner;
        }
        public void CheckWinner()
        {
            if (Winner() == "pun")
            {
                WhoFirst.Text = "Punane võitis !";
                restartPun();
            }
            else if (Winner() == "sin")
            {
                WhoFirst.Text = "Sinine võitis !";
                restartSin();
            }
        }
        private async void restartPun()
        {
            string Restart = await DisplayPromptAsync("Restart", "Sinine võitis! Kas te tahate mängida veel kord? Yah - 1, Ei - 2",
                initialValue: "1",
                maxLength: 1,
                keyboard: Keyboard.Numeric);
            if (Restart == "1")
            {
                First_playerRND();
                NewGame();
                WHO();
            }

        }
        private async void restartSin()
        {
            string Restart = await DisplayPromptAsync("Restart", "Punane võitis! Kas te tahate mängida veel kord? Yah - 1, Ei - 2",
                initialValue: "1",
                maxLength: 1,
                keyboard: Keyboard.Numeric);
            if (Restart == "1")
            {
                First_playerRND();
                NewGame();
                WHO();
            }

        }
    }
}