using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MySports.Fragments.Tennis;
using MySports.Models.Tennis;
using Newtonsoft.Json;

namespace MySports.Adapters.Tennis
{
    public class MatchesAdapter : SportAdapter<Match>
    {
        private MatchesFragment _matchesFragment;

        public MatchesAdapter(MatchesFragment matchesFragment, List<Match> matches)
            : base(matches)
        {
            _matchesFragment = matchesFragment;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_matchesFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.match_list_item, null);

            Match match = Items[position];

            view.FindViewById<TextView>(Resource.Id.match_description).Text = match.Description;
            view.FindViewById<TextView>(Resource.Id.match_date).Text = match.Date;
            view.FindViewById<TextView>(Resource.Id.match_players).Text = $"{match.PlayerOne} vs {match.PlayerTwo}";

            view.Click += delegate
            {
                SetsFragment fragment = new SetsFragment();
                Bundle bundle = new Bundle();
                bundle.PutString("@string/match", JsonConvert.SerializeObject(match));
                fragment.Arguments = bundle;

                _matchesFragment.Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment, "setsFragment").AddToBackStack("matchesFragment").Commit();
            };

            return view;
        }
    }
}
