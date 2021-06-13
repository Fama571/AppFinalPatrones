using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using Android.Graphics;
using Android.Widget;
using Plugin.Media;
using System;
using System.IO;
using Plugin.CurrentActivity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Android.Runtime;
using System.Threading.Tasks;

namespace AppFinalPatrones
{
    [Activity(Label = "RegistroTrabajador")]
    public class RegistroTrabajador : Activity
    {
        string Nombre, RFC, CURP, NumSocial, TipoTrabajo, Jornada, CP, Ocupacion, Sexo, FNac, LNac, Pension, TipoSalario, Archivo;
        double Salario;
        int UMF, ClaveUbi, Horas, contarMov;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Registro);

            await CargarDatos();

            var txtNombre = FindViewById<EditText>(Resource.Id.txtName);
            var txtRFC = FindViewById<EditText>(Resource.Id.txtRFC);
            var txtCURP = FindViewById<EditText>(Resource.Id.txtCURP);
            var txtNS = FindViewById<EditText>(Resource.Id.txtNumeroSocial);
            var txtTipoT = FindViewById<EditText>(Resource.Id.txtTipoTrabajador);
            var txtJornada = FindViewById<EditText>(Resource.Id.txtJornada);
            var txtSalario = FindViewById<EditText>(Resource.Id.txtSalario);
            var txtCP = FindViewById<EditText>(Resource.Id.txtCP);
            var txtOcupacion = FindViewById<EditText>(Resource.Id.txtOcupacion);
            var txtUMF = FindViewById<EditText>(Resource.Id.txtUMF);
            var txtSexo = FindViewById<EditText>(Resource.Id.txtSexo);
            var txtFNac = FindViewById<EditText>(Resource.Id.txtFNac);
            var txtLNac = FindViewById<EditText>(Resource.Id.txtLNac);
            var txtClaveUbi = FindViewById<EditText>(Resource.Id.txtClaveUbi);
            var txtHoras = FindViewById<EditText>(Resource.Id.txtHoras);
            var txtPension = FindViewById<EditText>(Resource.Id.txtPension);
            var txtTipoS = FindViewById<EditText>(Resource.Id.txtTipoSalario);
            var btnRegistrar = FindViewById<Button>(Resource.Id.btnRegistrar);
            var Imagen = FindViewById<ImageView>(Resource.Id.ImagenFoto);

