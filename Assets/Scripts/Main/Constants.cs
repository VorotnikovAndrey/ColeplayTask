namespace PlayVibe
{
    public static class Constants
    {
        public static class Stages
        {
            public const string GameplayStage = nameof(GameplayStage);
        }
        
        public static class Popups
        {
            public const string HudPopup = nameof(HudPopup);
            public const string InfoPopup = nameof(InfoPopup);
        }

        public static class Views
        {
            public const string FigureView = nameof(FigureView);
        }  
        
        public static class Text
        {
            public const string TargetColorFormat = "Click on the <color={1}>{0}</color> figure";
            public const string IncorrectColor = "Wrong selection, try again!";
        }
    }
}

