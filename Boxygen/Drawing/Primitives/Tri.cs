using System.Drawing;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public class Tri : Primitive {
		protected static readonly Point[] UnitTriangle = { new Point(0, 0), new Point(0, 1), new Point(1, 0) };

		public Tri(Vec3 o, Vec3 a, Vec3 b) : base(o, a, b) { }

		public override Vec3 CenterOfMass => O + (SpanA + SpanB) / 3;
		public override Vec3[] Vertices => new[] { O, A, B };
		public override double Area => (SpanA & SpanB).Length / 2;
	}
}
