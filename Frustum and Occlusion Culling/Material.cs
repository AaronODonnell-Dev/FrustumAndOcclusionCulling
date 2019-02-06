using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frustum_and_Occlusion_Culling
{
    //Storing effect parameters
    public class Material
    {
        public virtual void SetEffectParameters(Effect effect) { }
        public virtual void Update() { }
    }

    public class TextureMaterial : Material
    {
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D Overlay { get; set; }

        public TextureMaterial(Color color, Texture2D texture, Texture2D overlay) : base()
        {
            Color = color;
            Texture = texture;
            Overlay = overlay;
        }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["Color"].SetValue(Color.ToVector3());
            effect.Parameters["ModelTexture"].SetValue(Texture);
            effect.Parameters["SecondModelTexture"].SetValue(Overlay);
        }
    }

    public class ColorMaterial : Material
    {
        public Color Color { get; set; }
        public Color AltColor { get; set; }
        public bool DrawAlt { get; set; }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["Color"].SetValue(Color.ToVector3());
            effect.Parameters["AltColor"].SetValue(AltColor.ToVector3());
            effect.Parameters["DrawAlt"].SetValue(DrawAlt);

            base.SetEffectParameters(effect);
        }

        public ColorMaterial(): base()
        {
            Color = Color.White;
            AltColor = Color.LawnGreen;
            DrawAlt = true;
        }
    }
}
