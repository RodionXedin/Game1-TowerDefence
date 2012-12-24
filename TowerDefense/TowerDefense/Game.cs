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
        private int tileHeight = 40;
        private int tileWidth = 40;
        BitmapImage unitImg;
        BitmapImage towerImg;
        BitmapImage pathImage;
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
        private Map map;
        public Game(Canvas canvas, BitmapImage unitImg, BitmapImage towerImg, BitmapImage bulletImg)
        {
            this.gameCanvas = canvas;
            this.unitImg = unitImg;
            this.towerImg = towerImg;
            //tileHeight = Math.Max(this.unitImg.DecodePixelHeight, this.towerImg.DecodePixelHeight);
            //tileWidth = Math.Max(this.unitImg.DecodePixelWidth, this.towerImg.DecodePixelWidth);
            this.map = new Map((int)gameCanvas.Width, (int)canvas.Height, tileWidth, tileHeight);
            path.Add(map.GetTileCenter(158, 0));
			path.Add(map.GetTileCenter(158, 32));
            path.Add(map.GetTileCenter(50, 40));
			path.Add(map.GetTileCenter(50, 234));
			path.Add(map.GetTileCenter(154, 234));
			path.Add(map.GetTileCenter(160, 160));
			path.Add(map.GetTileCenter(330, 154));
			path.Add(map.GetTileCenter(334, 334));
			path.Add(map.GetTileCenter(58, 348));
			path.Add(map.GetTileCenter(50, 444));
			path.Add(map.GetTileCenter(440, 460));
			path.Add(map.GetTileCenter(450, 46));
			path.Add(map.GetTileCenter(286, 46));
			path.Add(map.GetTileCenter(286, -50));
            foreach (Point point in path)
            {
                map.SetTileType((int)point.X, (int)point.Y, ObjectType.Path);
            }
            for (int i = 0; i < path.Count - 1; ++i)
            {
                int divider = 20;
                int dx = (int)(path[i + 1].X - path[i].X)/divider;
                int dy = (int)(path[i + 1].Y - path[i].Y)/divider;
                for (int j = 0; j < divider; ++j)
                {
                    map.SetTileType((int)path[i].X + dx * j,
                        (int)path[i].Y + dy * j, ObjectType.Path);

                }
            }
            this.bulletImg = bulletImg;
            

            Point p = map.GetTileCenter(158, 0);
            units.Add(new Unit(gameCanvas, (int)p.X, (int)p.Y, unitImg, path, health));
            p = map.GetTileCenter(158, -60);
            units.Add(new Unit(gameCanvas, (int)p.X, (int)p.Y, unitImg, path, health));
            p = map.GetTileCenter(158, -120);
            units.Add(new Unit(gameCanvas, (int)p.X, (int)p.Y, unitImg, path, health));
            p = map.GetTileCenter(158, -180);
            units.Add(new Unit(gameCanvas, (int)p.X, (int)p.Y, unitImg, path, health));
            foreach (Unit unit in units)
            {
                map.SetTileType(unit.x, unit.y, ObjectType.Unit);
            }
            p = map.GetTileCenter(158, 60);
            
            towers.Add(new Tower(gameCanvas, (int)p.X, (int)p.Y, 5, shotDelay, 200, towerImg));
            foreach (Tower tower in towers)
            {
                map.SetTileType(tower.x, tower.y, ObjectType.Tower);
            }
            map.DrawTiles(gameCanvas);
            //map.DrawMap(gameCanvas);
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
            if (points >= towerCost && map.GetTileType((int)x, (int)y) == ObjectType.Empty)
            {
                points -= towerCost;
                Point p = map.GetTileCenter((int)x, (int)y);

                towers.Add(new Tower(gameCanvas, (int)p.X - tileWidth / 2,
                    (int)p.Y - tileHeight, 5, shotDelay, 200, towerImg));
                map.SetTileType((int)p.X, (int)p.Y, ObjectType.Tower);
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
                    Point p = map.GetTileCenter(158, 0);
                    units.Add(new Unit(gameCanvas, (int)p.X, (int)p.Y, unitImg, path, health));
                    p = map.GetTileCenter(158, -60);
                    units.Add(new Unit(gameCanvas, (int)p.X, (int)p.Y, unitImg, path, health));
                    p = map.GetTileCenter(158, -120);
                    units.Add(new Unit(gameCanvas, (int)p.X, (int)p.Y, unitImg, path, health));
                    p = map.GetTileCenter(158, -180);
                    units.Add(new Unit(gameCanvas, (int)p.X, (int)p.Y, unitImg, path, health));
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
