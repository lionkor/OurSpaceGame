namespace Core
{
    /// <summary>
    /// Holds two indices of an object in an array.
    /// This is useful for passing an object in an
    /// array its own position in that array, without
    /// using a Vector2 or similar, which may be slower.
    /// </summary>
    [System.Serializable]
    public struct Index2D
    {
        public int x;
        public int y;
    } 
}