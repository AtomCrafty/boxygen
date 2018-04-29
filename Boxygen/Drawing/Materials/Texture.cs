using System.Drawing;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Boxygen.Drawing.Materials {
	public class Texture {
		public string Path { get; protected set; }
		[JsonIgnore]
		public Bitmap Image { get; protected set; }

		public Texture(string path) {
			Path = path;
			Image = new Bitmap(path);
		}

		[OnDeserialized]
		protected void OnDeserialized(StreamingContext context) {
			Image = new Bitmap(Path);
		}

		public static implicit operator Bitmap(Texture tex) => tex.Image;
	}
}
