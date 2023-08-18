using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using MySports.Fragments.MiniGolf;
using MySports.Models.MiniGolf;

namespace MySports.Adapters.MiniGolf
{
    public class HolesAdapter : SportAdapter<Hole>
    {
        private HolesFragment _holesFragment;
        private Round _round;

        public HolesAdapter(HolesFragment holesFragment, List<Hole> holes, Round round)
            : base (holes)
        {
            this._holesFragment = holesFragment;
            this._round = round;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_holesFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.hole_list_item, null);

            Hole hole = Items[position];

            view.FindViewById<TextView>(Resource.Id.hole_number_description).Text = $"#{hole.Number} {hole.Description}";
            view.FindViewById<TextView>(Resource.Id.hole_par).Text = $"Par {hole.Par}";
            view.FindViewById<TextView>(Resource.Id.hole_player_one_score).Text = $"{_round.PlayerOne}: {hole.PlayerOneScore}";
            view.FindViewById<TextView>(Resource.Id.hole_player_two_score).Text = $"{_round.PlayerTwo}: {hole.PlayerTwoScore}";

            view.Click += delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(_holesFragment.Activity);
                View view = layoutInflater.Inflate(Resource.Layout.create_update_hole, null);

                AlertDialog.Builder builder = new AlertDialog.Builder(_holesFragment.Activity);
                builder.SetTitle("Update Hole");
				builder.SetView(view);
				builder.SetPositiveButton("Update", _holesFragment.UpdateHoleAction);
				builder.SetNeutralButton("Delete", _holesFragment.DeleteHoleAction);
				builder.SetNegativeButton("Cancel", _holesFragment.CancelAction);

                // Set the player labels.
                view.FindViewById<TextView>(Resource.Id.create_update_hole_player_one).Text = $"{_round.PlayerOne}:";
                view.FindViewById<TextView>(Resource.Id.create_update_hole_player_two).Text = $"{_round.PlayerTwo}:";

                // Prepopulate the fields.
                view.FindViewById<TextView>(Resource.Id.create_update_hole_id).Text = hole.Id.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_hole_number).Text = hole.Number.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_hole_description).Text = hole.Description;
                view.FindViewById<EditText>(Resource.Id.create_update_hole_par).Text = hole.Par.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_hole_player_one_score).Text = hole.PlayerOneScore.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_hole_player_two_score).Text = hole.PlayerTwoScore.ToString();

                builder.Show();
            };

            return view;
        }
    }
}
