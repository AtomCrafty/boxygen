using System.Drawing;
using System.Linq;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public class Stack : Primitive {

		public Primitive Surface;
		public RenderList RenderList = new RenderList { SortPrimitives = false };

		public override Vec3 CenterOfMass => Surface.CenterOfMass;
		public override Vec3[] Vertices => Surface.Vertices;
		public override double Area => Surface.Area;

		public Stack(Primitive surface, RenderList list) : base(surface.O, surface.A, surface.B) {
			Name = surface.Name;
			Surface = surface;
			RenderList.Transform.Apply(list.Transform);
			RenderList.Add(Surface);
		}

		public override void Draw(RenderContext ctx, Graphics g) {
			RenderList.Draw(ctx, g);
		}

		public override string ToString() => "Stack {\n" + string.Join("\n", RenderList.Primitives.Select(p => "    " + p.ToString().Replace("\n", "\n    "))) + "\n}";
	}
}
