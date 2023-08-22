using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MyWorkoutAndroid.Adapters;
using MyWorkoutAndroid.Models;

namespace MyWorkoutAndroid.Fragments
{
    public class ProgramsFragment : SportFragment, IMenuProvider
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Activity.AddMenuProvider(this);

            return inflater.Inflate(Resource.Layout.programs, container, false);
        }

        public override void OnDestroyView()
        {
            Activity.RemoveMenuProvider(this);
            base.OnDestroyView();
        }

        public void OnCreateMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.programs_menu, menu);
        }

        public bool OnMenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                    {
                        LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                        View view = layoutInflater.Inflate(Resource.Layout.create_update_program, null);

                        AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle("Create Program");
                        builder.SetView(view);
                        builder.SetPositiveButton("Create", CreateProgramAction);
                        builder.SetNegativeButton("Cancel", CancelAction);

                        builder.Show();
                        return true;
                    }
            }

            return false;
        }

        private void CreateProgramAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string name = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_name).Text;
            string durationInWeeks = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_duration_in_weeks).Text;
            string frequencyPerWeek = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_frequency_per_week).Text;

            Program program = new Program()
            {
                Name = name,
                DurationInWeeks = Convert.ToInt32(durationInWeeks),
                FrequencyPerWeek = Convert.ToInt32(frequencyPerWeek),
            };

            DbHelper.CreateProgram(program);
            LoadData();
        }

        public override void LoadData()
        {
            Activity.Title = Resources.GetString(Resource.String.title_gym);

            List<Program> programs = DbHelper.GetPrograms();
            ProgramsAdapter programsAdapter = new ProgramsAdapter(this, programs, DbHelper);
            ListView.Adapter = programsAdapter;
        }
    }
}
