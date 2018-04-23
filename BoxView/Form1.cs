using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Boxygen.Drawing;
using Boxygen.Drawing.Objects;
using Boxygen.Math;

namespace BoxView {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
			SetStyle(ControlStyles.DoubleBuffer, true);
			DrawTimer.Interval = 20;
		}

		public Box Box1 = new Box(200, 200, 100, 1, "Red");
		public Box Box2 = new Box(Math.Sqrt(2) * 100, Math.Sqrt(2) * 100, 100, 2, "Blue");
		public Box Box3 = new Box(Math.Sqrt(2) * Math.Sqrt(2) * 50, Math.Sqrt(2) * Math.Sqrt(2) * 50, 100, 3, "Green");
		public Box Box4 = new Box(Math.Sqrt(2) * Math.Sqrt(2) * Math.Sqrt(2) * 25, Math.Sqrt(2) * Math.Sqrt(2) * Math.Sqrt(2) * 25, 100, 1, "Red");

		public float SuperSampleFactor = 1;
		public DateTime LastFpsUpdate = DateTime.Now;
		public int LastFrameCounter;
		public int FrameCounter;

		private void PaintCanvas(object sender, PaintEventArgs e) {
			SuperSampleFactor = 2f;

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

			e.Graphics.DrawString("Render order:\n\n" + string.Join("\n", ordered), SystemFonts.StatusFont, Brushes.White, 5, 5);
			e.Graphics.DrawString($"~{LastFrameCounter} FPS\n{SuperSampleFactor * SuperSampleFactor}x SSAA", SystemFonts.StatusFont, Brushes.White, e.ClipRectangle.Right - 5, 5, new StringFormat { Alignment = StringAlignment.Far });
		}

		private List<Primitive> PaintCanvasInternal(Graphics g) {
			g.Clear(Color.Black);

			var list = new RenderList();
			Box1.Gather(list);
			Box2.Gather(list);
			Box3.Gather(list);
			Box4.Gather(list);
			return list.DrawInternal(g);
		}

		private void DrawTimer_Tick(object sender, EventArgs e) {
			Canvas.Invalidate();
			Canvas.Update();
		}

		private void Canvas_MouseWheel(object sender, MouseEventArgs e) {
			if(e.Delta > 0) {
				Box1.Transform.Rotate(Vec3.UnitZ, 1);
				Box2.Transform.Rotate(Vec3.UnitZ, -2);
				Box3.Transform.Rotate(Vec3.UnitZ, 4);
				Box4.Transform.Rotate(Vec3.UnitZ, -8);
			}
			else {
				Box1.Transform.Rotate(Vec3.UnitZ, -1);
				Box2.Transform.Rotate(Vec3.UnitZ, 2);
				Box3.Transform.Rotate(Vec3.UnitZ, -4);
				Box4.Transform.Rotate(Vec3.UnitZ, 8);
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
