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

using Windows.UI.Xaml.Media.Imaging;

namespace TowerDefense.TowerDefense
{
    interface GameObject
    {
        void draw(Canvas canvas);
    }

    class Unit
    {
        public int x, y, vx, vy, velocity, stepIndex;
        public bool isFinish;
        public bool isDead = false;
        public Rectangle rect;
        List<Point> path;
        public double health;

        public Unit(Canvas canvas, int x, int y, BitmapImage img, List<Point> path, double health)
        {
            this.health = health;

            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = img;
            this.rect = new Rectangle();
            rect.Fill = imgBrush;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
            rect.Width = 72;
            rect.Height = 72;
            canvas.Children.Add(rect);

	        this.x = x;
	        this.y = y;
	        this.vx = 0;
	        this.vy = 0;
	        this.velocity = 2;
	        this.path = path;
	        this.isFinish = false;
	        this.stepIndex = 0;
        }

	    public int size() {
		    //return this.img.width;
            return 72;
	    }

	    private void _nextStep() {
		    if(this.isFinish)
			    return;
            
		    Point currStep = path[this.stepIndex];
		    if(this.x == currStep.X && this.y == currStep.Y) {
			    if(this.stepIndex == path.Count - 1) {
				    this.isFinish = true;
				    //console.log('finish');
				    return;
			    }
			    currStep = path[++this.stepIndex];
		    }

		    if(this.x > currStep.X)
				    this.vx = -1;
		    else if(this.x < currStep.X)
				    this.vx = 1;
		    else
				    this.vx = 0;

		    if(this.y > currStep.Y)
				    this.vy = -1;
		    else if(this.y < currStep.Y)
				    this.vy = 1;
		    else
				    this.vy = 0;
            

		    this.x += this.vx * this.velocity;
		    this.y += this.vy * this.velocity;
	    }

	    public void update() {
            if (health <= 0)
            {
                this.isDead = true;
            }
		    if(!this.isFinish && !this.isDead) {
			    this._nextStep();
                rect.SetValue(Canvas.LeftProperty, this.x - this.size() / 2);
                rect.SetValue(Canvas.TopProperty, this.y - this.size() / 2);
			    //ctx.drawImage(this.img, this.x - this.img.width / 2, this.y -  this.img.width / 2);
		    }
	    }

    }
}
