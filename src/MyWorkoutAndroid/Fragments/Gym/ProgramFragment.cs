using System.Linq;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MyWorkoutAndroid.Models.Gym;
using Newtonsoft.Json;

namespace MyWorkoutAndroid.Fragments.Gym
{
    public class ProgramFragment : SportFragment, IMenuProvider
    {
        internal Program Program;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (Arguments != null)
            {
                Program = JsonConvert.DeserializeObject<Program>(Arguments.GetString("@string/program"));
            }

			Activity.AddMenuProvider(this);

			return inflater.Inflate(Resource.Layout.program, container, false);
        }

		public override void OnDestroyView()
		{
			Activity.RemoveMenuProvider(this);
			base.OnDestroyView();
		}

		public void OnCreateMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.program_menu, menu);
        }

        public bool OnMenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_copy:
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Copy Program");
                    builder.SetPositiveButton("Copy", CopyProgramAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    builder.Show();
                    return true;
                }
                case Resource.Id.action_edit:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_program, null);

                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Update Program");
                    builder.SetView(view);
                    builder.SetPositiveButton("Update", UpdateProgramAction);
                    builder.SetNeutralButton("Delete", DeleteProgramAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    view.FindViewById<TextView>(Resource.Id.create_update_program_id).Text = Program.Id.ToString();
                    view.FindViewById<EditText>(Resource.Id.create_update_program_name).Text = Program.Name;

                    builder.Show();
                    return true;
                }
            }
            return false;
        }

        private void CopyProgramAction(object sender, DialogClickEventArgs e)
        {
            DbHelper.CopyProgram(Program);
            LoadData();
        }

        private void UpdateProgramAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            Program.Name = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_name).Text;

            DbHelper.UpdateProgram(Program);
            LoadData();
        }

        private void DeleteProgramAction(object sender, DialogClickEventArgs e)
        {
            DbHelper.DeleteProgram(Program.Id);

            string text = $"{Program.Name} has been deleted";
            Toast.MakeText(Activity.Application, text, ToastLength.Short).Show();

            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, new ProgramsFragment(), "programsFragment").Commit();
        }

        public override void LoadData()
        {
            Activity.Title = Program.Name;

            View divider = View.FindViewById<View>(Resource.Id.program_exercises_workouts_divider);
            LinearLayout workouts = View.FindViewById<LinearLayout>(Resource.Id.workouts_fragment);

            bool hasProgramExercises = DbHelper.GetProgramExercises(Program.Id).Any();

            divider.Visibility = hasProgramExercises ? ViewStates.Visible : ViewStates.Gone;
            workouts.Visibility = hasProgramExercises ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}
