using System;
using System.Drawing;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace Boxygen.Drawing.Objects.Deco {
	public class TextureDecoration : Decoration {

		public Image Texture;
		public Anchor Anchor;
		public Vec2 Position;
		public Vec2 Size;

		public TextureDecoration(Face face, Image texture, Anchor anchor, Vec2 size) : base(face) {
			Texture = texture;
			Anchor = anchor;
			Size = size;
		}

		public override void Gather(RenderList list) {

			var faceO = Face.O.Pos;
			var faceA = Face.A.Pos;
			var faceB = Face.B.Pos;

			// Texture spans
			var spanA = (faceA - faceO).Normal * Size.Y;
			var spanB = (faceB - faceO).Normal * Size.X;

			// A and B displacements
			var dispA = faceA - spanA;
			var dispB = faceB - spanB;

			Vec3 origin;
			switch(Anchor) {
				case Anchor.TopLeft:
					origin = faceO + dispA;
					break;
				case Anchor.Top:
					origin = faceO + dispA + dispB / 2;
					break;
				case Anchor.TopRight:
					origin = faceO + dispA + dispB;
					break;
				case Anchor.Left:
					origin = faceO + dispA / 2;
					break;
				case Anchor.Center:
					origin = faceO + (dispA + dispB) / 2;
					break;
				case Anchor.Right:
					origin = faceO + dispA / 2 + dispB;
					break;
				case Anchor.BottomLeft:
					origin = faceO;
					break;
				case Anchor.Bottom:
					origin = faceO + dispB / 2;
					break;
				case Anchor.BottomRight:
					origin = faceO + dispB;
					break;
				case Anchor.Custom:
					origin = faceO + (faceA - faceO).Normal * Position.Y + (faceB - faceO).Normal * Position.X;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			//origin += Face.FrontFacingNormal;

			list.Add(new Tex(origin, origin + spanA, origin + spanB, Texture));
		}
	}
}
