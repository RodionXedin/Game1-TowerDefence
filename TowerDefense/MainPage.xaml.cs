using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowerDefense.TowerDefense;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

using Windows.UI.Xaml.Shapes;

using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TowerDefense
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Storyboard _gameLoop = new Storyboard(); 

        private TowerDefense.Game game;
        Rectangle rect = new Rectangle();
        public MainPage()
        {
            this.InitializeComponent();
            Initialise();

            BitmapImage unitImg = ImageFromRelativePath(this, "Assets/unit.png");
            BitmapImage towerImg = ImageFromRelativePath(this, "Assets/tower2.png");
            BitmapImage bulletImg = ImageFromRelativePath(this, "Assets/mario_fireball.png");

            BitmapImage mapImg = ImageFromRelativePath(this, "Assets/map.png");

            

            ImageBrush imBrush = new ImageBrush();
            //ImageBrush imBrush = SolidColorBrush;
            //SolidColorBrush imBrush = new SolidColorBrush();
            imBrush.ImageSource = mapImg;
            //imBrush.Color = Windows.UI.Colors.Black;
            rect = new Rectangle();
            rect.Fill = imBrush;
            rect.SetValue(Canvas.LeftProperty, 0);
            rect.SetValue(Canvas.TopProperty, 0);
            rect.Width = 500;
            rect.Height = 500;
            gameCanvas.Children.Add(rect);

            game = new Game(gameCanvas, unitImg, towerImg, bulletImg);
            
            /*
            //BitmapImage bi = new BitmapImage(new Uri("ms-appdata:///unit.png"));
            BitmapImage bi = ImageFromRelativePath(this, "Assets/unit.png");
            ImageBrush imBrush = new ImageBrush();
            //ImageBrush imBrush = SolidColorBrush;
            //SolidColorBrush imBrush = new SolidColorBrush();
            imBrush.ImageSource = bi;
            //imBrush.Color = Windows.UI.Colors.Black;
            rect = new Rectangle();
            rect.Fill = imBrush;
            rect.SetValue(Canvas.LeftProperty, 10);
            rect.SetValue(Canvas.TopProperty, 10);
            rect.Width = 72;
            rect.Height = 72;
            gameCanvas.Children.Add(rect);
             * */
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Initialise()
        {
            _gameLoop.Duration = TimeSpan.FromMilliseconds(0);

            //Once the duration has completed run the following method
            _gameLoop.Completed += GameLoop;

            //Begin executing the storyboard
            _gameLoop.Begin();
        }

        private void GameLoop(object sender, object e)
        {
            
            game.update();
            if (game.gameFinished == true)
                gameGoing.Text = "Game Over";
            else
            {
            surviversText.Text = "Survivers: " + game.survivers;
            killsText.Text = "Kills: " + game.kills;
            pointsText.Text = "Points: " + game.points;
            healthText.Text = "Monster Health: " + game.health;
            towercostText.Text = "Tower cost: " + game.towerCost;
            towerdamageText.Text = "Tower damage: " + game.damage;
            _gameLoop.Begin();
            }
        }

        public static BitmapImage ImageFromRelativePath(FrameworkElement parent, string path)
        {
            var uri = new Uri(parent.BaseUri, path);
            BitmapImage result = new BitmapImage();
            result.UriSource = uri;
            return result;
        }

        private void gameCanvas_LayoutUpdated(object sender, object e)
        {

            //rect.SetValue(Canvas.LeftProperty, 200);
            //game.update();
        }

        private void gameCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            game.addTower(e.GetPosition(this).X, e.GetPosition(this).Y);
        }
    }
}
