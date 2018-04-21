using System;
using System.Drawing;
using Boxygen.Drawing;
using Boxygen.Drawing.Objects;
using Boxygen.Math;

namespace Boxygen {
	public static class Program {

		public static void Main() {
			Console.WriteLine(Vec3.UnitX & Vec3.UnitY);

			using(var bitmap = Render(
				new Face {
					BaseBrush = new SolidBrush(Color.CadetBlue),
					Vertecies = new[] {
						new Vec3(0, 0, -200),
						new Vec3(-200, 0, -200),
						new Vec3(0, 0, 0)
					}
				},
				new Face {
					BaseBrush = new SolidBrush(Color.DarkCyan),
					Vertecies = new[] {
						new Vec3(0, 0, -200),
						new Vec3(0, -200, -200),
						new Vec3(0, 0, 0)
					}
				},
				new Face {
					BaseBrush = new SolidBrush(Color.Aquamarine),
					Vertecies = new[] {
						new Vec3(0, 0, 0),
						new Vec3(-200, 0, 0),
						new Vec3(0, -200, 0)
					}
				})) bitmap.Save("test.png");


			var box = new Box(200, 200, 100, 1);
			using(var bitmap = Render(box)) {
				bitmap.Save("box.png");
			}

			//Console.ReadLine();
		}

		public static Bitmap Render(params IDrawable[] objects) {
			var bitmap = new Bitmap(512, 512);
			using(var g = Graphics.FromImage(bitmap)) {
				//g.Clear(Color.Transparent);
				g.Clear(Color.Black);
				foreach(var drawable in objects) {
					drawable.Draw(g, new Vec2(256, 256));
				}
			}
			return bitmap;
		}
	}
}