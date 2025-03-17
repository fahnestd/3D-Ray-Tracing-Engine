namespace Engine.Util
{
    internal class Converters
    {
        public static float ToRadians(float degrees)
        {
            return degrees * MathF.PI / 180.0f;
        }
    }
}
