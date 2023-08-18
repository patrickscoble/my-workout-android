using System.Collections.Generic;
using Android.Widget;

namespace MyWorkoutAndroid.Adapters
{
    public abstract class SportAdapter<T> : BaseAdapter
    {
        protected List<T> Items;

        public override int Count { get { return Items.Count; } }

        public SportAdapter(List<T> items)
        {
            this.Items = items;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}
