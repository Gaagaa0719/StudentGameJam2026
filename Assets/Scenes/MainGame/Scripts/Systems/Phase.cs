using System;
using System.Collections;
using System.Collections.Generic;

namespace Sakemottekoi.MainGame
{
    public class PhaseBase<TSystem> where TSystem : GameSystemBase
    {
        protected bool isDirty = false;
        protected bool isRunning = false;
        protected List<TSystem> registeredSystemList = new();

        public PhaseBase(params TSystem[] systems)
        {
            registeredSystemList.AddRange(systems);
            Sort();
        }

        // 外部から動的に処理を追加する用の関数。
        public void Add(params TSystem[] systems)
        {
            if (isRunning) throw new Exception("フェーズの処理が走っている最中に追加を試みました。");
            registeredSystemList.AddRange(systems);
            isDirty = true;
        }

        /// <summary>
        /// 登録されたシステムをメタデータに従ってソートする。
        /// 破綻のある順序関係が見つかった場合エラーを吐く。
        /// </summary>
        protected void Sort()
        {
            var idToSystem = new Dictionary<string, TSystem>();
            var inDegree = new Dictionary<string, int>(); // 自身が実行される前に待たなければいけない処理の数
            var edges = new Dictionary<string, List<string>>(); // 自身の後に実行すべきSystemのIDリスト
            var nextConstraints = new Dictionary<string, string>(); // Next（直後）制約の辞書

            foreach (var sys in registeredSystemList)
            {
                if (!idToSystem.TryAdd(sys.Id, sys))
                {
                    throw new InvalidOperationException(
                        $"GameSystem ID '{sys.Id}' is duplicated.");
                }

                inDegree[sys.Id] = 0;
                edges[sys.Id] = new List<string>();
            }

            // 2. 依存関係の構築
            foreach (var sys in registeredSystemList)
            {
                var order = sys.OrderRule;

                // 自分より前に実行すべき処理について
                if (!string.IsNullOrEmpty(order.BeforeId) && idToSystem.ContainsKey(order.BeforeId))
                {
                    edges[order.BeforeId].Add(sys.Id);
                    inDegree[sys.Id]++;
                }

                // 自分より後に実行すべき処理について
                if (order is OrderRelative rel && !string.IsNullOrEmpty(rel.AfterId) && idToSystem.ContainsKey(rel.AfterId))
                {
                    edges[sys.Id].Add(rel.AfterId);
                    inDegree[rel.AfterId]++;
                }
                else if (order is OrderNext next && !string.IsNullOrEmpty(next.NextId) && idToSystem.ContainsKey(next.NextId))
                {
                    edges[sys.Id].Add(next.NextId);
                    inDegree[next.NextId]++;
                    nextConstraints[sys.Id] = next.NextId;
                }
            }

            // 3. 並び替えの実行
            var sortedList = new List<TSystem>();
            var readyNodes = new List<string>(); // 依存がなくなり、いつでも配置できるIDリスト

            // 最初に「誰にも依存していない」システムをピックアップ
            foreach (var kvp in inDegree)
            {
                if (kvp.Value == 0) readyNodes.Add(kvp.Key);
            }

            while (readyNodes.Count > 0)
            {
                // リストの先頭から取り出して結果に追加
                string currentId = readyNodes[0];
                readyNodes.RemoveAt(0);

                sortedList.Add(idToSystem[currentId]);

                // 直後（Next）に指定されているIDがあるか確認
                string nextTarget = nextConstraints.ContainsKey(currentId) ? nextConstraints[currentId] : null;

                // 自分が終わったので、自分に依存していたSystemの「待ちカウント」を1減らす
                foreach (var toId in edges[currentId])
                {
                    inDegree[toId]--;

                    // 待ちカウントが0になった（実行可能になった）場合
                    if (inDegree[toId] == 0)
                    {
                        if (toId == nextTarget)
                        {
                            // Next指定の対象だった場合、他の待機ノードを差し置いて「先頭」に割り込ませる
                            readyNodes.Insert(0, toId);
                            nextTarget = null; // 割り込み成功マーク
                        }
                        else
                        {
                            // 通常のBefore/Afterならリストの末尾に追加
                            readyNodes.Add(toId);
                        }
                    }
                }

                // エラー検知：Nextに指定したのに、別の依存関係のせいで待ちカウントが0にならなかった場合
                if (nextTarget != null)
                {
                    throw new Exception($"順序破綻: [{currentId}] のNextに指定された [{nextTarget}] が、他の依存関係によってブロックされています。");
                }
            }

            // 4. 循環参照の検知 (AがBを待ち、BがAを待っているとソートされないまま残る)
            if (sortedList.Count != registeredSystemList.Count)
            {
                throw new Exception("順序破綻: System間の依存関係に循環参照が存在します。");
            }

            // ソート成功
            registeredSystemList = sortedList;
            isDirty = false;
        }
    }

    public class Phase: PhaseBase<GameSystem>
    {
        public Phase(params GameSystem[] systems) : base(systems)
        {
        }

        public IEnumerator Run()
        {
            isRunning = true;
            if (isDirty) Sort();

            try
            {
                foreach (var system in registeredSystemList)
                {
                    system.Execute();
                    yield return VisualQueue.PlayAll();
                }

                // 一度きりの処理をクリア
                var removeCount = registeredSystemList.RemoveAll((v) => v.IsOneShot);
                if (removeCount != 0) isDirty = true;
            }
            finally
            {
                isRunning = false;
            }
        }
    }

    public class Phase<T> : PhaseBase<GameSystem<T>>
    {
        public Phase(params GameSystem<T>[] systems) : base(systems)
        {
        }

        public IEnumerator Run(T arg)
        {
            isRunning = true;
            if (isDirty) Sort();

            try
            {
                foreach (var system in registeredSystemList)
                {
                    system.Execute(arg);
                    yield return VisualQueue.PlayAll();
                }

                // 一度きりの処理をクリア
                var removeCount = registeredSystemList.RemoveAll((v) => v.IsOneShot);
                if (removeCount != 0) isDirty = true;
            }
            finally
            {
                isRunning = false;
            }
        }
    }
}