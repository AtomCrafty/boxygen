using System.Drawing;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public class Stack : Primitive {

		public Primitive Surface;
		public RenderList RenderList = new RenderList { SortPrimitives = false };

		public Stack(Primitive surface, RenderList list) : base(surface.O, surface.A, surface.B) {
			Surface = surface;
			RenderList.Transform.Apply(list.Transform);
			RenderList.Add(Surface);
		}

		public override Vec3 CenterOfMass => Surface.CenterOfMass;
		public override Vec3[] Vertices => Surface.Vertices;
		public override double Area => Surface.Area;

		public override void Draw(Graphics g) {
			RenderList.Draw(g);
		}
	}
}
