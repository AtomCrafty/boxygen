using System.Collections;
using System.Collections.Generic;

namespace Boxygen.Drawing.Objects {
	public class Group : Composite, IList<Composite> {
		public List<Composite> Children = new List<Composite>();

		public override void Gather(RenderList list) {
			list.PushTransform(Transform);
			Children.ForEach(list.Visit);
			list.PopTransform();
		}

		#region Forwarded methods

		public IEnumerator<Composite> GetEnumerator() => Children.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();
		public void Add(Composite item) => Children.Add(item);
		public void Clear() => Children.Clear();
		public bool Contains(Composite item) => Children.Contains(item);
		public void CopyTo(Composite[] array, int arrayIndex) => Children.CopyTo(array, arrayIndex);
		public bool Remove(Composite item) => Children.Remove(item);
		public int Count => Children.Count;
		public bool IsReadOnly => false;
		public int IndexOf(Composite item) => Children.IndexOf(item);
		public void Insert(int index, Composite item) => Children.Insert(index, item);
		public void RemoveAt(int index) => Children.RemoveAt(index);
		public Composite this[int index] {
			get => Children[index];
			set => Children[index] = value;
		}

		#endregion
	}
}
