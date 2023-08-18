using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MySports.Adapters.Tennis;
using MySports.Models.Tennis;
using Newtonsoft.Json;

namespace MySports.Fragments.Tennis
{
    public class SetsFragment : SportFragment, IMenuProvider
    {
        private Match _match;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			if (Arguments != null)
            {
                _match = JsonConvert.DeserializeObject<Match>(Arguments.GetString("@string/match"));
            }

			Activity.AddMenuProvider(this);

			return inflater.Inflate(Resource.Layout.sets, container, false);
        }

		public override void OnDestroyView()
		{
			Activity.RemoveMenuProvider(this);
			base.OnDestroyView();
		}

		public void OnCreateMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.sets_menu, menu);
        }

        public bool OnMenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_edit:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_match, null);

                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Update Match");
                    builder.SetView(view);
                    builder.SetPositiveButton("Update", UpdateMatchAction);
                    builder.SetNeutralButton("Delete", DeleteMatchAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    DateEditText = view.FindViewById<EditText>(Resource.Id.create_update_match_date);
                    DateEditText.Text = _match.Date;
                    DateEditText.Click += delegate
                    {
                        OnClickDateEditText();
                    };

                    view.FindViewById<TextView>(Resource.Id.create_update_match_id).Text = _match.Id.ToString();
                    view.FindViewById<EditText>(Resource.Id.create_update_match_description).Text = _match.Description;
                    view.FindViewById<EditText>(Resource.Id.create_update_match_player_one).Text = _match.PlayerOne;
                    view.FindViewById<EditText>(Resource.Id.create_update_match_player_two).Text = _match.PlayerTwo;

					builder.Show();
                    return true;
                }
                case Resource.Id.action_add:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_set, null);

                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Create Set");
                    builder.SetView(view);
                    builder.SetPositiveButton("Create", CreateSetAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    // Set the player labels.
                    view.FindViewById<TextView>(Resource.Id.create_update_set_player_one).Text = $"{_match.PlayerOne}:";
                    view.FindViewById<TextView>(Resource.Id.create_update_set_player_two).Text = $"{_match.PlayerTwo}:";

                    builder.Show();
                    return true;
                }
            }
            return false;
        }

        private void UpdateMatchAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            _match.Description = alertDialog.FindViewById<EditText>(Resource.Id.create_update_match_description).Text;
            _match.Date = alertDialog.FindViewById<EditText>(Resource.Id.create_update_match_date).Text;
            _match.PlayerOne = alertDialog.FindViewById<EditText>(Resource.Id.create_update_match_player_one).Text;
            _match.PlayerTwo = alertDialog.FindViewById<EditText>(Resource.Id.create_update_match_player_two).Text;

            DbHelper.UpdateMatch(_match);
            LoadData();
        }

        private void DeleteMatchAction(object sender, DialogClickEventArgs e)
        {
            DbHelper.DeleteMatch(_match.Id);

            string text = $"{_match.Description} has been deleted";
            Toast.MakeText(Activity.Application, text, ToastLength.Short).Show();

            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, new MatchesFragment(), "tennisMatchesFragment").Commit();
        }

        private void CreateSetAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string number = alertDialog.FindViewById<EditText>(Resource.Id.create_update_set_number).Text;
            string playerOneScore = alertDialog.FindViewById<EditText>(Resource.Id.create_update_set_player_one_score).Text;
            string playerTwoScore = alertDialog.FindViewById<EditText>(Resource.Id.create_update_set_player_two_score).Text;

            Set set = new Set()
            {
                MatchId = Convert.ToInt32(_match.Id),
                Number = Convert.ToInt32(number),
                PlayerOneScore = Convert.ToInt32(playerOneScore),
                PlayerTwoScore = Convert.ToInt32(playerTwoScore)
            };

            DbHelper.CreateSet(set);
            LoadData();
        }

        public void UpdateSetAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string id = alertDialog.FindViewById<TextView>(Resource.Id.create_update_set_id).Text;
            string number = alertDialog.FindViewById<EditText>(Resource.Id.create_update_set_number).Text;
            string playerOneScore = alertDialog.FindViewById<EditText>(Resource.Id.create_update_set_player_one_score).Text;
            string playerTwoScore = alertDialog.FindViewById<EditText>(Resource.Id.create_update_set_player_two_score).Text;

            Set set = new Set()
            {
                Id = Convert.ToInt32(id),
                Number = Convert.ToInt32(number),
                PlayerOneScore = Convert.ToInt32(playerOneScore),
                PlayerTwoScore = Convert.ToInt32(playerTwoScore)
            };

            DbHelper.UpdateSet(set);
            LoadData();
        }

        public void DeleteSetAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string id = alertDialog.FindViewById<TextView>(Resource.Id.create_update_set_id).Text;
            string number = alertDialog.FindViewById<TextView>(Resource.Id.create_update_set_number).Text;

            DbHelper.DeleteSet(Convert.ToInt32(id));
            LoadData();

            string text = $"#{number} has been deleted";
            Toast.MakeText(Activity.Application, text, ToastLength.Short).Show();
        }

        public override void LoadData()
        {
            Activity.Title = _match.Description;

            List<Set> sets = DbHelper.GetSets(_match.Id);

            string totalPlayerOne = sets.Sum(detail => Convert.ToInt32(detail.PlayerOneScore)).ToString();
            string totalPlayerTwo = sets.Sum(detail => Convert.ToInt32(detail.PlayerTwoScore)).ToString();

            Activity.FindViewById<TextView>(Resource.Id.player_one_total).Text = $"{_match.PlayerOne}: {totalPlayerOne}";
            Activity.FindViewById<TextView>(Resource.Id.player_two_total).Text = $"{_match.PlayerTwo}: {totalPlayerTwo}";

            SetsAdapter setsAdapter = new SetsAdapter(this, sets, _match);
            ListView.Adapter = setsAdapter;
        }
    }
}
