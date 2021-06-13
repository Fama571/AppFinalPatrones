using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Graphics;
using AndroidX.Core.Graphics.Drawable;
namespace AppFinalPatrones
{
    class MovAdapter : BaseAdapter<ElementosdeTabla>
    {
        List<ElementosdeTabla> items;
        Activity context;

        public MovAdapter(Activity context, List<ElementosdeTabla> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override ElementosdeTabla this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            view = context.LayoutInflater.Inflate(Resource.Layout.Movimientos_Detail, null);
            view.FindViewById<TextView>(Resource.Id.txtMovimiento).Text = item.Movimiento;
            view.FindViewById<TextView>(Resource.Id.txtFecInicio).Text = item.FECHAINICIO;
            view.FindViewById<TextView>(Resource.Id.txtFecFin).Text = item.FECHATERM;
            view.FindViewById<TextView>(Resource.Id.txtSDI).Text = item.SDI.ToString();
            view.FindViewById<TextView>(Resource.Id.txtRFCC).Text = item.RFC;

            return view;
        }


    }
}