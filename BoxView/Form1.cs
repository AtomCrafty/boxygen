using System;
using System.Drawing;
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

		private void PaintCanvas(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Color.Black);

			var list = new RenderList();
			Box1.Gather(list);
			//Box2.Gather(list);
			Box3.Gather(list);
			list.Draw(e.Graphics, new Vec2(256, 256));
			return;
			new RenderList {
				new Quad {
					Fill = Brushes.Tomato,
					Origin = new Vec3(-100, -100, 0),
					SpanA = new Vec3(200, 0, 0),
					SpanB = new Vec3(0, 200, 0),
				},
				new Tri {
					Fill = Brushes.CornflowerBlue,
					Origin = new Vec3(-100, 100, -100),
					SpanA = new Vec3(000, -200, 0),
					SpanB = new Vec3(0, 0, 100),
				}
			}.Draw(e.Graphics, new Vec2(256, 256));
		}

		private void DrawTimer_Tick(object sender, System.EventArgs e) {
			return;
			Box1.Transform.Rotate(Vec3.UnitZ, 0.25);
			Box2.Transform.Rotate(Vec3.UnitZ, -0.5);
			Box3.Transform.Rotate(Vec3.UnitZ, 1);
			Canvas.Invalidate();
			Canvas.Update();
		}

		private void Canvas_Click(object sender, EventArgs e) {
		}

		private void CanvasOnMouseWheel(object sender, MouseEventArgs e) {
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
			Canvas.Invalidate();
			Canvas.Update();
		}
	}

	public sealed class Display : Panel {
		public Display() {
			DoubleBuffered = true;
		}
	}
}
