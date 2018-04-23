using System.Drawing;
using System.Drawing.Drawing2D;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public class Tri : Primitive {
		protected static readonly Point[] UnitTriangle = { new Point(0, 0), new Point(0, 1), new Point(1, 0) };

		public Tri(Vec3 o, Vec3 a, Vec3 b) : base(o, a, b) {
			Fill = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Palette.Cardboard(7), Palette.Cardboard(1));
		}

		public override Vec3 CenterOfMass => O + (SpanA + SpanB) / 3;
		public override Vec3[] Vertices => new[] { O, A, B };
		public override double Area => (SpanA & SpanB).Length / 2;

		public override void Draw(Graphics g) {
			var o = O.Project();
			var a = SpanA.Project();
			var b = SpanB.Project();

			var poly = new PointF[] { o, o + a, o + b };

			g.FillPolygon(Fill, poly);
			g.DrawPolygon(Stroke, poly);
		}
	}
}
