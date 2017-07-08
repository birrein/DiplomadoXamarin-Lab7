using Android.App;
using Android.Widget;
using Android.OS;

namespace Lab7
{
    [Activity(Label = "Lab7", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var ValidateButton = FindViewById<Button>(Resource.Id.ValidateButton);
            var ResultView = FindViewById<TextView>(Resource.Id.ResultView);
            var EmailText = FindViewById<EditText>(Resource.Id.EmailText);
            var PasswordText = FindViewById<EditText>(Resource.Id.PasswordText);
            string EMail;
            string Password;
            string Device = Android.Provider.Settings.Secure.GetString(
                ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            ValidateButton.Click += (sender, e) =>
            {
                EMail = EmailText.Text;
                Password = PasswordText.Text;
                Validate();

            };

            async void Validate()
            {
                string Result;

                var ServiceClient = new SALLab07.ServiceClient();
                var SvcResult = await ServiceClient.ValidateAsync(EMail, Password, Device);

                Result = $"{SvcResult.Status}\n{SvcResult.Fullname}\n{SvcResult.Token}";

                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    var Builder = new Notification.Builder(this)
                        .SetContentTitle("Validación de actividad")
                        .SetContentText(Result)
                        .SetSmallIcon(Resource.Drawable.Icon);
                    Builder.SetCategory(Notification.CategoryMessage);
                    var ObjectNotification = Builder.Build();
                    var Manager = GetSystemService(
                        Android.Content.Context.NotificationService) as NotificationManager;
                    Manager.Notify(0, ObjectNotification);
                }
                else
                {
                    ResultView.Text = Result;
                }
            }
        }
    }
}

