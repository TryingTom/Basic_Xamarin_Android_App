/*
    Using both Labels and resource Strings on task 20... 
 */

using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Task_3_Finished_but_better
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        // initialize view elements
        TextView SummaryTextView;
        TextView CarAgeTextView;
        TextView FourWheelerTextView;
        TextView TypeTextView;
        EditText NameEditText;
        EditText CarAgeEditText;
        CheckBox OwnsCarCheckBox;
        bool OwnsCar = false;
        bool AdvancedOptions = false;
        bool FourWheeler = false;
        string Type = Labels.lStationWagon;
        Button SaveButton;
        ToggleButton FourWheelerToggleButton;
        RadioButton StationWagonRadioButton;
        RadioButton SedanRadioButton;
        RadioButton SportRadioButton;

        //-------------------------------------------------------------------- OnCreate Start --------------------------------------------------------------------//
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // basic settings
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // set up toolbar and menu items
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // find views by id and initialize buttons
            SummaryTextView = FindViewById<TextView>(Resource.Id.twSummary);
            CarAgeTextView = FindViewById<TextView>(Resource.Id.twAge);
            FourWheelerTextView = FindViewById<TextView>(Resource.Id.twFourWheeler);
            TypeTextView = FindViewById<TextView>(Resource.Id.twType);

            NameEditText = FindViewById<EditText>(Resource.Id.etName);
            CarAgeEditText = FindViewById<EditText>(Resource.Id.etAge);
                // set a filter that only ages of 1-10 are accepted
            CarAgeEditText.SetFilters(new Android.Text.IInputFilter[] { new MinMaxInputFilter(1,10)});

            OwnsCarCheckBox = FindViewById<CheckBox>(Resource.Id.cbOwnCar);
            OwnsCarCheckBox.CheckedChange += OwnsCarCheckBox_CheckedChange;

            FourWheelerToggleButton = FindViewById<ToggleButton>(Resource.Id.tbFourWheeler);
            FourWheelerToggleButton.CheckedChange += FourWheelerToggleButton_CheckedChange;

            SaveButton = FindViewById<Button>(Resource.Id.btnSave);
            SaveButton.Click += SaveButton_Click;

            StationWagonRadioButton = FindViewById<RadioButton>(Resource.Id.rbStationWagon);
            SedanRadioButton = FindViewById<RadioButton>(Resource.Id.rbSedan);
            SportRadioButton = FindViewById<RadioButton>(Resource.Id.rbSport);

            // set context for Resource.string elements
            Context context = this;
            // Get the Resources object from our context
            Android.Content.Res.Resources res = context.Resources;

            StationWagonRadioButton.Click += delegate
            {
                Type = res.GetString(Resource.String.RadioStationWagon);
                SedanRadioButton.Checked = false;
                SportRadioButton.Checked = false;
            };

            SedanRadioButton.Click += delegate
            {
                Type = res.GetString(Resource.String.RadioSedan);
                StationWagonRadioButton.Checked = false;
                SportRadioButton.Checked = false;
            };

            SportRadioButton.Click += delegate
            {
                Type = res.GetString(Resource.String.RadioSport);
                StationWagonRadioButton.Checked = false;
                SedanRadioButton.Checked = false;
            };
        } // on create end
        //-------------------------------------------------------------------- OnCreate End   --------------------------------------------------------------------//

        //-------------------------------------------------------------------- Button Functions Start ------------------------------------------------------------//
        private void FourWheelerToggleButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            // set the Fourwheeler to the check status
            FourWheeler = e.IsChecked;
        }

        private void OwnsCarCheckBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            // if it's checked, set age question visible - otherwise hide it
            if (e.IsChecked)
            {
                CarAgeTextView.Visibility = Android.Views.ViewStates.Visible;
                CarAgeEditText.Visibility = Android.Views.ViewStates.Visible;
            }
            else
            {
                CarAgeTextView.Visibility = Android.Views.ViewStates.Invisible;
                CarAgeEditText.Visibility = Android.Views.ViewStates.Invisible;
            }
            // set boolean value to be true or false
            OwnsCar = e.IsChecked;
        } // OwnsCarCheckBox_CheckedChange end

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // build the summary with StringBuilder
            if (NameEditText.Text.Length == 0)
            {
                Toast.MakeText(this, Labels.lNameError, ToastLength.Long).Show();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(Labels.lAskName);
                sb.AppendFormat(NameEditText.Text);
                sb.AppendFormat("\n");
                
                // if the person owns a car
                if (OwnsCar) {
                    if (CarAgeEditText.Text.Length == 0) {Toast.MakeText(this, Labels.lAgeError, ToastLength.Long).Show();}
                    else{
                        sb.AppendFormat(Labels.lAskAge);
                        sb.AppendFormat(CarAgeEditText.Text);
                        sb.AppendFormat("\n");
                        SummaryTextView.Text = sb.ToString();

                        // if the user has selected advanced options
                        if (AdvancedOptions)
                        {
                            // show if user has four wheeler
                            sb.AppendFormat(Labels.lFourWheelers);
                            sb.AppendFormat(FourWheeler.ToString());
                            sb.AppendFormat("\n");

                            // show user's car type
                            sb.AppendFormat(Labels.lCarType);
                            sb.AppendFormat(Type);
                            sb.AppendFormat("\n");
                            SummaryTextView.Text = sb.ToString();
                        }
                    } 
                }
                else { 
                    // if user doesn't have a car, set text to indicate so
                    sb.AppendFormat(Labels.lNoCar);
                    sb.AppendFormat("\n");
                    SummaryTextView.Text = sb.ToString();
                }
            }
        } // SaveButton_Click end
        //-------------------------------------------------------------------- Button Functions End   ------------------------------------------------------------//

        //-------------------------------------------------------------------- Menu Functions Start   ------------------------------------------------------------//
        
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // set the menu layout on Main Activity  
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        } // OnCreateOptionsMenu end

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.AdvancedOptions:
                    {
                        // toggle the advanced options
                        AdvancedOptions = !AdvancedOptions;
                        // set Advanced options visibility accordingly
                        ShowAdvancedOptions(AdvancedOptions);
                        return true;
                    }
            }

            return base.OnOptionsItemSelected(item);
        } // OnOptionsItemSelected end

        private void ShowAdvancedOptions(bool advancedOptions)
            {
                // if advanced options are selected, show the advanced menu
                if (advancedOptions)
                {
                    FourWheelerTextView.Visibility = Android.Views.ViewStates.Visible;
                    FourWheelerToggleButton.Visibility = Android.Views.ViewStates.Visible;
                    TypeTextView.Visibility = Android.Views.ViewStates.Visible;
                    StationWagonRadioButton.Visibility = Android.Views.ViewStates.Visible;
                    SedanRadioButton.Visibility = Android.Views.ViewStates.Visible;
                    SportRadioButton.Visibility = Android.Views.ViewStates.Visible;
                }
                // else hide them
                else
                {
                    FourWheelerTextView.Visibility = Android.Views.ViewStates.Invisible;
                    FourWheelerToggleButton.Visibility = Android.Views.ViewStates.Invisible;
                    TypeTextView.Visibility = Android.Views.ViewStates.Invisible;
                    StationWagonRadioButton.Visibility = Android.Views.ViewStates.Invisible;
                    SedanRadioButton.Visibility = Android.Views.ViewStates.Invisible;
                    SportRadioButton.Visibility = Android.Views.ViewStates.Invisible;
                }
            } // ShowItems end
        //-------------------------------------------------------------------- Menu Functions End     ------------------------------------------------------------//
    } // MainActivity end
} // NameSpace end
