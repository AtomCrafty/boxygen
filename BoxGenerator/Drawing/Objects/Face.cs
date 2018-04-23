using System.Collections.Generic;
using System.Drawing;
using Boxygen.Drawing.Objects.Deco;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace Boxygen.Drawing.Objects {
	public class Face : Composite {

		public string Name;

		public Vertex O;
		public Vertex A;
		public Vertex B;

		public Vec3 FrontFacingNormal {
			get {
				var normal = ((A.Pos - O.Pos) & (B.Pos - O.Pos)).Normal;
				return normal * System.Math.Sign(normal.ViewDistance);
			}
		}

		public List<Decoration> Deco = new List<Decoration>();

		public Brush Fill = Brushes.White; //new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Palette.Cardboard(7), Palette.Cardboard(1));

		public Face(Vertex o, Vertex a, Vertex b) {
			O = o;
			A = a;
			B = b;
			var tex = new Bitmap(@"D:\OneDrive\Dokumente\Stuff\opening-closed-cardboard-boxes-isometric-illustration-set-box-open-delivery-packaging-vector-96025389.jpg");
			Deco.Add(new TextureDecoration(this, tex, Anchor.Custom, new Vec2(40, 40)) { Position = new Vec2(10, 10) });
		}

		public override void Gather(RenderList list) {
			var stack = new Stack(new Quad(O.Pos, A.Pos, B.Pos) { Fill = Fill, Name = Name }, list);
			Deco.ForEach(stack.RenderList.Visit);
			list.Add(stack);
		}
	}
}
