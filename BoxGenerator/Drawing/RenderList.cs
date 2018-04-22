using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Boxygen.Math;

namespace Boxygen.Drawing {
	public class RenderList : IDrawable, IEnumerable<Primitive> {

		private readonly List<Primitive> _primitives = new List<Primitive>();
		private readonly Stack<Transform> _transforms = new Stack<Transform>();

		public bool RenderBackFaces = true;
		public bool RenderNormals = false;

		public RenderList() {
			_transforms.Push(Transform.Identity);
		}

		public List<Primitive> DrawInternal(Graphics g) {

			// back face culling
			if(!RenderBackFaces) _primitives.RemoveAll(p => (p.Normal | Vec3.Camera) > 0);

			// topologically sort
			var list = OrderingGraph<Primitive>.Sort(_primitives, Primitive.OrderSelector);

			// draw polygons
			foreach(var drawable in list) {
				//sb.AppendLine(drawable.ToString());
				drawable.Draw(g);
				if(RenderNormals) drawable.DrawNormals(g, false);
			}

			return list;
		}

		public void Draw(Graphics g) {
			DrawInternal(g);
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
