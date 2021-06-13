using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AppFinalPatrones
{
    [Activity(Label = "Menu")]
    public class Menu : Activity
    {
        string Usuario;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);
            var btnCT = FindViewById<Button>(Resource.Id.btnConsultaTrabajadores);
            var btnMT = FindViewById<Button>(Resource.Id.btnMovimientoTrabajadores);
            var btnRT = FindViewById<Button>(Resource.Id.btnRegistroTrabajadores);
            Usuario = Intent.GetStringExtra("Usuario");
            btnCT.Click += delegate
            {
                CargarCT();
            };
            btnMT.Click += delegate
            {
                CargarMT();
            };
            btnRT.Click += delegate
            {
                CargarRT();
            };
        }
        public void CargarCT()
        {
            var VistaConsulta = new Intent(this, typeof(ConsultaTrabajadores));
            VistaConsulta.PutExtra("Usuario", Usuario);
            StartActivity(VistaConsulta);
        }
        public void CargarMT()
        {
            var VistaMovimientos = new Intent(this, typeof(ConsultaMovimientos));
            VistaMovimientos.PutExtra("Usuario", Usuario);
            StartActivity(VistaMovimientos);
        }
        public void CargarRT()
        {
            var VistaRegistro = new Intent(this, typeof(RegistroTrabajador));
            VistaRegistro.PutExtra("Usuario", Usuario);
            StartActivity(VistaRegistro);
        }
       
    }
}