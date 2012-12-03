using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TowerDefense.TowerDefense
{
    class Game
    {
        private List<Unit> units = new List<Unit>();
        private List<Tower> towers = new List<Tower>();
        private List<Point> path = new List<Point>();
        private BitmapImage bi = new BitmapImage(new Uri(@"C:\Users\rodio_000\Documents\Visual Studio 2012\Projects\TowerDefense\TowerDefense\Assets\img.png"));
        Canvas canvas;
        ImageBrush imBrush = new ImageBrush();
        public Game(Canvas canvas, List<Point> path = null)
        {
            this.canvas = canvas;
            imBrush.ImageSource = bi;
            
            if (path == null)
            {
                for (int i = 0; i < 100; ++i)
                {
                    path.Add(new Point(10, i + 5));
                }
            }
        }

        public void addUnit() 
        {
            units.Add(new Unit(10, 0, path, imBrush));
        }

        public void update()
        {
            foreach (Unit unit in units)
                if (!unit.isFinish())
                    unit.draw(canvas);
                else
                    units.Remove(unit);
            //foreach (Tower tower in towers)
            //    tower.draw(canvas);

        }
    }
}
