using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using DiscordApi.LeagueApi.Models;
using System.Drawing.Text;

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
        public static string DrawAndSave(List<PlayerGameStat> playerGames)
        {
            Console.WriteLine("пошёл процесс");
            float scaleX = (float)(Width - Margin * 2) / playerGames.Count;
            float scaleY = (float)(Height - Margin * 2) / 100; // 100- винрейт от 0% до 100%
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

            //рисуем деления
            //по y
            for(int i = 0; i <= 100; i += 5)
            {
                g.DrawString($"{i}", new Font(new FontFamily(GenericFontFamilies.SansSerif), 5f),
                   Brushes.Black, new PointF(1, ys(scaleY, (float)i)));
            }
            //кол-во игр
            g.DrawString($"{playerGames.Count}", new Font(new FontFamily(GenericFontFamilies.SansSerif), 5f),
                   Brushes.Black, new PointF(xs(scaleX, playerGames.Count), ys(scaleY, 0)));
            
            int countWins = 0;
            if (playerGames[0].PlayerStat.Win == true)
                countWins++;
            float x = 0;
            float y = countWins / 1;

            for (int i = 1; i < playerGames.Count; i++)
            {
                if (playerGames[i].PlayerStat.Win == true)
                    countWins++;
                //дичь
                Console.WriteLine(countWins);
                float winrate = (float)((int)((float)((float)countWins / (float)(i + 1)) * 100f * 100f)) / 100f;
                g.DrawLine(new Pen(Color.Goldenrod, 1f), new PointF(xs(scaleX, x), ys(scaleY, y)),
                    new PointF(xs(scaleX, i), ys(scaleY, winrate)));
                x = i;
                y = winrate;
                Console.WriteLine(y);
            }
            //красная строка - 50% винрейт
            g.DrawLine(new Pen(Color.Red, 0.1f), new PointF(xs(scaleX, 0), ys(scaleY, 50)), 
                new PointF(xs(scaleX, playerGames.Count), ys(scaleY, 50)));

            //ник
            g.DrawString($"{playerGames.First().PlayerStat.SummonerName} винрейт {y}%",
                new Font(new FontFamily(GenericFontFamilies.SansSerif), 10f),
                   Brushes.Crimson,
                   new PointF(xs(scaleX, 0), 0));

            Directory.CreateDirectory(@"D:\pict");
            bitmap.Save(@"D:\pict\kub.png");

            return @"D:\pict\kub.png";
        }

        public static float xs(float scale, float x)
        {
            return Margin + (float)(x * scale);
        }

        public static float ys(float scale, float y)
        {
            return (Height - Margin) - (float)(y * scale);
        }
    }
}
