namespace PlayVibe
{
    public class InfoPopupData
    {
        public string Message { get; }
        public float LifeTime { get; }

        public InfoPopupData(string message, float lifeTime = 1f)
        {
            Message = message;
            LifeTime = lifeTime;
        }
    }
}