using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MySports.Adapters.MiniGolf;
using MySports.Models.MiniGolf;

namespace MySports.Fragments.MiniGolf
{
    public class RoundsFragment : SportFragment, IMenuProvider
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			Activity.AddMenuProvider(this);

			return inflater.Inflate(Resource.Layout.rounds, container, false);
        }

		public override void OnDestroyView()
		{
			Activity.RemoveMenuProvider(this);
			base.OnDestroyView();
		}

		public void OnCreateMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.rounds_menu, menu);
        }

        public bool OnMenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(this.Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_round, null);

                    DateEditText = view.FindViewById<EditText>(Resource.Id.create_update_round_date);
                    DateEditText.Text = DateTime.Now.ToShortDateString();
                    DateEditText.Click += delegate
                    {
                        OnClickDateEditText();
                    };

                    AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
                    builder.SetTitle("Create Round");
                    builder.SetView(view);
                    builder.SetPositiveButton("Create", CreateRoundAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

					builder.Show();
                    return true;
                }
            }
            return false;
        }

        private void CreateRoundAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string description = alertDialog.FindViewById<EditText>(Resource.Id.create_update_round_description).Text;
            string date = DateEditText.Text;
            string playerOne = alertDialog.FindViewById<EditText>(Resource.Id.create_update_round_player_one).Text;
            string playerTwo = alertDialog.FindViewById<EditText>(Resource.Id.create_update_round_player_two).Text;

            Round round = new Round()
            {
                Description = description,
                Date = date,
                PlayerOne = playerOne,
                PlayerTwo = playerTwo
            };

            DbHelper.CreateRound(round);
            LoadData();
        }

        public override void LoadData()
        {
            Activity.Title = Resources.GetString(Resource.String.title_mini_golf);

            List<Round> rounds = DbHelper.GetRounds();
            RoundsAdapter roundsAdapter = new RoundsAdapter(this, rounds);
            ListView.Adapter = roundsAdapter;
        }
    }
}
