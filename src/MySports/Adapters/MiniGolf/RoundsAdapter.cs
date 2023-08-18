using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MySports.Fragments.MiniGolf;
using MySports.Models.MiniGolf;
using Newtonsoft.Json;

namespace MySports.Adapters.MiniGolf
{
    public class RoundsAdapter : SportAdapter<Round>
    {
        private RoundsFragment _roundsFragment;

        public RoundsAdapter(RoundsFragment roundsFragment, List<Round> rounds)
            : base(rounds)
        {
            _roundsFragment = roundsFragment;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_roundsFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.round_list_item, null);

            Round round = Items[position];

            view.FindViewById<TextView>(Resource.Id.round_description).Text = round.Description;
            view.FindViewById<TextView>(Resource.Id.round_date).Text = round.Date;
            view.FindViewById<TextView>(Resource.Id.round_players).Text = $"{round.PlayerOne} vs {round.PlayerTwo}";

            view.Click += delegate
            {
                HolesFragment fragment = new HolesFragment();
                Bundle bundle = new Bundle();
                bundle.PutString("@string/round", JsonConvert.SerializeObject(round));
                fragment.Arguments = bundle;

                _roundsFragment.Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment, "holesFragment").AddToBackStack("roundsFragment").Commit();
            };

            return view;
        }
    }
}
