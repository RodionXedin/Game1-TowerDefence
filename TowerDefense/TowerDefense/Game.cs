using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;

using Windows.UI.Xaml.Shapes;

using Windows.UI.Popups;

namespace TowerDefense.TowerDefense
{
    class Game
    {
        private List<Unit> units = new List<Unit>();
        private List<Tower> towers = new List<Tower>();
        private List<Bullet> bullets = new List<Bullet>();
        private List<Point> path = new List<Point>();
        Canvas gameCanvas;
        BitmapImage bulletImg;
        public int survivers = 0;
        public int kills = 0;
        int delay = 100;
        int delayCount = 0;
        BitmapImage unitImg;
        BitmapImage towerImg;
        public double health = 100;
        public double damage = 15;
        public double handicap = 0.4;
        private int shotDelay = 80;
        public double points = 10;
        public double towerCost = 10;
        private int lastupgrade = 0;
        private int unitCounter = 0;
        public bool gameFinished = false;
        public static bool Susp = false;

        public Game(Canvas canvas, BitmapImage unitImg, BitmapImage towerImg, BitmapImage bulletImg)
        {
            this.gameCanvas = canvas;
            this.unitImg = unitImg;
            this.towerImg = towerImg;
            path.Add(new Point(158, 0));
			path.Add(new Point(158, 32));
			path.Add(new Point(50, 40));
			path.Add(new Point(50, 234));
			path.Add(new Point(154, 234));
			path.Add(new Point(160, 160));
			path.Add(new Point(330, 154));
			path.Add(new Point(334, 334));
			path.Add(new Point(58, 348));
			path.Add(new Point(50, 444));
			path.Add(new Point(440, 460));
			path.Add(new Point(450, 46));
			path.Add(new Point(286, 46));
			path.Add(new Point(286, -50));
            this.bulletImg = bulletImg;

            

            units.Add(new Unit(gameCanvas, 158, 0, unitImg, path, health));
            units.Add(new Unit(gameCanvas, 158, -60, unitImg, path, health));
            units.Add(new Unit(gameCanvas, 158, -120, unitImg, path, health));
            units.Add(new Unit(gameCanvas, 158, -180, unitImg, path, health));

            towers.Add(new Tower(gameCanvas, 158, 60, 5, shotDelay, 200, towerImg));
        }

        public static BitmapImage ImageFromRelativePath(FrameworkElement parent, string path)
        {
            var uri = new Uri(parent.BaseUri, path);
            BitmapImage result = new BitmapImage();
            result.UriSource = uri;
            return result;
        }

        public void addTower(double x, double y)
        {
            if (points >= towerCost)
            {
                points -= towerCost;
                towers.Add(new Tower(gameCanvas, (int)x, (int)y, 5, shotDelay, 200, towerImg));
            }
        }

        private void upgradeTowers()
        {
            damage *= 1.01;
            if (shotDelay > 15)
            {
                shotDelay--;
            }
            foreach (Tower tower in towers)
            {
               tower.upgradeShotDelay(shotDelay);
            }
        }
        public static void ContinueGame()
        {
            Susp = false;
        }
        public static void SuspendGame()
        {
            Susp = true;
        }

        public void update()
        {
            if (Susp == false)
            {
                delayCount++;
                if (survivers >= 20)
                    gameFinished = true;
                if (unitCounter >= 10)
                {
                    health *= 1.1;
                    unitCounter *= 0;
                }
                if (kills - lastupgrade >= 30)
                {
                    lastupgrade = kills;
                    upgradeTowers();
                    towerCost *= 1.1;
                }
                // In the moment when we cannot build more towers , we will upgrade what we have for 100 points 
                if (points > 100)
                {
                    upgradeTowers();
                }
                if (delayCount == delay)
                {
                    units.Add(new Unit(gameCanvas, 158, 0, unitImg, path, health));
                    units.Add(new Unit(gameCanvas, 158, -60, unitImg, path, health));
                    units.Add(new Unit(gameCanvas, 158, -120, unitImg, path, health));
                    units.Add(new Unit(gameCanvas, 158, -180, unitImg, path, health));
                    delayCount = 0;
                }
                //ctx.clearRect(0, 0, canvas.width, canvas.height);

                //new MessageDialog("fff").ShowAsync();


                for (var i = 0; i < this.towers.Count; ++i)
                {
                    this.towers[i].update();

                    bool isBreak = false;
                    for (var j = 0; j < this.units.Count; ++j)
                    {
                        if (this.units[j].x > (this.towers[i].x)
                            && this.units[j].x < (this.towers[i].x + 30)
                            && this.units[j].y > (this.towers[i].y)
                            && this.units[j].y < (this.towers[i].y + 30)
                            )
                        {
                            gameCanvas.Children.Remove(this.towers[i].rect);
                            towers.Remove(this.towers[i]);
                            i--;
                            isBreak = true;
                        }
                    }

                    if (isBreak)
                        continue;

                    for (var j = 0; j < this.units.Count; ++j)
                    {
                        if (this.units[j].x > (this.towers[i].x - this.towers[i].radius)
                            && this.units[j].x < (this.towers[i].x + this.towers[i].radius)
                            && this.units[j].y > (this.towers[i].y - this.towers[i].radius)
                            && this.units[j].y < (this.towers[i].y + this.towers[i].radius)
                            )
                        {
                            // TODO: calculate bullet path
                            // ...
                            // this.towers.shot(vx, vy);
                            Bullet bullet = towers[i].shot(this.units[j].x, this.units[j].y);
                            if (bullet != null)
                            {
                                bullet.setImage(gameCanvas, bulletImg);
                                bullets.Add(bullet);
                            }
                        }
                    }
                }

                for (var i = 0; i < this.units.Count; ++i)
                {
                    this.units[i].update();
                    if (this.units[i].isFinish)
                    {
                        //this.units.splice(i, 1);

                        gameCanvas.Children.Remove(this.units[i].rect);
                        units.Remove(this.units[i]);
                        i--;
                        survivers++;
                    }
                    else if (this.units[i].isDead)
                    {

                        kills++;
                        points += ((double) (1 + survivers)*handicap + (health/kills)*0.01 + 0.5);
                        handicap /= 1 + survivers;
                        unitCounter++;
                        gameCanvas.Children.Remove(this.units[i].rect);
                        units.Remove(this.units[i]);
                        i--;
                    }


                }



                for (var i = 0; i < this.bullets.Count; ++i)
                {
                    if (this.bullets[i].x > 0 && this.bullets[i].x < 500
                        && this.bullets[i].y > 0 && this.bullets[i].y < 500)
                        this.bullets[i].update();
                    else
                    {

                        gameCanvas.Children.Remove(this.bullets[i].rect);
                        bullets.Remove(this.bullets[i]);
                        i--;
                        continue;
                    }

                    for (var j = 0; j < this.units.Count; ++j)
                    {
                        if (this.bullets[i].x > this.units[j].x - this.units[j].size()/2
                            && this.bullets[i].x < this.units[j].x + this.units[j].size()/2
                            && this.bullets[i].y > this.units[j].y - this.units[j].size()/2
                            && this.bullets[i].y < this.units[j].y + this.units[j].size()/2)
                        {

                            gameCanvas.Children.Remove(this.bullets[i].rect);
                            bullets.Remove(this.bullets[i]);
                            this.units[j].health -= damage;
                            i--;
                            break;
                        }

                    }
                }
            }

        }
    }

}
