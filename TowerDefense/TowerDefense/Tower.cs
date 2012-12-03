using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace TowerDefense.TowerDefense
{
    class Tower : GameObject
    {
        private int x;
        private int y;

        public Tower(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void draw(Canvas canvas)
        {
        }
    }
}
