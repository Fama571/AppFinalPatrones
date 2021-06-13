using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Linq;
using Android.Content;
using System.Threading.Tasks;
using Android.Graphics;

namespace AppFinalPatrones
{
    [Activity(Label = "ConsultaMovimientos")]
    public class ConsultaMovimientos : Activity
    {
        Android.App.ProgressDialog progress;
        ListView listado;
        List<Movimientos> ListadeMovimientos = new List<Movimientos>();
        List<ElementosdeTabla> ElementosTabla = new List<ElementosdeTabla>();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListaMovimientos);
            listado = FindViewById<ListView>(Resource.Id.ListaMovimientos);
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Cargando...");
            progress.SetCancelable(false);
            progress.Show();
            await CargarDatos();
            progress.Hide();

        }
        public async Task CargarDatos()
        {
            try
            {
                var titulo = FindViewById<TextView>(Resource.Id.textView);
                var typeface = Typeface.CreateFromAsset(this.Assets, "fonts/BebasNeue.ttf");
                titulo.SetTypeface(typeface, TypefaceStyle.Normal);
                var CuentadeAlmacenamiento = CloudStorageAccount.Parse
                    ("DefaultEndpointsProtocol=https;AccountName=programacionmovil;AccountKey=RvGLvIagYrqkqSzVy5pvVwLBR2+G+Bf/B2UqoZeFNrZXPwAhuUPVGAOhC33JTTZZOJlmSJqMALB6tAXw//J1Tg==;EndpointSuffix=core.windows.net");
                var TablaNoSQL = CuentadeAlmacenamiento.CreateCloudTableClient();
                var Tabla = TablaNoSQL.GetTableReference("Movimientos");
                var Consulta = new TableQuery<Movimientos>();
                TableContinuationToken token = null;
                var Datos = await Tabla.ExecuteQuerySegmentedAsync<Movimientos>
                    (Consulta, token, null, null);
                ListadeMovimientos.AddRange(Datos.Results);
                int iID =0, iRFC=0, iSDI=0, iFI=0,iFF=0, iVALOR =0, iESTATUS=0, iFOLIO=0,iDIAS=0,iRAMAINICIO=0,iTIPORGO=0,iSECUELA=0,iCRTLINCAP=0,iFECHATERM=0,iPORCINCAP=0,iCREDITO=0,iTIPO=0,iMovimiento=0;
                ElementosTabla = ListadeMovimientos.Select(r => new ElementosdeTabla()
                {
                    ID = ListadeMovimientos.ElementAt(iID++).RowKey,
                    RFC = ListadeMovimientos.ElementAt(iRFC++).RFC,
                    SDI = ListadeMovimientos.ElementAt(iSDI++).SDI,
                    FECHAINICIO = ListadeMovimientos.ElementAt(iFI++).FECHAINICIO,
                    VALOR = ListadeMovimientos.ElementAt(iVALOR++).VALOR,
                    ESTATUS = ListadeMovimientos.ElementAt(iESTATUS++).ESTATUS,
                    DIAS = ListadeMovimientos.ElementAt(iDIAS++).DIAS,
                    FOLIO = ListadeMovimientos.ElementAt(iFOLIO++).FOLIO,
                    RAMAINICIO = ListadeMovimientos.ElementAt(iRAMAINICIO++).RAMAINICIO,
                    TIPORGO = ListadeMovimientos.ElementAt(iTIPORGO++).TIPORGO,
                    SECUELA = ListadeMovimientos.ElementAt(iSECUELA++).SECUELA,
                    CRTLINCAP = ListadeMovimientos.ElementAt(iCRTLINCAP++).CRTLINCAP,
                    FECHATERM = ListadeMovimientos.ElementAt(iFECHATERM++).FECHATERM,
                    PORCINCAP = ListadeMovimientos.ElementAt(iPORCINCAP++).PORCINCAP,
                    CREDITO = ListadeMovimientos.ElementAt(iCREDITO++).CREDITO,
                    TIPO = ListadeMovimientos.ElementAt(iTIPO++).TIPO,
                    Movimiento = ListadeMovimientos.ElementAt(iMovimiento++).Movimiento,
                }).ToList();
                Toast.MakeText(this, "Movimientos Obtenidos", ToastLength.Long).Show();
                listado.Adapter = new MovAdapter(this, ElementosTabla);
                
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

        }
    }
    
    public class ElementosdeTabla
    {
        public string ID { get; set; }
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