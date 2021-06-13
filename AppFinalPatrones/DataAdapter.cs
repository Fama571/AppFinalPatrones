using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Graphics;
using AndroidX.Core.Graphics.Drawable;

namespace AppFinalPatrones
{
    class DataAdapter : BaseAdapter<ConsultaTrabajadores.ElementosdelaTabla>
    {
        List<ConsultaTrabajadores.ElementosdelaTabla> items;
        Activity context;

        public DataAdapter(Activity context, List<ConsultaTrabajadores.ElementosdelaTabla> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override ConsultaTrabajadores.ElementosdelaTabla this[int position]
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
            view = context.LayoutInflater.Inflate(Resource.Layout.Trabajador_Details, null);
            view.FindViewById<TextView>(Resource.Id.txtRFC).Text = item.RFC;
            var txtHead = view.FindViewById<TextView>(Resource.Id.txtNombre);
            txtHead.Text = item.Nombre;
            var typeface = Typeface.CreateFromAsset(context.Assets, "fonts/BebasNeue.ttf");
            txtHead.SetTypeface(typeface, TypefaceStyle.Normal);
            var path = System.IO.Path.Combine(System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal), item.imagen);
            var Image = BitmapFactory.DecodeFile(path);
            Image = ResizeBitmap(Image, 100, 100);
            view.FindViewById<ImageView>(Resource.Id.Imagen).SetImageDrawable(getRoundedCornerImagen(Image, 5));
            return view;
        }
        public static RoundedBitmapDrawable getRoundedCornerImagen(Bitmap image, int cornerRadius)
        {
            var corner = RoundedBitmapDrawableFactory.Create(null, image);
            corner.CornerRadius = cornerRadius;
            return corner;
        }

        private Bitmap ResizeBitmap(Bitmap imageoriginal, int withimageoriginal, int heightimagenoriginal)
        {
            Bitmap resizedImage = Bitmap.CreateBitmap(withimageoriginal, heightimagenoriginal, Bitmap.Config.Argb8888);
            float width = imageoriginal.Width;
            float height = imageoriginal.Height;
            var canvas = new Canvas(resizedImage);
            var scala = withimageoriginal / width;
            var xtranslation = 0.0f;
            var ytranslation = (heightimagenoriginal - height * scala) / 2.0f;
            var transformacion = new Matrix();
            transformacion.PostTranslate(xtranslation, ytranslation);
            transformacion.PreScale(scala, scala);
            var paint = new Paint();
            paint.FilterBitmap = true;
            canvas.DrawBitmap(imageoriginal, transformacion, paint);
            return resizedImage;
        }
    }
}