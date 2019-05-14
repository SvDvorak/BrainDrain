using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    public static class Extensions
    {
        public static float Rotation(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static void DrawLine(this SpriteBatch sb, Vector2 pos, float length, Color color, float rotation = 0, float thickness = 1, float depth = 0)
        {
            sb.Draw(BrainGame.Pixel, pos, null, color, rotation, new Vector2(0, 0.5f), new Vector2(length, thickness), SpriteEffects.None, depth);
        }
        public static void DrawLine(this SpriteBatch sb, Vector2 pos, Vector2 pos2, Color color, float thickness = 1, float depth = 0)
        {
            var diff = pos2 - pos;
            var len = diff.Length();
            var rot = diff.Rotation();
            DrawLine(sb, pos, len, color, rot, thickness, depth);
        }

        public static Vector2 ToV2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static bool IsPointOnLine(Vector2 start, Vector2 end, float width, Vector2 point)
        {
            var AP = point - start;       //Vector from start to point   
            var AB = end - start;       //Vector from start to end  

            var magnitudeAB = AB.LengthSquared();     //Magnitude of start-end vector (it's length squared)     
            var ABAPproduct = Vector2.Dot(AP, AB);    //The DOT product of a_to_p and a_to_b     
            var alongLine = ABAPproduct / magnitudeAB; //The normalized "distance" from start to your closest point  

            var onLine = start + AB * alongLine;
            if (alongLine < 0)
                onLine = start;
            if (alongLine > 1)
                onLine = end;
            var distance = (point - onLine).Length();

            return distance < width;
        }

        public static float NextFloat(this Random random)
        {
            return (float) random.NextDouble();
        }
    }
}
