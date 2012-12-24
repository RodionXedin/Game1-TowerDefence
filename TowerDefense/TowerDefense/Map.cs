using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace TowerDefense.TowerDefense
{
    internal enum ObjectType
    {
        Empty,
        Tower,
        Unit,
        Path
    };


    internal class Map
    {
        private readonly int _height;
        private readonly List<ObjectType> _map;
        public Dictionary<ObjectType, ImageSource> _images;

        private readonly int _tileHeight;
        private readonly int _tileWidth;
        private readonly int _totalXTiles;

        private readonly int _totalYTiles;
        private readonly int _width;

        public Map(int width, int height, int tileWidth, int tileHeight)
        {
            _width = width;
            _height = height;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _totalXTiles = (int)Math.Ceiling((double)width / tileWidth);
            _totalYTiles = (int)Math.Ceiling((double)height / tileHeight);
            _map = new List<ObjectType>(_totalXTiles * _totalYTiles);
            for (int i = 0; i < _map.Capacity; i++)
            {
                _map.Add(ObjectType.Empty);
            }
            _images = new Dictionary<ObjectType,ImageSource>();
        }

        public int TotalXTiles
        {
            get { return _totalXTiles; }
        }

        public int TotalYTiles
        {
            get { return _totalYTiles; }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int TileWidth
        {
            get { return _tileWidth; }
        }

        public int TileHeight
        {
            get { return _tileHeight; }
        }

        public ObjectType GetTileType(int x, int y)
        {
            int tileX = x / TileWidth;
            int tileY = y / TileHeight;
            if (tileX <= _totalXTiles && tileY <= _totalYTiles && tileX >= 0 && tileY >= 0)
            {
                return _map[tileY * _totalXTiles + tileX];
            }
            return ObjectType.Empty;
        }

        public void SetTileType(int x, int y, ObjectType type)
        {
            int tileX = x / TileWidth;
            int tileY = y / TileHeight;
            if (tileX <= _totalXTiles && tileY <= _totalYTiles && tileX >= 0 && tileY >= 0)
            {
                _map[tileY * _totalXTiles + tileX] = type;
            }
        }

        public Point GetTileCenter(int x, int y)
        {
            int tileX = x / TileWidth;
            int tileY = y / TileHeight;
            Point res = new Point(tileX * TileWidth + TileWidth / 2, tileY * TileHeight + TileHeight / 2);
            return res;
        }

        public Rectangle GetTileCoordinates(int x, int y)
        {
            int tileX = x / TileWidth;
            int tileY = y / TileHeight;
            Rectangle res = new Rectangle();
            res.SetValue(Canvas.LeftProperty, tileX * TileWidth);
            res.SetValue(Canvas.TopProperty, tileY*TileHeight);
            res.Height = TileHeight;
            res.Width = TileWidth;
            return res;
        }

        public void DrawTiles(Canvas canvas)
        {
            for (int i = 0; i < TotalYTiles; ++i)
            {
                for (int j = 0; j < TotalXTiles; ++j)
                {
                    int tileX = i * TileHeight;
                    int tileY = j * TileWidth;
                    Rectangle curTile = GetTileCoordinates(tileX, tileY);
                    curTile.Fill = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    curTile.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
                    canvas.Children.Add(curTile);
                }
            }
        }

        public void DrawMap(Canvas canvas)
        {
            for (int i = 0; i < TotalYTiles; ++i)
            {
                for (int j = 0; j < TotalXTiles; ++j)
                {
                    int tileX = i * TileHeight;
                    int tileY = j * TileWidth;
                    Rectangle curTile = GetTileCoordinates(tileX, tileY);
                    ImageBrush imBrush = new ImageBrush();
                    imBrush.ImageSource = _images[ObjectType.Path];
                    if (_map[tileX * i + tileX] == ObjectType.Path)
                    {
                        curTile.Fill = imBrush;
                        canvas.Children.Add(curTile);
                    }
                }
            }
        }
    }
}
    ;