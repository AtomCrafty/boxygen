using Boxygen.Math;
using System.Drawing;

namespace Boxygen.Drawing.Primitives {
	public class Tex : Quad {

		public Bitmap Texture;

		public Tex(Bitmap texture) {
			Texture = texture;
		}

		public override void Draw(Graphics g) {
			var o = Origin.Project();
			var a = SpanA.Project();
			var b = SpanB.Project();

			var poly = new PointF[] { o, o + a, o + b };

			g.DrawImage(Texture, poly);
		}
	}
}
