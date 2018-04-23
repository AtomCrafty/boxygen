using System.Drawing;
using Boxygen.Math;

namespace Boxygen.Drawing {
	public abstract class Composite : IDrawable {

		public Transform Transform = Transform.Identity;
		public abstract void Gather(RenderList list);

		public void Draw(Graphics g) {
			var list = new RenderList();
			Gather(list);
			list.Draw(g);
		}
	}
}
