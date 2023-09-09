using UnityEngine;

namespace FUI.UGUI.ValueConverter
{
    public class StringToSpriteConverter : AssetLoadableValueConverter<string, Sprite>
    {
        public override Sprite Convert(string value)
        {
            return AssetLoader.Load<Sprite>(value);
        }

        public override string ConvertBack(Sprite value)
        {
            return value.name;
        }
    }
}
