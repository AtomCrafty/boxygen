using System.Collections.Generic;
using System.Drawing;
using Boxygen.Drawing.Materials;
using Boxygen.Drawing.Objects.Decoration;
using Boxygen.Drawing.Primitives;

namespace Boxygen.Drawing.Objects {
	public class Face : Composite {

		public string Name;

		public Vertex O;
		public Vertex A;
		public Vertex B;

		public List<Deco> Deco = new List<Deco>();

		public Brush Fill = Brushes.White; //new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Palette.Cardboard(7), Palette.Cardboard(1));

		public Face(Vertex o, Vertex a, Vertex b) {
			O = o;
			A = a;
			B = b;
			//var tex = new Bitmap(@"D:\Projects\C#\Boxygen\Boxygen\bin\Debug\assets\symbols\symbol_03.png");
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.TopLeft));
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.Top));
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.TopRight));
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.Left));
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.Center));
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.Right));
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.BottomLeft));
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.Bottom));
			//Deco.Add(new TextureDeco(this, tex, new Vec2(30, 30), Anchor.BottomRight));
		}

		public override void Gather(RenderList list) {
			var stack = new Stack(new Quad(O.Pos, A.Pos, B.Pos) { Material = new DirectedBrushMaterial(), Name = Name }, list);
			//var stack = new Stack(new Tri(O.Pos, A.Pos + (B.Pos - O.Pos) / 2, B.Pos) { Material = new DirectedBrushMaterial(), Name = Name }, list);
			Deco.ForEach(stack.RenderList.Visit);
			list.Add(stack);
		}
	}
}
