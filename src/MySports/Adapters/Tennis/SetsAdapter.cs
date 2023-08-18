using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using MySports.Fragments.Tennis;
using MySports.Models.Tennis;

namespace MySports.Adapters.Tennis
{
    public class SetsAdapter : SportAdapter<Set>
    {
        private SetsFragment _setsFragment;
        private Match _match;

        public SetsAdapter(SetsFragment setsFragment, List<Set> sets, Match match)
            : base (sets)
        {
            this._setsFragment = setsFragment;
            this._match = match;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_setsFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.set_list_item, null);

            Set set = Items[position];

            view.FindViewById<TextView>(Resource.Id.set_number).Text = $"Set #{set.Number}";
            view.FindViewById<TextView>(Resource.Id.set_player_one_score).Text = $"{_match.PlayerOne}: {set.PlayerOneScore}";
            view.FindViewById<TextView>(Resource.Id.set_player_two_score).Text = $"{_match.PlayerTwo}: {set.PlayerTwoScore}";

            view.Click += delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(_setsFragment.Activity);
                View view = layoutInflater.Inflate(Resource.Layout.create_update_set, null);

                AlertDialog.Builder builder = new AlertDialog.Builder(_setsFragment.Activity);
				builder.SetTitle("Update Set");
				builder.SetView(view);
				builder.SetPositiveButton("Update", _setsFragment.UpdateSetAction);
				builder.SetNeutralButton("Delete", _setsFragment.DeleteSetAction);
				builder.SetNegativeButton("Cancel", _setsFragment.CancelAction);

                // Set the player labels.
                view.FindViewById<TextView>(Resource.Id.create_update_set_player_one).Text = $"{_match.PlayerOne}:";
                view.FindViewById<TextView>(Resource.Id.create_update_set_player_two).Text = $"{_match.PlayerTwo}:";

                // Prepopulate the fields.
                view.FindViewById<TextView>(Resource.Id.create_update_set_id).Text = set.Id.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_set_number).Text = set.Number.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_set_player_one_score).Text = set.PlayerOneScore.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_set_player_two_score).Text = set.PlayerTwoScore.ToString();

                builder.Show();
            };

            return view;
        }
    }
}
