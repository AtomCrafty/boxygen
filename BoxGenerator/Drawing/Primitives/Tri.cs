using System.Drawing;
using System.Drawing.Drawing2D;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public class Tri : Primitive {
		protected static readonly Point[] UnitTriangle = { new Point(0, 0), new Point(0, 1), new Point(1, 0) };

		public Tri() {
			Fill = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Palette.Cardboard(7), Palette.Cardboard(1));
		}

		public override Vec3 CenterOfMass => Origin + (SpanA + SpanB) / 3;
		public override double Area => (SpanA & SpanB).Length / 2;

		public override void Draw(Graphics g, Vec2 center) {
			g.SetTransform(center, Origin, SpanA, SpanB);
			g.FillPolygon(Fill, UnitTriangle);
		}
	}
}
