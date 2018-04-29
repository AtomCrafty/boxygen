using System.Drawing;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public class Quad : Primitive {
		protected static readonly Rectangle UnitRect = new Rectangle(0, 0, 1, 1);

		public Quad(Vec3 o, Vec3 a, Vec3 b) : base(o, a, b) { }

		public override Vec3 CenterOfMass => (A + B) / 2;
		public override Vec3[] Vertices => new[] { O, A, A + B - O, B };
		public override double Area => (SpanA & SpanB).Length;
	}
}