using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Sakemottekoi.MainGame.Tetris
{
    public enum TetrisBlock
    {
        None,
        Orange,
        Green
    }

    public class Tetris : MiniGame
    {
        [SerializeField] private TetrisRenderer tetrisRenderer;
        [SerializeField] private ParticleSystem filledParticle;

        private Vector2Int fieldPosOffset = new (-65, -1);
        private BoundsInt fieldBounds = new(Vector3Int.zero, new Vector3Int(10, 13, 1));
        private List<List<TetrisBlock>> fieldData = new();

        private Vector2Int[] tetromino;

        private bool isControllable = true;
        private float elappsedTime = 0f;
        private float fallInterval = 0.8f;

        protected override void OnStart(float dp)
        {
            isControllable = true;
            elappsedTime = 0f;

            tetromino = new Vector2Int[4]
            {
                new (fieldBounds.xMax / 2, fieldBounds.yMax - 1),
                new (fieldBounds.xMax / 2, fieldBounds.yMax - 2),
                new (fieldBounds.xMax / 2, fieldBounds.yMax - 3),
                new (fieldBounds.xMax / 2, fieldBounds.yMax - 4)
            };
            InitField();
        }

        private void Update()
        {
            if (!isPlaying) return;
            if (!isControllable) return;

            elappsedTime += Time.deltaTime;
            if (elappsedTime < fallInterval) return;

            // tick処理
            elappsedTime -= fallInterval;
            bool result = TryMove(Vector2Int.down);
            if (result) return;

            isControllable = false;
            Judge();
        }

        private void InitField()
        {
            fieldData = new();

            int emptyRow = UnityEngine.Random.Range(0, fieldBounds.xMax);
            for (int y = 0; y < fieldBounds.yMax; y++)
            {
                fieldData.Add(new());
                for (int x = 0; x < fieldBounds.xMax; x++)
                {
                    if(emptyRow == x || y >= 4) fieldData[y].Add(TetrisBlock.None);
                    else fieldData[y].Add(TetrisBlock.Orange);
                }
            }

            foreach (var pos in tetromino)
            {
                fieldData[pos.y][pos.x] = TetrisBlock.Green;
            }

            RequestRender();
        }

        public override void OnPrimary()
        {
            if (!isControllable) return;
            TryMove(Vector2Int.left);
        }

        public override void OnSecondary()
        {
            if (!isControllable) return;
            TryMove(Vector2Int.right);
        }

        private bool TryMove(Vector2Int moveVec)
        {
            Vector2Int[] nextTetromino = tetromino.Select(v => v + moveVec).ToArray();
            if (nextTetromino.Any(pos => IsCollide((Vector3Int)pos)) ) return false;

            foreach (var pos in tetromino) fieldData[pos.y][pos.x] = TetrisBlock.None;
            foreach (var pos in nextTetromino) fieldData[pos.y][pos.x] = TetrisBlock.Green;
            tetromino = nextTetromino;

            RequestRender();
            return true;
        }

        private bool IsCollide(Vector3Int pos)
        {
            if (!fieldBounds.Contains(pos)) return true;
            if (fieldData[pos.y][pos.x] == TetrisBlock.Orange) return true;

            return false;
        }

        private async void Judge()
        {
            await CheckLines();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), ignoreTimeScale: false);
            bool isAllRemoved = fieldData.All(line => line.All(v => v == TetrisBlock.None));
            FinishGame(isAllRemoved);
        }

        private async UniTask CheckLines()
        {
            for (int y = fieldData.Count - 1; y >= 0; y--)
            {
                var line = fieldData[y];
                if (line.Any(v => v == TetrisBlock.None)) continue;

                ParticleSystem particle = Instantiate<ParticleSystem>(filledParticle, new Vector3(0, y - 6, 0), Quaternion.identity);
                particle.transform.SetParent(transform, false);
                particle.Play();
                Destroy(particle, 0.5f);

                for (int x = 0; x < line.Count; x++) line[x] = TetrisBlock.None;
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), ignoreTimeScale: false);
                RequestRender();
            }
        }

        private void RequestRender()
        {
            tetrisRenderer.Render(fieldData, fieldPosOffset);
        }
    }
}