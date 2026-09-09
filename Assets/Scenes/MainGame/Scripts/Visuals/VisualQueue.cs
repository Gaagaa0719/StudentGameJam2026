using System.Collections;
using System.Collections.Generic;

namespace Sakemottekoi.MainGame
{
    public class VisualQueue
    {
        private static readonly Queue<VisualEvent> visuals = new();

        public static void Enqueue(VisualEvent visual)
        {
            visuals.Enqueue(visual);
        }

        public static IEnumerator PlayAll()
        {
            while(visuals.Count > 0)
            {
                yield return visuals.Dequeue().Play();
            }
        }
    }
}