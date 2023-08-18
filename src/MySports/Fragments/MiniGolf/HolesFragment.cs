using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MySports.Adapters.MiniGolf;
using MySports.Models.MiniGolf;
using Newtonsoft.Json;

namespace MySports.Fragments.MiniGolf
{
    public class HolesFragment : SportFragment, IMenuProvider
    {
        private Round _round;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (Arguments != null)
            {
                _round = JsonConvert.DeserializeObject<Round>(Arguments.GetString("@string/round"));
            }

			Activity.AddMenuProvider(this);

			return inflater.Inflate(Resource.Layout.holes, container, false);
        }

		public override void OnDestroyView()
		{
			Activity.RemoveMenuProvider(this);
			base.OnDestroyView();
		}

		public void OnCreateMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.holes_menu, menu);
        }

        public bool OnMenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_edit:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_round, null);

                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Update Round");
                    builder.SetView(view);
                    builder.SetPositiveButton("Update", UpdateRoundAction);
                    builder.SetNeutralButton("Delete", DeleteRoundAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    DateEditText = view.FindViewById<EditText>(Resource.Id.create_update_round_date);
                    DateEditText.Text = _round.Date;
                    DateEditText.Click += delegate
                    {
                        OnClickDateEditText();
                    };

                    view.FindViewById<TextView>(Resource.Id.create_update_round_id).Text = _round.Id.ToString();
                    view.FindViewById<EditText>(Resource.Id.create_update_round_description).Text = _round.Description;
                    view.FindViewById<EditText>(Resource.Id.create_update_round_player_one).Text = _round.PlayerOne;
                    view.FindViewById<EditText>(Resource.Id.create_update_round_player_two).Text = _round.PlayerTwo;

					builder.Show();
                    return true;
                }
                case Resource.Id.action_add:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_hole, null);

                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Create Hole");
                    builder.SetView(view);
                    builder.SetPositiveButton("Create", CreateHoleAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    // Set the player labels.
                    view.FindViewById<TextView>(Resource.Id.create_update_hole_player_one).Text = $"{_round.PlayerOne}:";
                    view.FindViewById<TextView>(Resource.Id.create_update_hole_player_two).Text = $"{_round.PlayerTwo}:";

						builder.Show();
                    return true;
                }
            }
            return false;
        }

        private void UpdateRoundAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            _round.Description = alertDialog.FindViewById<EditText>(Resource.Id.create_update_round_description).Text;
            _round.Date = alertDialog.FindViewById<EditText>(Resource.Id.create_update_round_date).Text;
            _round.PlayerOne = alertDialog.FindViewById<EditText>(Resource.Id.create_update_round_player_one).Text;
            _round.PlayerTwo = alertDialog.FindViewById<EditText>(Resource.Id.create_update_round_player_two).Text;

            DbHelper.UpdateRound(_round);
            LoadData();
        }

        private void DeleteRoundAction(object sender, DialogClickEventArgs e)
        {
            DbHelper.DeleteRound(_round.Id);

            string text = $"{_round.Description} has been deleted";
            Toast.MakeText(Activity.Application, text, ToastLength.Short).Show();

            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, new RoundsFragment(), "roundsFragment").Commit();
        }

        private void CreateHoleAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string number = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_number).Text;
            string description = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_description).Text;
            string par = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_par).Text;
            string playerOneScore = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_player_one_score).Text;
            string playerTwoScore = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_player_two_score).Text;

            Hole hole = new Hole()
            {
                RoundId = Convert.ToInt32(_round.Id),
                Number = Convert.ToInt32(number),
                Description = description,
                Par = Convert.ToInt32(par),
                PlayerOneScore = Convert.ToInt32(playerOneScore),
                PlayerTwoScore = Convert.ToInt32(playerTwoScore)
            };

            DbHelper.CreateHole(hole);
            LoadData();
        }

        public void UpdateHoleAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string id = alertDialog.FindViewById<TextView>(Resource.Id.create_update_hole_id).Text;
            string number = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_number).Text;
            string description = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_description).Text;
            string par = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_par).Text;
            string playerOneScore = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_player_one_score).Text;
            string playerTwoScore = alertDialog.FindViewById<EditText>(Resource.Id.create_update_hole_player_two_score).Text;

            Hole hole = new Hole()
            {
                Id = Convert.ToInt32(id),
                Number = Convert.ToInt32(number),
                Description = description,
                Par = Convert.ToInt32(par),
                PlayerOneScore = Convert.ToInt32(playerOneScore),
                PlayerTwoScore = Convert.ToInt32(playerTwoScore)
            };

            DbHelper.UpdateHole(hole);
            LoadData();
        }

        public void DeleteHoleAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string id = alertDialog.FindViewById<TextView>(Resource.Id.create_update_hole_id).Text;
            string description = alertDialog.FindViewById<TextView>(Resource.Id.create_update_hole_description).Text;

            DbHelper.DeleteHole(Convert.ToInt32(id));
            LoadData();

            string text = $"{description} has been deleted";
            Toast.MakeText(Activity.Application, text, ToastLength.Short).Show();
        }

        public override void LoadData()
        {
            Activity.Title = _round.Description;

            List<Hole> holes = DbHelper.GetHoles(_round.Id);

            string totalPar = holes.Sum(detail => Convert.ToInt32(detail.Par)).ToString();
            string totalPlayerOne = holes.Sum(detail => Convert.ToInt32(detail.PlayerOneScore)).ToString();
            string totalPlayerTwo = holes.Sum(detail => Convert.ToInt32(detail.PlayerTwoScore)).ToString();

            Activity.FindViewById<TextView>(Resource.Id.par_total).Text = $"Par: {totalPar}";
            Activity.FindViewById<TextView>(Resource.Id.player_one_total).Text = $"{_round.PlayerOne}: {totalPlayerOne}";
            Activity.FindViewById<TextView>(Resource.Id.player_two_total).Text = $"{_round.PlayerTwo}: {totalPlayerTwo}";

            HolesAdapter holesAdapter = new HolesAdapter(this, holes, _round);
            ListView.Adapter = holesAdapter;
        }
    }
}
