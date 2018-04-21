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
					Color.FromArgb(unchecked((int)0xffff1111))
				} :
				palette == 2 ? new[] {
					Color.FromArgb(unchecked((int)0xffccccff)),
					Color.FromArgb(unchecked((int)0xff9999ff)),
					Color.FromArgb(unchecked((int)0xff6666ff)),
					Color.FromArgb(unchecked((int)0xff3333ff)),
					Color.FromArgb(unchecked((int)0xff1111ff))
				} :
				palette == 3 ? new[] {
					Color.FromArgb(unchecked((int)0xffccffcc)),
					Color.FromArgb(unchecked((int)0xff99ff99)),
					Color.FromArgb(unchecked((int)0xff66ff66)),
					Color.FromArgb(unchecked((int)0xff33ff33)),
					Color.FromArgb(unchecked((int)0xff11ff11))
				} : throw new ArgumentOutOfRangeException();

			Back = new Face { BaseBrush = new SolidBrush(colors[3]), Name = Name + " " + nameof(Back) };
			Right = new Face { BaseBrush = new SolidBrush(colors[4]), Name = Name + " " + nameof(Right) };
			Bottom = new Face { BaseBrush = new SolidBrush(colors[0]), Name = Name + " " + nameof(Bottom) };
			Front = new Face { BaseBrush = new SolidBrush(colors[1]), Name = Name + " " + nameof(Front) };
			Left = new Face { BaseBrush = new SolidBrush(colors[2]), Name = Name + " " + nameof(Left) };

			FlapBack = new Face { BaseBrush = new SolidBrush(colors[3]), Name = Name + " " + nameof(FlapBack) };
			FlapRight = new Face { BaseBrush = new SolidBrush(colors[4]), Name = Name + " " + nameof(FlapRight) };
			FlapFront = new Face { BaseBrush = new SolidBrush(colors[1]), Name = Name + " " + nameof(FlapFront) };
			FlapLeft = new Face { BaseBrush = new SolidBrush(colors[2]), Name = Name + " " + nameof(FlapLeft) };

			Update();
		}

		public void Update() {
			double halfWidth = Width / 2;
			double halfDepth = Depth / 2;
			double flapLength = 50;

			Back.Vertecies[0].Set(-halfDepth, -halfWidth, 0);
			Back.Vertecies[1].Set(-halfDepth, -halfWidth, Height);
			Back.Vertecies[2].Set(-halfDepth, halfWidth, 0);

			Right.Vertecies[0].Set(-halfDepth, -halfWidth, 0);
			Right.Vertecies[1].Set(-halfDepth, -halfWidth, Height);
			Right.Vertecies[2].Set(halfDepth, -halfWidth, 0);

			Bottom.Vertecies[0].Set(-halfDepth, -halfWidth, 0);
			Bottom.Vertecies[1].Set(-halfDepth, halfWidth, 0);
			Bottom.Vertecies[2].Set(halfDepth, -halfWidth, 0);

			Front.Vertecies[0].Set(halfDepth, -halfWidth, 0);
			Front.Vertecies[1].Set(halfDepth, -halfWidth, Height);
			Front.Vertecies[2].Set(halfDepth, halfWidth, 0);

			Left.Vertecies[0].Set(-halfDepth, halfWidth, 0);
			Left.Vertecies[1].Set(-halfDepth, halfWidth, Height);
			Left.Vertecies[2].Set(halfDepth, halfWidth, 0);

			FlapFront.Vertecies[0].Set(halfDepth, halfWidth, Height);
			FlapFront.Vertecies[1].Set(halfDepth, -halfWidth, Height);
			FlapFront.Vertecies[2] = FlapFront.Vertecies[0] + new Vec3(1, 0, -2).Normal * flapLength;
		}

		public override void Gather(RenderList list) {
			list.PushTransform(Transform);

			Right.Gather(list);
			Back.Gather(list);
			Bottom.Gather(list);
			Left.Gather(list);
			Front.Gather(list);

			//FlapLeft.Gather(list);
			FlapFront.Gather(list);
			//FlapRight.Gather(list);
			//FlapBack.Gather(list);

			list.PopTransform();
		}
	}
}
