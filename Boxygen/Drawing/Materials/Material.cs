using System.Drawing;
using Boxygen.Drawing.Primitives;

namespace Boxygen.Drawing.Materials {
	public abstract class Material {
		public static Material Default = new FlatBrushMaterial();

		public abstract void DrawPolygon(RenderContext ctx, Graphics g, Primitive p);
	}
}
