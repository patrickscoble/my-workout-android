using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MyWorkoutAndroid.Helpers;

using static Android.App.DatePickerDialog;
using ListFragment = AndroidX.Fragment.App.ListFragment;

namespace MyWorkoutAndroid
{
    public abstract class SportFragment : ListFragment, IOnDateSetListener
    {
        protected EditText DateEditText;
        protected DbHelper DbHelper;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;

            this.DbHelper = new DbHelper(Activity);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            LoadData();
        }

        public override void OnResume()
        {
            base.OnResume();
            LoadData();
        }

		protected void OnClickDateEditText()
        {
            DateTime dateTimeNow = DateTime.Now;
            Android.App.DatePickerDialog datePicker = new Android.App.DatePickerDialog(Activity, this, dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day);
            datePicker.UpdateDate(dateTimeNow);
            datePicker.Show();
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            DateEditText.Text = new DateTime(year, month + 1, dayOfMonth).ToShortDateString();
        }

        public void CancelAction(object sender, DialogClickEventArgs e)
        {
        }

        public virtual void LoadData()
        {
        }
    }
}