            Imagen.Click += async delegate
            {
                await CrossMedia.Current.Initialize();
                var archivo = await CrossMedia.Current.TakePhotoAsync
                (new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Imagenes",
                    Name = txtRFC.Text,
                    SaveToAlbum = true,
                    CompressionQuality = 30,
                    CustomPhotoSize = 30,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear
                });
                if (archivo == null)
                    return;
                Bitmap bp = BitmapFactory.DecodeStream(archivo.GetStream());
                Archivo = System.IO.Path.Combine(System.IO.Path.Combine
                    (System.Environment.GetFolderPath(
                        System.Environment.SpecialFolder.Personal), txtRFC.Text + ".jpg"));
                var stream = new FileStream(Archivo, FileMode.Create);
                bp.Compress(Bitmap.CompressFormat.Png, 30, stream);
                stream.Close();
                Imagen.SetImageBitmap(bp);
                long memoria1 = GC.GetTotalMemory(false);
                Toast.MakeText(this.ApplicationContext, memoria1.ToString(), ToastLength.Long).Show();
                GC.Collect();
                long memoria2 = GC.GetTotalMemory(false);
                Toast.MakeText(this.ApplicationContext, memoria2.ToString(), ToastLength.Long).Show();
            };

            btnRegistrar.Click += async delegate
            {
                try
                {
                    Nombre = txtNombre.Text;
                    RFC = txtRFC.Text;
                    CURP = txtCURP.Text;
                    NumSocial = txtNS.Text;
                    TipoTrabajo = txtTipoT.Text;
                    Jornada = txtJornada.Text;
                    Salario = double.Parse(txtSalario.Text);
                    CP = txtCP.Text;
                    Ocupacion = txtOcupacion.Text;
                    UMF = int.Parse(txtUMF.Text);
                    Sexo = txtSexo.Text;
                    FNac = txtFNac.Text;
                    LNac = txtLNac.Text;
                    ClaveUbi = int.Parse(txtClaveUbi.Text);
                    Horas = int.Parse(txtHoras.Text);
                    Pension = txtPension.Text;
                    TipoSalario = txtTipoS.Text;

                    var CuentadeAlmacenamiento = CloudStorageAccount.Parse
                    ("DefaultEndpointsProtocol=https;AccountName=programacionmovil;AccountKey=RvGLvIagYrqkqSzVy5pvVwLBR2+G+Bf/B2UqoZeFNrZXPwAhuUPVGAOhC33JTTZZOJlmSJqMALB6tAXw//J1Tg==;EndpointSuffix=core.windows.net");
                    var ClienteBlob = CuentadeAlmacenamiento.CreateCloudBlobClient();
                    var Carpeta = ClienteBlob.GetContainerReference("trabajadores");
                    var resourceBlob = Carpeta.GetBlockBlobReference(txtRFC.Text + ".jpg");
                    await resourceBlob.UploadFromFileAsync(Archivo.ToString());
                    Toast.MakeText(this, "Imagen Guardada correctamente", ToastLength.Long).Show();

                    var TablaNoSQL = CuentadeAlmacenamiento.CreateCloudTableClient();
                    var Tabla = TablaNoSQL.GetTableReference("Trabajadores");
                    await Tabla.CreateIfNotExistsAsync();
                    var Trabajador = new Registrar("Trabajador", txtNombre.Text);
                    Trabajador.RFC = txtRFC.Text;
                    Trabajador.CURP = txtCURP.Text;
                    Trabajador.ClaveUbicacion = int.Parse(txtClaveUbi.Text);
                    Trabajador.FechaNac = txtFNac.Text;
                    Trabajador.Horas = int.Parse(txtHoras.Text);
                    Trabajador.Imagen = txtRFC.Text + ".jpg";
                    Trabajador.Jornada = txtJornada.Text;
                    Trabajador.LugarNac = txtLNac.Text;
                    Trabajador.NumeroSocial = txtNS.Text;
                    Trabajador.Ocupacion = txtOcupacion.Text;
                    Trabajador.Pension = txtPension.Text;
                    Trabajador.TipoSalario = txtTipoS.Text;
                    Trabajador.Salario = double.Parse(txtSalario.Text);
                    Trabajador.CP = txtCP.Text;
                    Trabajador.UMF = int.Parse(txtUMF.Text);
                    Trabajador.Sexo = txtSexo.Text;
                    Trabajador.TipoTrabajador = txtTipoT.Text;

                    var Store = TableOperation.Insert(Trabajador);
                    await Tabla.ExecuteAsync(Store);
                    Toast.MakeText(this, "Datos Guardados", ToastLength.Long).Show();

                    string Date = DateTime.Today.ToString("yyyy/MM/dd");
                    var TablaMov = CuentadeAlmacenamiento.CreateCloudTableClient();
                    var TMov = TablaNoSQL.GetTableReference("Movimientos");
                    await Tabla.CreateIfNotExistsAsync();
                    var Movimiento = new Movimientos("Trabajador", (contarMov+1).ToString());
                    Movimiento.FECHAINICIO = Date;
                    Movimiento.SDI = double.Parse(txtSalario.Text);
                    Movimiento.VALOR = 0;
                    Movimiento.ESTATUS = 0;
                    Movimiento.DIAS = 0;
                    Movimiento.FOLIO = "-";
                    Movimiento.RAMAINICIO = "-";
                    Movimiento.TIPORGO = "-";
                    Movimiento.SECUELA = "-";
                    Movimiento.CRTLINCAP = "-";
                    Movimiento.FECHATERM = "-";
                    Movimiento.PORCINCAP = 0;
                    Movimiento.CREDITO = 0;
                    Movimiento.TIPO = "-";
                    Movimiento.RFC = txtRFC.Text;
                    Movimiento.Movimiento = "Alta";
                    var Guardar = TableOperation.Insert(Movimiento);
                    await TMov.ExecuteAsync(Guardar);
                    Toast.MakeText(this, "Datos Guardados", ToastLength.Long).Show();
                }
                catch (System.Exception EX)
                {

                }

            };
        }
        public async Task CargarDatos()
        {
            var CuentadeAlmacenamiento = CloudStorageAccount.Parse
            ("DefaultEndpointsProtocol=https;AccountName=programacionmovil;AccountKey=RvGLvIagYrqkqSzVy5pvVwLBR2+G+Bf/B2UqoZeFNrZXPwAhuUPVGAOhC33JTTZZOJlmSJqMALB6tAXw//J1Tg==;EndpointSuffix=core.windows.net");
            var TablaNoSQL = CuentadeAlmacenamiento.CreateCloudTableClient();
            var Tabla = TablaNoSQL.GetTableReference("Movimientos");
            var Consulta = new TableQuery<Movimientos>();
            TableContinuationToken token = null;
            var Datos = await Tabla.ExecuteQuerySegmentedAsync<Movimientos>
                (Consulta, token, null, null);
            contarMov = Datos.Results.Count;
        }



        public class Registrar : TableEntity
        {
            public Registrar(string Categoria, string Nombre)
            {
                PartitionKey = Categoria;
                RowKey = Nombre;
            }
            public Registrar() { }
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