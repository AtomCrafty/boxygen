using System;
using System.Drawing;
using Boxygen.Math;

namespace Boxygen.Drawing.Objects {
	public class Box : Composite {
		public double Width;
		public double Depth;
		public double Height;
		public double Rim;

		public string Name;

		public Face Back, Right, Bottom, Front, Left, FlapBack, FlapRight, FlapFront, FlapLeft;

		public Vertex[] Vertices;

		public Box(double width, double depth, double height, int palette, string name = "Box") {
			Width = width;
			Depth = depth;
			Height = height;
			Name = name;

			var colors =
				palette == 1 ? new[] {
					Color.FromArgb(unchecked((int)0xffffcccc)),
					Color.FromArgb(unchecked((int)0xffff9999)),
					Color.FromArgb(unchecked((int)0xffff6666)),
					Color.FromArgb(unchecked((int)0xffff3333)),
					Color.FromArgb(unchecked((int)0xffff0000))
				} :
				palette == 2 ? new[] {
					Color.FromArgb(unchecked((int)0xffccccff)),
					Color.FromArgb(unchecked((int)0xff9999ff)),
					Color.FromArgb(unchecked((int)0xff6666ff)),
					Color.FromArgb(unchecked((int)0xff3333ff)),
					Color.FromArgb(unchecked((int)0xff0000ff))
				} :
				palette == 3 ? new[] {
					Color.FromArgb(unchecked((int)0xffccffcc)),
					Color.FromArgb(unchecked((int)0xff99ff99)),
					Color.FromArgb(unchecked((int)0xff66ff66)),
					Color.FromArgb(unchecked((int)0xff33ff33)),
					Color.FromArgb(unchecked((int)0xff00ff00))
				} : throw new ArgumentOutOfRangeException();

			double sx = width / 2;
			double sy = depth / 2;
			double sz = height;

			Vertex v0, v1, v2, v3, v4, v5, v6, v7;

			Vertices = new[] {
				v0 = new Vertex(new Vec3(-sx,  sy,  0)),
				v1 = new Vertex(new Vec3(-sx, -sy,  0)),
				v2 = new Vertex(new Vec3( sx, -sy,  0)),
				v3 = new Vertex(new Vec3( sx,  sy,  0)),
				v4 = new Vertex(new Vec3(-sx,  sy, sz)),
				v5 = new Vertex(new Vec3(-sx, -sy, sz)),
				v6 = new Vertex(new Vec3( sx, -sy, sz)),
				v7 = new Vertex(new Vec3( sx,  sy, sz)),
			};

			Back = new Face(v2, v6, v1) { Fill = new SolidBrush(colors[3]), Name = Name + " " + nameof(Back) };
			Right = new Face(v3, v7, v2) { Fill = new SolidBrush(colors[4]), Name = Name + " " + nameof(Right) };
			Bottom = new Face(v0, v3, v1) { Fill = new SolidBrush(colors[0]), Name = Name + " " + nameof(Bottom) };
			Front = new Face(v0, v4, v3) { Fill = new SolidBrush(colors[1]), Name = Name + " " + nameof(Front) };
			Left = new Face(v1, v5, v0) { Fill = new SolidBrush(colors[2]), Name = Name + " " + nameof(Left) };

			//FlapBack = new Face { BaseBrush = new SolidBrush(colors[3]), Name = Name + " " + nameof(FlapBack) };
			//FlapRight = new Face { BaseBrush = new SolidBrush(colors[4]), Name = Name + " " + nameof(FlapRight) };
			//FlapFront = new Face { BaseBrush = new SolidBrush(colors[1]), Name = Name + " " + nameof(FlapFront) };
			//FlapLeft = new Face { BaseBrush = new SolidBrush(colors[2]), Name = Name + " " + nameof(FlapLeft) };

		}

		public override void Gather(RenderList list) {
			list.PushTransform(Transform);

			Left.Gather(list);
			Front.Gather(list);
			Bottom.Gather(list);
			Right.Gather(list);
			Back.Gather(list);

			//FlapLeft.Gather(list);
			//FlapFront.Gather(list);
			//FlapRight.Gather(list);
			//FlapBack.Gather(list);

			list.PopTransform();
		}
	}
}
