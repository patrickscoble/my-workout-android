using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MySports.Adapters.Tennis;
using MySports.Models.Tennis;

namespace MySports.Fragments.Tennis
{
    public class MatchesFragment : SportFragment, IMenuProvider
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			Activity.AddMenuProvider(this);

			return inflater.Inflate(Resource.Layout.matches, container, false);
        }

		public override void OnDestroyView()
		{
			Activity.RemoveMenuProvider(this);
			base.OnDestroyView();
		}

		public void OnCreateMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.matches_menu, menu);
        }

        public bool OnMenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(this.Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_match, null);

                    DateEditText = view.FindViewById<EditText>(Resource.Id.create_update_match_date);
                    DateEditText.Text = DateTime.Now.ToShortDateString();
                    DateEditText.Click += delegate
                    {
                        OnClickDateEditText();
                    };

                    AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
                    builder.SetTitle("Create Match");
                    builder.SetView(view);
                    builder.SetPositiveButton("Create", CreateMatchAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    builder.Show();
                    return true;
                }
            }
            return false;
        }

        private void CreateMatchAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string description = alertDialog.FindViewById<EditText>(Resource.Id.create_update_match_description).Text;
            string date = DateEditText.Text;
            string playerOne = alertDialog.FindViewById<EditText>(Resource.Id.create_update_match_player_one).Text;
            string playerTwo = alertDialog.FindViewById<EditText>(Resource.Id.create_update_match_player_two).Text;

            Match match = new Match()
            {
                Description = description,
                Date = date,
                PlayerOne = playerOne,
                PlayerTwo = playerTwo
            };

            DbHelper.CreateMatch(match);
            LoadData();
        }

        public override void LoadData()
        {
            Activity.Title = Resources.GetString(Resource.String.title_tennis);

            List<Match> matches = DbHelper.GetMatches();
            MatchesAdapter matchesAdapter = new MatchesAdapter(this, matches);
            ListView.Adapter = matchesAdapter;
        }
    }
}
