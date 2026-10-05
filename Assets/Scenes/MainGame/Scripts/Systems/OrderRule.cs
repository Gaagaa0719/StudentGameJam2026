namespace Sakemottekoi.MainGame
{
    public abstract class OrderRule
    {
        /// <summary>
        /// 同じIDのシステムが登録されていた場合の優先順位(大きいほうが優先)
        /// </summary>
        public abstract int Priority { get; protected set; }

        /// <summary>
        /// 自身が実行されるより前に実行すべきシステムのID
        /// </summary>
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

        /// <summary>
        /// 自身が実行された後に実行すべきシステムのID
        /// </summary>
        public string BeforeId { get; protected set; }

        /// <param name="beforeId">自身が実行された後に実行すべきシステムのID</param>
        /// <param name="afterId">自身が実行されるより前に実行すべきシステムのID</param>
        /// <param name="priority">同じIDのシステムが登録されていた場合の優先順位(大きいほうが優先)</param>
        public OrderRelative(string beforeId = null, string afterId = null, int priority = 0) : base(afterId, priority)
        {
            BeforeId = beforeId;
        }
    }
}