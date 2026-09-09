namespace Sakemottekoi.MainGame
{
    public abstract class OrderRule
    {
        public abstract int Priority { get; protected set; }

        public abstract string AfterId { get; protected set; }

        protected OrderRule(string afterId = null, int priority = 0)
        {
            Priority = priority;
            AfterId = afterId;
        }
    }

    public class OrderNext : OrderRule
    {
        public override int Priority { get; protected set; }
        public override string AfterId { get; protected set; }
        public string NextId { get; protected set; }

        public OrderNext(string nextId, string afterId = null, int priority = 0) : base(afterId, priority)
        {
            NextId = nextId;
        }
    }

    public class OrderRelative : OrderRule
    {
        public override int Priority { get; protected set; }
        public override string AfterId { get; protected set; }
        public string BeforeId { get; protected set; }

        public OrderRelative(string beforeId = null, string afterId = null, int priority = 0) : base(afterId, priority)
        {
            BeforeId = beforeId;
        }
    }
}