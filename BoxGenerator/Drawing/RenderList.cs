using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Boxygen.Math;

namespace Boxygen.Drawing {
	public class RenderList : IDrawable, IEnumerable<Primitive> {

		private readonly List<Primitive> _primitives = new List<Primitive>();
		private readonly Stack<Transform> _transforms = new Stack<Transform>();

		public RenderList() {
			_transforms.Push(Transform.Identity);
		}

		public void Draw(Graphics g, Vec2 center) {
			var sb = new StringBuilder("Render order:\n\n");

			foreach(var p in _primitives/*.Where(p => (p.Normal | Camera) > 0)*/) {
				//p.Fill = new SolidBrush(Color.Aquamarine);
				//Console.WriteLine(p.Normal.ViewDistance);
			}
			// topologically sort
			var list = OrderingGraph<Primitive>.Sort(_primitives, Primitive.OrderSelector);

			foreach(var drawable in list) {
				sb.AppendLine(drawable.ToString());
				//Console.WriteLine(drawable.GetType().Name);
				drawable.Draw(g, center);
				//drawable.DrawNormals(g, center, false);
			}
			Console.WriteLine();

			g.ResetTransform(); foreach(var drawable in list) {
				//drawable.DrawNormals(g, center, true);
			}
			g.DrawString(sb.ToString(), SystemFonts.StatusFont, Brushes.White, 10, 10);
		}

		public Transform Transform => _transforms.Peek();

		public void PushTransform(Transform transform = null) {
			_transforms.Push(Transform.Copy());
			if(transform != null) Transform.Apply(transform);
		}

		public void PopTransform() {
			_transforms.Pop();
		}

		public void Add(Primitive item) {
			item.Transform(Transform);
			_primitives.Add(item);
		}

		#region Forwarded methods

		public IEnumerator<Primitive> GetEnumerator() => _primitives.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _primitives.GetEnumerator();

		#endregion
	}
}
