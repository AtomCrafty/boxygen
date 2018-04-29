namespace Boxygen.Drawing.Objects.Decoration {
	public abstract class Deco : Composite {
		public Face Face;

		protected Deco(Face face) {
			Face = face;
		}
	}

	public enum Direction {
		Horizontal, Vertical, Upward, Downward
	}

	public enum DecoAnchor {
		Custom,
		TopLeft, Top, TopRight,
		Left, Center, Right,
		BottomLeft, Bottom, BottomRight
	}
}
