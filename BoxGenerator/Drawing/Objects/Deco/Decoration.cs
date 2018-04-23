namespace Boxygen.Drawing.Objects.Deco {
	public abstract class Decoration : Composite {
		public Face Face;

		protected Decoration(Face face) {
			Face = face;
		}
	}

	public enum Direction {
		Horizontal, Vertical, Upward, Downward
	}

	public enum Anchor {
		Custom,
		TopLeft, Top, TopRight,
		Left, Center, Right,
		BottomLeft, Bottom, BottomRight
	}
}
