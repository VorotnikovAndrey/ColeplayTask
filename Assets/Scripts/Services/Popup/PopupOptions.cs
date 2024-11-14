namespace PlayVibe
{
    public sealed class PopupOptions
    {
        public object Data { get; }
        public string PopupKey { get; }
        public PopupGroup PopupGroup { get; }
        public int? SortingOrder { get; }
        public bool Force { get; }

        public PopupOptions(
            string popupKey,
            object data = null,
            PopupGroup popupGroup = PopupGroup.Gameplay,
            int? sortingOrder = null,
            bool force = false)
        {
            PopupKey = popupKey;
            Data = data;
            PopupGroup = popupGroup;
            SortingOrder = sortingOrder;
            Force = force;
        }
    }
}
