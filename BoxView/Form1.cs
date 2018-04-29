using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Boxygen.Drawing;
using Boxygen.Drawing.Materials;
using Boxygen.Drawing.Objects;
using Boxygen.Drawing.Objects.Decoration;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace BoxView {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
			SetStyle(ControlStyles.DoubleBuffer, true);
			DrawTimer.Interval = 20;
		}

		public Scene Scene;
		public Box Box1 = new Box(200, 200, 100, 1, "Red");
		public Box Box2 = new Box(Math.Sqrt(2) * 100, Math.Sqrt(2) * 100, 100, 2, "Blue");
		public Box Box3 = new Box(Math.Sqrt(2) * Math.Sqrt(2) * 50, Math.Sqrt(2) * Math.Sqrt(2) * 50, 100, 3, "Green");
		public Box Box4 = new Box(Math.Sqrt(2) * Math.Sqrt(2) * Math.Sqrt(2) * 25, Math.Sqrt(2) * Math.Sqrt(2) * Math.Sqrt(2) * 25, 100, 1, "Red");

		public readonly RenderContext Context = new RenderContext {
			//LightNormal = new Vec3(-1, 0, -1).Normal,
			LightNormal = new Vec3(-3, 6, -3).Normal,
			AmbientIntensity = 0.6,
			DiffuseIntensity = 1.0
		};

		public float SuperSampleFactor = 2;
		public DateTime LastFpsUpdate = DateTime.Now;
		public int LastFrameCounter;
		public int FrameCounter;

		private void PaintCanvas(object sender, PaintEventArgs e) {
			List<Primitive> ordered;
			if(Math.Abs(SuperSampleFactor - 1) < 0.0001) {
				e.Graphics.TranslateTransform(Canvas.Width / 2f, Canvas.Height / 2f);
				ordered = PaintCanvasInternal(e.Graphics);
				e.Graphics.ResetTransform();
			}
			else {
				int bufferWidth = (int)(Canvas.Width * SuperSampleFactor);
				int bufferHeight = (int)(Canvas.Height * SuperSampleFactor);

				using(var bitmap = new Bitmap(bufferWidth, bufferHeight))
				using(var g = Graphics.FromImage(bitmap)) {
					g.TranslateTransform(bufferWidth / 2f, bufferHeight / 2f);
					g.ScaleTransform(SuperSampleFactor, SuperSampleFactor);

					ordered = PaintCanvasInternal(g);

					e.Graphics.InterpolationMode = SuperSampleFactor > 1 ? InterpolationMode.Bilinear : InterpolationMode.NearestNeighbor;
					g.PixelOffsetMode = PixelOffsetMode.Half;
					if(Math.Abs(SuperSampleFactor - 1) < 0.0001) e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
					else e.Graphics.DrawImage(bitmap, 0, 0, Canvas.Width, Canvas.Height);
				}
			}

			var now = DateTime.Now;
			FrameCounter++;
			if(now > LastFpsUpdate.AddSeconds(1)) {
				LastFpsUpdate = now;
				LastFrameCounter = FrameCounter;
				FrameCounter = 0;
			}

			var light = Context.LightNormal.Project() * 50;
			e.Graphics.DrawLine(Pens.Red, Canvas.Width - 50, 50, Canvas.Width - 50 + (float)light.X, 50 + (float)light.Y);
			e.Graphics.DrawString("Render order:\n\n" + string.Join("\n", ordered), SystemFonts.StatusFont, Brushes.Black, 5, 5);
			e.Graphics.DrawString($"~{LastFrameCounter} FPS\n{SuperSampleFactor * SuperSampleFactor}x SSAA", SystemFonts.StatusFont, Brushes.Black, e.ClipRectangle.Right - 5, 5, new StringFormat { Alignment = StringAlignment.Far });
		}

		private List<Primitive> PaintCanvasInternal(Graphics g) {
			g.Clear(Color.Black);

			var box = new Box(150, 250, 100, 1);
			//using(var r = new StreamReader("test.box"))
			//Scene = new JsonSerializer().Deserialize<Scene>(new JsonTextReader(r));
			Scene = new Scene { box };
			box.Right.Deco.Add(new TextureDeco(box.Right, new Texture(@"D:\Projects\C#\Boxygen\Boxygen\bin\Debug\assets\symbols\symbol_03.png"), new Vec2(30, 30), DecoAnchor.Custom) { Name = "Up Arrows Icon", Position = new Vec2() });
			box.Right.Deco.Add(new TextureDeco(box.Right, new Texture(@"D:\Projects\C#\Boxygen\Boxygen\bin\Debug\assets\symbols\symbol_05.png"), new Vec2(30, 30), DecoAnchor.Custom) { Name = "Umbrella Icon", Position = new Vec2(35, 0) });
			box.Right.Deco.Add(new TextureDeco(box.Right, new Texture(@"D:\Projects\C#\Boxygen\Boxygen\bin\Debug\assets\symbols\symbol_24.png"), new Vec2(30, 30), DecoAnchor.Custom) { Name = "Fragile Icon", Position = new Vec2(70, 0) });

			//Scene.Save("test.box");


			var list = new RenderList {
				new Quad(
					new Vec3(-1000, -1000, 0),
					new Vec3(-1000, 1000, 0),
					new Vec3(1000, -1000, 0)
				) {
					CastsShadows = false,
					Material = new DirectedBrushMaterial {
						Color = Color.White
					}
				}
			};
			//Scene.Gather(list);
			Box1.Gather(list);
			//Box2.Gather(list);
			//Box3.Gather(list);
			//Box4.Gather(list);
			Context.Polygons = list.ToList();
			return list.DrawInternal(Context, g);
		}

		private void DrawTimer_Tick(object sender, EventArgs e) {
			Canvas.Invalidate();
			Canvas.Update();
		}

		private void Canvas_MouseWheel(object sender, MouseEventArgs e) {
			var transform = new Transform();
			if(e.Delta > 0) {
				Box1.Transform.Rotate(Vec3.UnitZ, 1);
				Box2.Transform.Rotate(Vec3.UnitZ, -2);
				Box3.Transform.Rotate(Vec3.UnitZ, 4);
				Box4.Transform.Rotate(Vec3.UnitZ, -8);
				transform.Rotate(Vec3.UnitZ, -0.5);
				Context.LightNormal = transform.TransformVector(Context.LightNormal);
			}
			else {
				Box1.Transform.Rotate(Vec3.UnitZ, -1);
				Box2.Transform.Rotate(Vec3.UnitZ, 2);
				Box3.Transform.Rotate(Vec3.UnitZ, -4);
				Box4.Transform.Rotate(Vec3.UnitZ, 8);
				transform.Rotate(Vec3.UnitZ, 0.5);
				Context.LightNormal = transform.TransformVector(Context.LightNormal);
			}
			//Canvas.Invalidate();
			//Canvas.Update();
		}

		private void Form1_Load(object sender, EventArgs e) {
			ClientSize = new Size(512, 512);
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e) {
			switch(e.KeyChar) {
				case '+':
					SuperSampleFactor *= 2;
					break;
				case '-':
					SuperSampleFactor /= 2;
					break;
			}
		}
	}

	public sealed class Display : Panel {
		public Display() {
			DoubleBuffered = true;
		}
	}
}
