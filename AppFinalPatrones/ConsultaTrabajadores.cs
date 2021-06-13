using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Linq;
using Android.Content;
using System.Threading.Tasks;
using Android.Graphics;

namespace AppFinalPatrones
{
    [Activity(Label = "ConsultaTrabajadores")]
    public class ConsultaTrabajadores : AppCompatActivity
    {
        Android.App.ProgressDialog progress;
        string elementoimagen;
        ListView listado;
        List<Trabajador> ListadeTrabajadores = new List<Trabajador>();
        List<ElementosdelaTabla> ElementosTabla = new List<ElementosdelaTabla>();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.ListaTrabajadores);
            listado = FindViewById<ListView>(Resource.Id.ListaTrabajadores);
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Cargando datos de Azure...");
            progress.SetCancelable(false);
            progress.Show();
            await CargarDatosAzure();
            progress.Hide();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public async Task CargarDatosAzure()
        {
            try
            {
                var titulo = FindViewById<TextView>(Resource.Id.textView3);
                var typeface = Typeface.CreateFromAsset(this.Assets, "fonts/BebasNeue.ttf");
                titulo.SetTypeface(typeface, TypefaceStyle.Normal);
                var CuentadeAlmacenamiento = CloudStorageAccount.Parse
                    ("DefaultEndpointsProtocol=https;AccountName=programacionmovil;AccountKey=RvGLvIagYrqkqSzVy5pvVwLBR2+G+Bf/B2UqoZeFNrZXPwAhuUPVGAOhC33JTTZZOJlmSJqMALB6tAXw//J1Tg==;EndpointSuffix=core.windows.net");
                var ClienteBlob = CuentadeAlmacenamiento.CreateCloudBlobClient();
                var Contenedor = ClienteBlob.GetContainerReference("trabajadores");
                var TablaNoSQL = CuentadeAlmacenamiento.CreateCloudTableClient();
                var Tabla = TablaNoSQL.GetTableReference("Trabajadores");
                var Consulta = new TableQuery<Trabajador>();
                TableContinuationToken token = null;
                var Datos = await Tabla.ExecuteQuerySegmentedAsync<Trabajador>
                    (Consulta, token, null, null);
                ListadeTrabajadores.AddRange(Datos.Results);
                int iNombre = 0;
                int iRFC = 0;
                int iCURP = 0;
                int iNumSocial = 0;
                int iTipoTrabajo = 0;
                int iJornada = 0;
                int iSalario = 0;
                int iCP = 0;
                int iOcupacion = 0;
                int iUMF = 0;
                int iSexo = 0;
                int iFNac = 0;
                int iLNac = 0;
                int iClaveUbi = 0;
                int iHoras = 0;
                int iPension = 0;
                int iTipoSalario = 0;
                int iImagen = 0;
                ElementosTabla = ListadeTrabajadores.Select(r => new ElementosdelaTabla()
                {
                    Nombre = ListadeTrabajadores.ElementAt(iNombre++).RowKey,
                    RFC = ListadeTrabajadores.ElementAt(iRFC++).RFC,
                    CURP = ListadeTrabajadores.ElementAt(iCURP++).CURP,
                    NumSocial = ListadeTrabajadores.ElementAt(iNumSocial++).NumeroSocial,
                    TipoTrabajo = ListadeTrabajadores.ElementAt(iTipoTrabajo++).TipoTrabajador,
                    Jornada = ListadeTrabajadores.ElementAt(iJornada++).Jornada,
                    Salario = ListadeTrabajadores.ElementAt(iSalario++).Salario,
                    CP = ListadeTrabajadores.ElementAt(iCP++).CP,
                    Ocupacion = ListadeTrabajadores.ElementAt(iOcupacion++).Ocupacion,
                    UMF = ListadeTrabajadores.ElementAt(iUMF++).UMF,
                    Sexo = ListadeTrabajadores.ElementAt(iSexo++).Sexo,
                    FNac = ListadeTrabajadores.ElementAt(iFNac++).FechaNac,
                    LNac = ListadeTrabajadores.ElementAt(iLNac++).LugarNac,
                    ClaveUbicacion = ListadeTrabajadores.ElementAt(iClaveUbi++).ClaveUbicacion,
                    Horas = ListadeTrabajadores.ElementAt(iHoras++).Horas,
                    Pension = ListadeTrabajadores.ElementAt(iPension++).Pension,
                    TipoSalario = ListadeTrabajadores.ElementAt(iTipoSalario++).TipoSalario,
                    imagen = ListadeTrabajadores.ElementAt(iImagen++).Imagen

                }).ToList();
                int contadorimagen = 0;
                try
                {
                    while (contadorimagen < ListadeTrabajadores.Count)
                    {
                        elementoimagen = ListadeTrabajadores.ElementAt(contadorimagen).Imagen;
                        var ImagenBlob = Contenedor.GetBlockBlobReference(elementoimagen);
                        var rutaimagen = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                        var ArchivoImagen = System.IO.Path.Combine(rutaimagen, elementoimagen);
                        var StreamImagen = File.OpenWrite(ArchivoImagen);
                        await ImagenBlob.DownloadToStreamAsync(StreamImagen);
                        contadorimagen++;
                    }
                    Toast.MakeText(this, "Datos descargados", ToastLength.Long).Show();
                }
                catch(System.Exception ex)
                {

                }

                listado.Adapter = new DataAdapter(this, ElementosTabla);
                listado.ItemClick += OnListItemClick;
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var DataSend = ElementosTabla[e.Position];
            var DataIntent = new Intent(this, typeof(DetallesTrabajadorConsulta));
            DataIntent.PutExtra("nombre", DataSend.Nombre);
            DataIntent.PutExtra("RFC", DataSend.RFC);
            DataIntent.PutExtra("CURP", DataSend.CURP);
            DataIntent.PutExtra("salario", DataSend.Salario);
            DataIntent.PutExtra("tiposalario", DataSend.TipoSalario);
            DataIntent.PutExtra("tipotrabajo", DataSend.TipoTrabajo);
            DataIntent.PutExtra("numsocial", DataSend.NumSocial);
            DataIntent.PutExtra("jornada", DataSend.Jornada);
            DataIntent.PutExtra("CP", DataSend.CP);
            DataIntent.PutExtra("ocupacion", DataSend.Ocupacion);
            DataIntent.PutExtra("UMF", DataSend.UMF);
            DataIntent.PutExtra("sexo", DataSend.Sexo);
            DataIntent.PutExtra("fnac", DataSend.FNac);
            DataIntent.PutExtra("lnac", DataSend.LNac);
            DataIntent.PutExtra("claveubi", DataSend.ClaveUbicacion);
            DataIntent.PutExtra("horas", DataSend.Horas);
            DataIntent.PutExtra("pension", DataSend.Pension);
            DataIntent.PutExtra("imagen", DataSend.imagen);
            StartActivity(DataIntent);
        }
        public class ElementosdelaTabla
        {
            public string Nombre { get; set; }
            public string RFC { get; set; }
            public string CURP { get; set; }
            public string NumSocial { get; set; }
            public string TipoTrabajo { get; set; }
            public string Jornada { get; set; }
            public double Salario { get; set; }
            public string CP { get; set; }
            public string Ocupacion { get; set; }
            public int UMF { get; set; }
            public string Sexo { get; set; }
            public string FNac { get; set; }
            public string LNac { get; set; }
            public int ClaveUbicacion { get; set; }
            public int Horas { get; set; }
            public string Pension { get; set; }
            public string TipoSalario { get; set; }
            public string imagen { get; set; }

        }
        public class Trabajador : TableEntity
        {
            public Trabajador(string Categoria, string Nombre)
            {
                PartitionKey = Categoria;
                RowKey = Nombre;
            }
            public Trabajador() { }
            public string RFC { get; set; }
            public string CURP { get; set; }
            public int ClaveUbicacion { get; set; }
            public string FechaNac { get; set; }
            public int Horas { get; set; }
            public string Imagen { get; set; }
            public string Jornada { get; set; }
            public string LugarNac { get; set; }
            public string NumeroSocial { get; set; }
            public string Ocupacion { get; set; }
            public string Pension { get; set; }
            public string TipoTrabajador { get; set; }
            public double Salario { get; set; }
            public string CP { get; set; }
            public int UMF { get; set; }
            public string Sexo { get; set; }
            public string TipoSalario { get; set; }
        }
        public class Movimientos : TableEntity
        {
            public Movimientos(string Categoria, string ID)
            {
                PartitionKey = Categoria;
                RowKey = ID;
            }
            public Movimientos() { }
            public string RFC { get; set; }
            public string FECHAINICIO { get; set; }
            public double SDI { get; set; }
            public double VALOR { get; set; }
            public int ESTATUS { get; set; }
            public int DIAS { get; set; }
            public string FOLIO { get; set; }
            public string RAMAINICIO { get; set; }
            public string TIPORGO { get; set; }
            public string SECUELA { get; set; }
            public string CRTLINCAP { get; set; }
            public string FECHATERM { get; set; }
            public double PORCINCAP { get; set; }
            public double CREDITO { get; set; }
            public string TIPO { get; set; }
            public string Movimiento { get; set; }

        }
    }
}