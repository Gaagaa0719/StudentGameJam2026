using System.Collections;

namespace Assets.Scenes.MainGame
{
    public abstract class VisualEvent
    {
        public abstract IEnumerator Play();
    }
}