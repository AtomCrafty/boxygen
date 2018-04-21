using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Boxygen.Math {
	public static class Extensions {

		private static readonly Random Rand = new Random();

		public static void Shuffle<T>(this IList<T> list) {
			int n = list.Count;
			while(n > 1) {
				n--;
				int k = Rand.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		#region Drawing

		public static bool SetTransform(this Graphics g, Vec2 center, Vec3 origin, Vec3 spanA, Vec3 spanB)
			=> SetTransform(g, center, origin.Project(), spanA.Project(), spanB.Project());

		public static bool SetTransform(this Graphics g, Vec2 center, Vec2 origin, Vec2 spanA, Vec2 spanB) {
			float x0 = 256 + (float)origin.X, y0 = 256 + (float)origin.Y;
			float ax = (float)spanA.X, ay = (float)spanA.Y;
			float bx = (float)spanB.X, by = (float)spanB.Y;

			if(System.Math.Abs(ax) < 0.0001 && System.Math.Abs(bx) < 0.0001) return false;
			g.Transform = new Matrix(ax, ay, bx, by, x0, y0);
			return true;
		}

		#endregion

		public static T Select<T>(this Random random, params T[] items) => items[random.Next(items.Length)];
	}
}
