using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using DiscordApi.LeagueApi.Models;

namespace DiscordApi.Drawing
{

    //считаю минимальный винрейт у человека
    //считаю максимальный винрейт у человека
    //ys
    public static class DrawingChart
    {
        public static int Width { get; set; } = 500;
        public static int Height { get; set; } = 300;
        public static int Margin { get; set; } = 20;
        /// <summary>
        /// 
        /// </summary>
        /// <returns>path to image</returns>
        public static string DrawAndSave(int playerGames)//List<PlayerGameStat> playerGames
        {
            float scale = (float)(Width - Margin * 2) / playerGames;

            Bitmap bitmap = new Bitmap(Width, Height);

            Graphics g = Graphics.FromImage(bitmap);
            g.PageUnit = GraphicsUnit.Pixel;
            g.Clear(Color.White);
            Pen blackPen = new Pen(Color.Black, 0.5f);
            //внешняя рамочка
            g.DrawLine(new Pen(Color.Black, 4f), new Point(0, 0), new Point(0, Height));
            g.DrawLine(new Pen(Color.Black, 4f), new Point(0, 0), new Point(Width, 0));
            g.DrawLine(new Pen(Color.Black, 4f), new Point(Width, 0), new Point(Width, Height));
            g.DrawLine(new Pen(Color.Black, 4f), new Point(0, Height), new Point(Width, Height));

      

            //внутренняя рамочка
            g.DrawLine(new Pen(Color.Black, 1f), new Point(Margin, Margin), new Point(Margin, Height - Margin));
            g.DrawLine(new Pen(Color.Black, 1f), new Point(Margin, Margin), new Point(Width - Margin, Margin));
            g.DrawLine(new Pen(Color.Black, 1f), new Point(Width - Margin, Margin), new Point(Width - Margin, Height - Margin));
            g.DrawLine(new Pen(Color.Black, 1f), new Point(Margin, Height - Margin), new Point(Width - Margin, Height - Margin));

            //отрисовка
            g.DrawLine(new Pen(Color.Red, 2f), new PointF(xs(scale, 0), 100), new PointF(xs(scale, 50), 100));
            g.DrawLine(new Pen(Color.Green, 2f), new PointF(xs(scale, 50), 100), new PointF(xs(scale, 100), 100));

           


            bitmap.Save(@"E:\pict\kub.png");

            return @"E:\pict\kub.png";
        }

        public static float xs(float scale, float x)
        {
            return Margin + (float)(x * scale);
        }
    }
}
