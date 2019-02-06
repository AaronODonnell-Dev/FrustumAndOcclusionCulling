using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frustum_and_Occlusion_Culling
{
    class TextureModel : CustomEffectModel
    {
        public TextureModel(string asset, Vector3 position) : base(asset, position) { }

        public override void LoadContent()
        {
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects/BasicTexture");

            Texture2D tex = GameUtilities.Content.Load<Texture2D>("Textures/cubeCover");
            Texture2D overlay = GameUtilities.Content.Load<Texture2D>("Textures/overlay");
            Material = new TextureMaterial(Color.White, tex, overlay);
            //OverlayMaterial = new TextureMaterial(Color.White, 0, overlay);

            base.LoadContent();
        }
    }
}
