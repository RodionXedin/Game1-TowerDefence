using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace TowerDefense.TowerDefense
{
    interface GameObject
    {
        void draw(Canvas canvas);
    }

    class Unit : GameObject
    {
        private Point position;
        private List<Point> path;
        private int currStep = 0;
        private bool finish = false;
        private double velocity = 0.2;
        private double moveCount = 0;
        private double moveLength = 0;
        private int directionX = 1;
        private int directionY = 0;
        private Rectangle rect = new Rectangle();

        public Unit(int x, int y, List<Point> path, ImageBrush imBrush)
        {
            this.position = new Point(x, y);
            this.path = path;
            rect.Fill = imBrush;
           
        }

        public void draw(Canvas canvas)
        {
            update();
            canvas.Children.Add(rect);
        }

        public bool isFinish()
        {
            return finish;
        }

        private void update()
        {

            if (currStep < path.Count - 1)
            {
                if (moveCount < moveLength)
                {
                    position.X += directionX * velocity;
                    position.Y += directionY * velocity;
                    moveCount++;
                }
                else
                {
                    currStep++;
                    double dx = path[currStep].X - position.X;
                    double dy = path[currStep].Y - position.Y;
                    if(dx == 0)
                        directionX = 0;
                    else if(dx > 0)
                        directionX = 1;
                    else if(dx < 0)
                        directionX = -1;

                    if(dy == 0)
                        directionY = 0;
                    else if(dx > 0)
                        directionY = 1;
                    else if(dx < 0)
                        directionY = -1;
                    
                    moveCount = 0;
                    position = path[currStep - 1];
                    moveLength = dx != 0 ? dx : dy;
                }
            }
            else
                finish = true;
        }

    }
}
