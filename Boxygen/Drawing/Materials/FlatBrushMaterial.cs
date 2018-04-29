using System.Drawing;
using Boxygen.Drawing.Primitives;

namespace Boxygen.Drawing.Materials {
	public class FlatBrushMaterial : Material {

		public Brush Fill = Brushes.Transparent;
		public Pen Stroke = Pens.Transparent;

		public override void DrawPolygon(RenderContext ctx, Graphics g, Primitive p) {
			g.FillPolygon(Fill, p.Points);
			g.DrawPolygon(Stroke, p.Points);
		}
	}
}
