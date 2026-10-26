namespace Sakemottekoi.MainGame
{
    public abstract class OrderRule
    {
        /// <summary>
        /// 自身が実行されるより前に実行すべきシステムのID
        /// </summary>
        public abstract string BeforeId { get; protected set; }

        protected OrderRule(string afterId = null)
        {
            BeforeId = afterId;
        }
    }

    public class OrderNext : OrderRule
    {
        public override string BeforeId { get; protected set; }
        public string NextId { get; protected set; }

        public OrderNext(string nextId, string afterId = null) : base(afterId)
        {
            NextId = nextId;
        }
    }

    public class OrderRelative : OrderRule
    {
        public override string BeforeId { get; protected set; }

        /// <summary>
        /// 自身が実行された後に実行すべきシステムのID
        /// </summary>
        public string AfterId { get; protected set; }

        /// <param name="beforeId">自身が実行されるより前に実行すべきシステムのID</param>
        /// <param name="afterId">自身が実行された後に実行すべきシステムのID</param>
        public OrderRelative(string beforeId = null, string afterId = null) : base(afterId)
        {
            AfterId = beforeId;
        }
    }
}