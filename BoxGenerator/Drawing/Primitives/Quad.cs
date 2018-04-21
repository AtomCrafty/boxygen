using System.Drawing;
using System.Drawing.Drawing2D;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public class Quad : Primitive {
		protected static readonly Rectangle UnitRect = new Rectangle(0, 0, 1, 1);

		public Quad() {
			Fill = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Palette.Cardboard(7), Palette.Cardboard(1));
		}

		public override Vec3 CenterOfMass => Origin + (SpanA + SpanB) / 2;
		public override double Area => (SpanA & SpanB).Length;

		public override void Draw(Graphics g, Vec2 center) {
			g.SetTransform(center, Origin, SpanA, SpanB);
			g.FillRectangle(Fill, UnitRect);

			var o = Origin.Project();
			var a = SpanA.Project();
			var b = SpanB.Project();

			g.ResetTransform();
			g.DrawPolygon(Stroke, new[] {
				(PointF) (center + o        ),
				(PointF) (center + o + a    ),
				(PointF) (center + o + a + b),
				(PointF) (center + o     + b),
				//(PointF) (center + o        ),
			});
		}
	}
}
