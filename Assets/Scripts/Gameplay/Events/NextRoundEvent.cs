using Gameplay.Figure;
using PlayVibe;

namespace Gameplay.Events
{
    public class NextRoundEvent : AbstractBaseEvent
    {
        public FigureColorType TargetColor { get; }

        public NextRoundEvent(FigureColorType targetColor)
        {
            TargetColor = targetColor;
        }
    }
}