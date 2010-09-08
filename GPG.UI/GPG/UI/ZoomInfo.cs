namespace GPG.UI
{
    using System;

    public class ZoomInfo
    {
        public int HorizontalTiles = 100;
        public string Prefix = "small_";
        public int TileHeight = 0xd8;
        public int TileWidth = 0xd8;
        public const int TOTAL_HEIGHT = 0x2a30;
        public const int TOTAL_WIDTH = 0x5460;
        public int VerticleTiles = 50;
        public float ZoomThreshold = 1f;

        public float RelativeX(float x)
        {
            int num = this.TileHeight * this.HorizontalTiles;
            return (num * (x / 21600f));
        }

        public float RelativeY(float y)
        {
            int num = this.TileHeight * this.VerticleTiles;
            return (num * (y / 10800f));
        }
    }
}

