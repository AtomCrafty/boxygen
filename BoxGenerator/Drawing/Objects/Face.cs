using System.Drawing;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace Boxygen.Drawing.Objects {
	public class Face : Composite {

		public string Name;

		public Vec3[] Vertecies = { new Vec3(), new Vec3(), new Vec3() };
		public Brush BaseBrush = Brushes.White; //new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Palette.Cardboard(7), Palette.Cardboard(1));

		// TODO temp
		private static Bitmap texture = new Bitmap(@"C:\Users\mario\Pictures\stamps.PNG");

		public override void Gather(RenderList list) {
			list.Add(new Quad { Fill = BaseBrush, Origin = Vertecies[0], SpanA = Vertecies[1] - Vertecies[0], SpanB = Vertecies[2] - Vertecies[0], Name = Name });
			//list.Add(new Tri { Fill = BaseBrush, Origin = Vertecies[0], SpanA = Vertecies[1] - Vertecies[0], SpanB = Vertecies[2] - Vertecies[0], Name = Name });
			//list.Add(new Tex(texture) { Origin = Vertecies[0], SpanA = Vertecies[1] - Vertecies[0], SpanB = Vertecies[2] - Vertecies[0], Name = Name });
		}
	}
}
