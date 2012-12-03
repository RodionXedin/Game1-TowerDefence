using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

using Windows.UI.Xaml.Media.Imaging;

using Windows.UI.Xaml.Controls;

namespace TowerDefense.TowerDefense
{
    class Bullet
    {
        public double x;
        public double y;
        double vx;
        double vy;
        public Rectangle rect;


        public Bullet(double x, double y, double vx, double vy)
        {
	        this.x = x;
	        this.y = y;
	        this.vx = vx;
	        this.vy = vy;
        }

        public void setImage(Canvas canvas, BitmapImage img)
        {
            /*
            Random r = new Random();
            byte red = (byte)r.Next(0, byte.MaxValue + 1);
            byte green = (byte)r.Next(0, byte.MaxValue + 1);
            byte blue = (byte)r.Next(0, byte.MaxValue + 1);
            Brush brush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, red, green, blue));
            */

            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = img;
            
            this.rect = new Rectangle();
            rect.Fill = imgBrush;
            //rect.Fill = brush;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
            rect.Width = 20;
            rect.Height = 20;
            canvas.Children.Add(rect);
        }

	    public void update() {
            this.x += this.vx;
            this.y += this.vy;
            rect.SetValue(Canvas.LeftProperty, this.x);
            rect.SetValue(Canvas.TopProperty, this.y);
		    // draw
	    } 
    }
}
