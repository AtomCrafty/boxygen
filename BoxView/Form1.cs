using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Boxygen.Drawing;
using Boxygen.Drawing.Objects;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace BoxView {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
			SetStyle(ControlStyles.DoubleBuffer, true);

			Canvas.Paint += PaintCanvas;

			//Box.Transform.Rotate(Vec3.UnitZ, 30);
			//Box.Transform.Rotate(Vec3.UnitX, -90);
		}

		public Box Box1 = new Box(200, 200, 100, 1, "Red");
		public Box Box2 = new Box(Math.Sqrt(2) * 100, Math.Sqrt(2) * 100, 100, 2, "Blue");
		public Box Box3 = new Box(Math.Sqrt(2) * Math.Sqrt(2) * 50, Math.Sqrt(2) * Math.Sqrt(2) * 50, 100, 3, "Green");

		public DateTime LastFpsUpdate = DateTime.Now;
		public int LastFrameCounter;
		public int FrameCounter;

		private void PaintCanvas(object sender, PaintEventArgs e) {
			SuperSampleFactor = 0.5f;

			int bufferWidth = (int)(Canvas.Width * SuperSampleFactor);
			int bufferHeight = (int)(Canvas.Height * SuperSampleFactor);

			using(var bitmap = new Bitmap(bufferWidth, bufferHeight))
			using(var g = Graphics.FromImage(bitmap)) {
				g.Clear(Color.Black);
				g.TranslateTransform(bufferWidth / 2f, bufferHeight / 2f);
				g.ScaleTransform(SuperSampleFactor, SuperSampleFactor);

				var list = new RenderList();
				Box1.Gather(list);
				Box2.Gather(list);
				Box3.Gather(list);
				var ordered = list.DrawInternal(g);
				//bitmap.Save("test.png");

				e.Graphics.InterpolationMode = SuperSampleFactor > 1 ? InterpolationMode.Bilinear : InterpolationMode.NearestNeighbor;
				g.PixelOffsetMode = PixelOffsetMode.Half;
				if(Math.Abs(SuperSampleFactor - 1) < 0.0001) e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
				else e.Graphics.DrawImage(bitmap, 0, 0, Canvas.Width, Canvas.Height);

				e.Graphics.DrawString("Render order:\n\n" + string.Join("\n", ordered), SystemFonts.StatusFont, Brushes.White, 5, 5);

				var now = DateTime.Now;
				FrameCounter++;
				if(now > LastFpsUpdate.AddSeconds(1)) {
					LastFpsUpdate = now;
					LastFrameCounter = FrameCounter;
					FrameCounter = 0;
				}
				e.Graphics.DrawString($"~{LastFrameCounter} FPS", SystemFonts.StatusFont, Brushes.White, e.ClipRectangle.Right - 5, 5, new StringFormat { Alignment = StringAlignment.Far });
			}
		}

		private void DrawTimer_Tick(object sender, EventArgs e) {
			//Box1.Transform.Rotate(Vec3.UnitZ, 0.25);
			//Box2.Transform.Rotate(Vec3.UnitZ, -0.5);
			//Box3.Transform.Rotate(Vec3.UnitZ, 1);
			Canvas.Invalidate();
			Canvas.Update();
		}

		private void Canvas_MouseWheel(object sender, MouseEventArgs e) {
			if(e.Delta > 0) {
				Box1.Transform.Rotate(Vec3.UnitZ, 1);
				Box2.Transform.Rotate(Vec3.UnitZ, -2);
				Box3.Transform.Rotate(Vec3.UnitZ, 4);
			}
			else {
				Box1.Transform.Rotate(Vec3.UnitZ, -1);
				Box2.Transform.Rotate(Vec3.UnitZ, 2);
				Box3.Transform.Rotate(Vec3.UnitZ, -4);
			}
			//Canvas.Invalidate();
			//Canvas.Update();
		}

		private void Form1_Load(object sender, EventArgs e) {
			ClientSize = new Size(512, 512);
		}
	}

	public sealed class Display : Panel {
		public Display() {
			DoubleBuffered = true;
		}
	}
}
