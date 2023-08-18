using Android.OS;
using Android.Views;

using Fragment = AndroidX.Fragment.App.Fragment;

namespace MyWorkoutAndroid
{
    public class HomeFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Activity.Title = Resources.GetString(Resource.String.app_name);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.home, container, false);
        }
    }
}
