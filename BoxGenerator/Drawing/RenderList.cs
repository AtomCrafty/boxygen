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
			var sb = new StringBuilder();

			foreach(var p in _primitives/*.Where(p => (p.Normal | Camera) > 0)*/) {
				//p.Fill = new SolidBrush(Color.Aquamarine);
				//Console.WriteLine(p.Normal.ViewDistance);
			}
			var list = new List<Primitive>();
			// sort
			if(false)
				foreach(var primitive in _primitives) {
					if(primitive.Area < 0.0001) continue;
					if(list.Count == 0) {
						list.Add(primitive);
						continue;
					}

					// find last layer that must be drawn behind the new one
					int lowLimit = list.FindLastIndex(p => SelectOrder(p, primitive) < 0);
					// find first layer that must be drawn on top of the new one
					int highLimit = list.FindIndex(p => SelectOrder(p, primitive) > 0);

					if(lowLimit == -1) lowLimit = 0;
					if(highLimit == -1) highLimit = list.Count;

					if(lowLimit > highLimit) Console.WriteLine("RIP");

					list.Insert(highLimit, primitive);
				}

			list = OrderingGraph<Primitive>.Sort(_primitives, Comparer<Primitive>.Create(SelectOrder));

			foreach(var drawable in list) {
				sb.AppendLine(drawable.ToString());
				//Console.WriteLine(drawable.GetType().Name);
				drawable.Draw(g, center);
				drawable.DrawNormals(g, center, false);
			}
			Console.WriteLine();

			g.ResetTransform(); foreach(var drawable in list) {
				//drawable.DrawNormals(g, center, true);
			}
			g.DrawString(sb.ToString(), SystemFonts.StatusFont, Brushes.White, 10, 10);
		}

		private int SelectOrder(Primitive p1, Primitive p2) {
			if(!Intersect(p1, p2)) return 0;

			var n1 = p1.Normal * System.Math.Sign(p1.Normal.ViewDistance);
			var n2 = p2.Normal * System.Math.Sign(p2.Normal.ViewDistance);

			var o1 = p1.Origin - p2.CenterOfMass;
			var o2 = p2.Origin - p1.CenterOfMass;

			if(p2.Name == "Green Right" && p1.Name == "Green Front") ;

			int state = 0;

			// all vertecies are projected onto the camera-facing normal
			// if the value is < 0, they are behind, if it is > 0, they are in front

			if((o2 | n1) <= 0.0001 &&
			   ((o2 + p2.SpanA) | n1) <= 0.0001 &&
			   ((o2 + p2.SpanB) | n1) <= 0.0001 &&
			   ((o2 + p2.SpanA + p2.SpanB) | n1) <= 0.0001) {
				// all vertecies of p2 are behind the p1 plane
				//Console.WriteLine($"{p2.Name} is behind {p1.Name}");
				state += 1;
			}

			if((o2 | n1) >= -0.0001 &&
			   ((o2 + p2.SpanA) | n1) >= -0.0001 &&
			   ((o2 + p2.SpanB) | n1) >= -0.0001 &&
			   ((o2 + p2.SpanA + p2.SpanB) | n1) >= -0.0001) {
				// all vertecies of p2 are in front of the p1 plane
				//Console.WriteLine($"{p1.Name} is in front of {p2.Name}");
				state -= 1;
			}

			if((o1 | n2) <= 0.0001 &&
			   ((o1 + p1.SpanA) | n2) <= 0.0001 &&
			   ((o1 + p1.SpanB) | n2) <= 0.0001 &&
			   ((o1 + p1.SpanA + p1.SpanB) | n2) <= 0.0001) {
				// all vertecies of p1 are behind the p2 plane
				//Console.WriteLine($"{p1.Name} is behind {p2.Name}");
				state -= 1;
			}

			if((o1 | n2) >= -0.0001 &&
			   ((o1 + p1.SpanA) | n2) >= -0.0001 &&
			   ((o1 + p1.SpanB) | n2) >= -0.0001 &&
			   ((o1 + p1.SpanA + p1.SpanB) | n2) >= -0.0001) {
				// all vertecies of p1 are in front of the p2 plane
				//Console.WriteLine($"{p1.Name} is in front of {p2.Name}");
				state += 1;
			}

			//if(((p2.Origin - p1.CenterOfMass) | n1) <= 0.0001 &&
			//   ((p2.Origin + p2.SpanA - p1.CenterOfMass) | n1) <= 0.0001 &&
			//   ((p2.Origin + p2.SpanB - p1.CenterOfMass) | n1) <= 0.0001 &&
			//   ((p2.Origin + p2.SpanA + p2.SpanB - p1.CenterOfMass) | n1) <= 0.0001) {
			//	Console.WriteLine($"{p1.Name} is in front of {p2.Name}");
			//	state += 1;
			//}

			//if(((p2.Origin - p1.CenterOfMass) | n1) >= -0.0001 &&
			//   ((p2.Origin + p2.SpanA - p1.CenterOfMass) | n1) >= -0.0001 &&
			//   ((p2.Origin + p2.SpanB - p1.CenterOfMass) | n1) >= -0.0001 &&
			//   ((p2.Origin + p2.SpanA + p2.SpanB - p1.CenterOfMass) | n1) >= -0.0001) {
			//	Console.WriteLine($"{p1.Name} is behind {p2.Name}");
			//	state -= 1;
			//}

			//if(((p1.Origin - p2.CenterOfMass) | n2) <= 0.0001 &&
			//   ((p1.Origin + p1.SpanA - p2.CenterOfMass) | n2) <= 0.0001 &&
			//   ((p1.Origin + p1.SpanB - p2.CenterOfMass) | n2) <= 0.0001 &&
			//   ((p1.Origin + p1.SpanA + p1.SpanB - p2.CenterOfMass) | n2) <= 0.0001) {
			//	Console.WriteLine($"{p2.Name} can be in front of {p1.Name}");
			//	state -= 1;
			//}

			//if(((p1.Origin - p2.CenterOfMass) | n2) >= -0.0001 &&
			//   ((p1.Origin + p1.SpanA - p2.CenterOfMass) | n2) >= -0.0001 &&
			//   ((p1.Origin + p1.SpanB - p2.CenterOfMass) | n2) >= -0.0001 &&
			//   ((p1.Origin + p1.SpanA + p1.SpanB - p2.CenterOfMass) | n2) >= -0.0001) {
			//	Console.WriteLine($"{p1.Name} can be in front of {p2.Name}");
			//	state += 1;
			//}

			//if(state == 0) Console.WriteLine("NONE");
			//else Console.WriteLine(state);

			return state;
		}

		public bool Intersect(Primitive p1, Primitive p2) {
			// TODO Intersect
			return true;
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
