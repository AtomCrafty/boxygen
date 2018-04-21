using System.Drawing;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public class Tex : Quad {

		public Bitmap Texture;

		public Tex(Bitmap texture) {
			Texture = texture;
		}

		public override void Draw(Graphics g, Vec2 center) {
			g.SetTransform(center, Origin, SpanA, SpanB);
			g.DrawImage(Texture, UnitRect);
		}
	}
}
