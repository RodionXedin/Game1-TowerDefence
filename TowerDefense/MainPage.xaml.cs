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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TowerDefense
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TowerDefense.Game game;
        public MainPage()
        {
            this.InitializeComponent();
            List<Point> points = new List<Point>();
            points.Add(new Point(0,0));
            points.Add(new Point(30,30));
            points.Add(new Point(50,50));
            points.Add(new Point(500,500));

            game = new Game(gameCanvas, points);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void gameCanvas_LayoutUpdated(object sender, object e)
        {
            game.update();
        }
    }
}
