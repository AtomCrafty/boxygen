using System.Collections.Generic;
using System.Drawing;

namespace Boxygen {
	public class Palette {
		public static Dictionary<string, Color> Colors = new Dictionary<string, Color> {
			{"cardboard_1", Color.FromArgb(unchecked((int)0xFFFFDDA8))},
			{"cardboard_2", Color.FromArgb(unchecked((int)0xFFEDC893))},
			{"cardboard_3", Color.FromArgb(unchecked((int)0xFFDDA866))},
			{"cardboard_4", Color.FromArgb(unchecked((int)0xFFD2A576))},
			{"cardboard_5", Color.FromArgb(unchecked((int)0xFFC68B51))},
			{"cardboard_6", Color.FromArgb(unchecked((int)0xFFBC7C46))},
			{"cardboard_7", Color.FromArgb(unchecked((int)0xFFAA6838))},
			{"tape_shadow", Color.FromArgb(unchecked((int)0xFFA36438))},
			{"tape_light" , Color.FromArgb(unchecked((int)0xFFF5DFBE))},
		};

		public static Color Cardboard(int shade) => Colors["cardboard_" + shade];
	}
}
