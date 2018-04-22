using System;
using System.Drawing;
using Boxygen.Drawing;
using Boxygen.Drawing.Objects;
using Boxygen.Math;

namespace Boxygen {
	public static class Program {

		public static void Main() {
		}

		public static Bitmap Render(params IDrawable[] objects) {
			var bitmap = new Bitmap(512, 512);
			using(var g = Graphics.FromImage(bitmap)) {
				//g.Clear(Color.Transparent);
				g.Clear(Color.Black);
				foreach(var drawable in objects) {
					drawable.Draw(g);
				}
			}
			return bitmap;
		}
	}
}