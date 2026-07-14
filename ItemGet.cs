using System;
using Yukar.Engine;
using Microsoft.Xna.Framework;

namespace Bakin
{
    public class ItemGet : BakinObject 
    {
        // 確認したXとZの最小値・最大値を入力
        // Set the minimum and maximum X/Z coordinates of the target area.
        private float MIN_X = 10.00f;
        private float MAX_X = 14.00f;
        private float MIN_Z = 11.00f;
        private float MAX_Z = 15.00f;

        // どのくらいの確率でアイテムを入手したいか入力
        // Set the probability of obtaining the item.
        private double PROBABILITY = 0.1f; // 10%

        // 特定エリア内・歩行中かの判断に使用
        // Variables used to determine whether the player is inside the target area and walking.
        private bool isInSpecifiedArea = false;
        private bool isWalking = false;
        private bool firstUpdate = true;
        private float posX;
        private float posZ;
        private float prevPosX;
        private float prevPosZ;
        private float distance = 0f;

        [BakinFunction(Description="Obtain an item with a certain probability while walking within a specified area.")]
        public int SampleCode()
        {
            if(isInSpecifiedArea && isWalking)
            {
                // 確率を判断
                // Roll the probability.
                Random rand = new Random();
                if(rand.NextDouble() <= PROBABILITY)
                {
                    // デバッグ用
                    // Debug message.
                    mapScene.ShowToast("Get Item!");
                    return 1;
                }
            }
            return 0;
        }

        public override void Update()
        {
            // 前フレームの時点でどこにいたか格納
            // Store the player's position from the previous frame.
            prevPosX = posX;
            prevPosZ = posZ;

            // 主人公の現在位置を取得
            // Get the player's current position.
            Vector3 playersPos = mapScene.hero.getPosition();
            posX = playersPos.X;
            posZ = playersPos.Z;

            // 歩行中か判断
            // Determine whether the player has walked at least one tile.
            if(firstUpdate) {
                firstUpdate = false;
                return;
            }

            Vector2 prevPos = new Vector2(prevPosX, prevPosZ);
            Vector2 nowPos = new Vector2(posX, posZ);
            distance += Vector2.Distance(prevPos, nowPos);

            // 1マス以上動いたとき、歩行中と判断。
            // Consider the player to have walked after moving at least one tile.
            isWalking = (distance >= 1f);

            if(isWalking) distance = 0;

            // 特定エリア内にいるか判断
            // Check whether the player is inside the target area.
            isInSpecifiedArea =
                MIN_X <= posX
                && posX <= MAX_X
                && MIN_Z <= posZ
                && posZ <= MAX_Z;
        }
    }
}