using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using AndroidX.Core.Graphics.Drawable;

namespace AppFinalPatrones
{
    [Activity(Label = "DetallesTrabajadorConsulta")]
    public class DetallesTrabajadorConsulta : Activity, IOnMapReadyCallback
    {
        string nombre, RFC, CURP, tipoSalario,tipotrabajo, numSocial, jornada, CP, Ocupacion, sexo, FNac, LNac, pension, imagen;
        double salario, lat, lon;
        int horas, UMF, claveUbi;
        TextView txtNombre, txtRFC, txtCURP, txttipoSalario, txttipotrabajo, txtnumSocial, txtjornada, txtCP, txtOcupacion, txtUMF, txtsexo, txtFNac, txtLNac, txtclaveUbi, txtpension, txtsalario, txthoras;
        GoogleMap googleMap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ConsultaTrabajador);
            try
            {
                nombre = Intent.GetStringExtra("nombre");
                RFC = Intent.GetStringExtra("RFC");
                CURP = Intent.GetStringExtra("CURP");
                salario = Intent.Extras.GetDouble("salario");
                tipoSalario = Intent.GetStringExtra("tiposalario");
                tipotrabajo = Intent.GetStringExtra("tipotrabajo");
                numSocial = Intent.GetStringExtra("numsocial");
                jornada = Intent.GetStringExtra("jornada");
                CP = Intent.GetStringExtra("CP");
                Ocupacion = Intent.GetStringExtra("ocupacion");
                UMF = Intent.Extras.GetInt("UMF");
                sexo = Intent.GetStringExtra("sexo");
                FNac = Intent.GetStringExtra("fnac");
                LNac = Intent.GetStringExtra("lnac");
                claveUbi = Intent.Extras.GetInt("claveubi");
                horas = Intent.Extras.GetInt("horas");
                pension = Intent.GetStringExtra("pension");
                imagen = Intent.GetStringExtra("imagen");

                txtNombre = FindViewById<TextView>(Resource.Id.txtName); 
                txtRFC = FindViewById<TextView>(Resource.Id.txtRFC); 
                txtCURP = FindViewById<TextView>(Resource.Id.txtCURP);
                var Imagen = FindViewById<ImageView>(Resource.Id.image);
                txttipoSalario = FindViewById<TextView>(Resource.Id.txtTipoSalario); 
                txttipotrabajo = FindViewById<TextView>(Resource.Id.txtTipoTrabajador); 
                txtnumSocial = FindViewById<TextView>(Resource.Id.txtNumeroSocial); 
                txtjornada = FindViewById<TextView>(Resource.Id.txtJornada);
                txtCP = FindViewById<TextView>(Resource.Id.txtCP);
                txtOcupacion = FindViewById<TextView>(Resource.Id.txtOcupacion);
                txtUMF = FindViewById<TextView>(Resource.Id.txtUMF);
                txtsexo = FindViewById<TextView>(Resource.Id.txtSexo);
                txtFNac = FindViewById<TextView>(Resource.Id.txtFecNac);
                txtLNac = FindViewById<TextView>(Resource.Id.txtLNac);
                txtclaveUbi = FindViewById<TextView>(Resource.Id.txtClaveUbi);
                txtpension = FindViewById<TextView>(Resource.Id.txtPension);
                txtsalario = FindViewById<TextView>(Resource.Id.txtSalario);
                txthoras = FindViewById<TextView>(Resource.Id.txtHoras);

                txtNombre.Text = nombre;
                txtRFC.Text = RFC;
                txtCURP.Text = CURP; 
                txttipoSalario.Text = tipoSalario;
                txttipotrabajo.Text = tipotrabajo;
                txtnumSocial.Text = numSocial;
                txtjornada.Text = jornada;
                txtCP.Text = CP;
                txtOcupacion.Text = Ocupacion;
                txtUMF.Text = UMF.ToString();
                txtsexo.Text = sexo;
                txtFNac.Text = FNac;
                txtLNac.Text = LNac;
                txtclaveUbi.Text = claveUbi.ToString();
                txtpension.Text = pension;
                txtsalario.Text = salario.ToString();
                txthoras.Text = horas.ToString();

                var typeface = Typeface.CreateFromAsset(this.Assets, "fonts/BebasNeue.ttf");
                txtNombre.SetTypeface(typeface, TypefaceStyle.Normal);
                var RutaImagen = System.IO.Path.Combine(System.Environment.GetFolderPath
                    (System.Environment.SpecialFolder.Personal), imagen);
                var rutaimagen = Android.Net.Uri.Parse(RutaImagen);
                Imagen.SetImageURI(rutaimagen);

                var opciones = new BitmapFactory.Options();
                opciones.InPreferredConfig = Bitmap.Config.Argb8888;
                var bitmap = BitmapFactory.DecodeFile(RutaImagen, opciones);
                Imagen.SetImageDrawable(getRoundedCornerImagen(bitmap, 20));
                lat = 21.5648;
                lon = -101.2564;
                var mapView = FindViewById<MapView>(Resource.Id.map);
                mapView.OnCreate(savedInstanceState);
                mapView.GetMapAsync(this);
                MapsInitializer.Initialize(this);
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }
        public void OnMapReady(GoogleMap googleMap)
        {
            this.googleMap = googleMap;
            var builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lon));
            builder.Zoom(17);
            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            this.googleMap.AnimateCamera(cameraUpdate);
        }

        public static RoundedBitmapDrawable getRoundedCornerImagen(Bitmap image, int cornerRadius)
        {
            var corner = RoundedBitmapDrawableFactory.Create(null, image);
            corner.CornerRadius = cornerRadius;
            return corner;
        }
    }
}