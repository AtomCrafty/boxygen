using Boxygen.Math;
using System.Drawing;

namespace Boxygen.Drawing.Primitives {
	public class Tex : Quad {

		public Image Texture;

		public Tex(Vec3 o, Vec3 a, Vec3 b, Image texture) : base(o, a, b) {
			Texture = texture;
		}

		public override void Draw(Graphics g) {
			var o = O.Project();
			var a = SpanA.Project();
			var b = SpanB.Project();

			var poly = new PointF[] { o, o + b, o + a };

			g.DrawImage(Texture, poly);
		}
	}
}
