using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat;
using Android.Widget;
using Android.Content;
using Android.Support.V4;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Graphics;

namespace AppFinalPatrones
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        string Usuario, Password;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            SupportActionBar.Hide();
            var txtRFC = FindViewById<EditText>(Resource.Id.txtRFCPatron);
            var txtPass = FindViewById<EditText>(Resource.Id.txtPasswordPatron);
            var btnLogIn = FindViewById<Button>(Resource.Id.btnIngresar);

            btnLogIn.Click += delegate{
                try
                {
                    Usuario = txtRFC.Text;
                    Password = txtPass.Text;
                    if (Usuario == "ABC123")
                    {
                        if (Password == "123456")
                        {
                            Cargar();
                        }
                        else
                        {
                            Toast.MakeText(this, "Error en el password", ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "Error en el usuario", ToastLength.Long).Show();
                    }
                }
                catch (System.Exception EX)
                {
                    Toast.MakeText(this, EX.Message, ToastLength.Long).Show();
                }
            };
        }

      
        public void Cargar()
        {
            var VistaMenu = new Intent(this, typeof(Menu));
            VistaMenu.PutExtra("Usuario", Usuario);
            StartActivity(VistaMenu);
        }
        
    }
}