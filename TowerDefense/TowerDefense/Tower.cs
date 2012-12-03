using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml.Media;

using Windows.UI.Xaml.Media.Imaging;

using Windows.UI.Xaml.Shapes;

namespace TowerDefense.TowerDefense
{
    class Tower
    {
        public int x;
        public int y;
        public int radius;
        private int bulletVelocity;
        private int delay;
        private int shotDelayCount;
        public Rectangle rect;

        public Tower(Canvas canvas, int x, int y, int v, int delay, int radius, BitmapImage img)
        {

            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = img;
            this.rect = new Rectangle();
            rect.Fill = imgBrush;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
            rect.Width = 27;
            rect.Height = 47;
            canvas.Children.Add(rect);

	        this.x = x;
	        this.y = y;
	        //this.img = img;
	        this.radius = radius;
	        this.bulletVelocity = v;
	        this.delay = delay;
	        this.shotDelayCount = delay;
        }

	    public Bullet shot(int unitX, int unitY) {
		    if(this.shotDelayCount == this.delay) {
			    double px = unitX - this.x;
			    double py = unitY - this.y;
			    double g = (double) Math.Sqrt(px * px + py * py);
			    double dx = (double)px / (double)g;
			    double dy = (double)py / (double)g;
			    this.shotDelayCount = 0;

			    return new Bullet(this.x, this.y, dx * this.bulletVelocity, dy * this.bulletVelocity);
		    }
		    this.shotDelayCount++;
		    return null;
	    }

	    public void update() {
		    // draw
            rect.SetValue(Canvas.LeftProperty, this.x);
            rect.SetValue(Canvas.TopProperty, this.y);
	    }
    }
}
