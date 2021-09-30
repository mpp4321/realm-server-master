using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class SetAltTexture : Behavior
    {

        class TextureState
        {
            public int currentTexture;
            public int remainingTime;
        }

        private static Random Random = new Random();

        private readonly int _indexMin;
        private readonly int _indexMax;
        private Cooldown _cooldown;
        private readonly bool _loop;

        public SetAltTexture(int minValue, int maxValue = -1, Cooldown cooldown = new Cooldown(), bool loop = false)
        {
            _indexMin = minValue;
            if(maxValue == -1)
            {
                _indexMax = minValue;
                _indexMin = minValue - 1;
            } else
                _indexMax = maxValue;
            _cooldown = cooldown.Normalize(0);
            _loop = loop;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new TextureState()
            {
                currentTexture = host.AltTextureIndex,
                remainingTime = _cooldown.Next(Random)
            };
            if (host.AltTextureIndex != _indexMin)
            {
                host.AltTextureIndex = _indexMin;
                (host.StateObject[Id] as TextureState).currentTexture = _indexMin;
            }
        }

        public override bool Tick(Entity host)
        {
            var textState = host.StateObject[Id] as TextureState;

            if (_indexMax == -1 || (textState.currentTexture == _indexMax && !_loop))
                return false;

            if (textState.remainingTime <= 0)
            {
                int newTexture = (textState.currentTexture >= _indexMax) ? _indexMin : textState.currentTexture + 1;
                host.AltTextureIndex = newTexture;
                textState.currentTexture = newTexture;
                textState.remainingTime = _cooldown.Next(Random);
            }
            else
            {
                textState.remainingTime -= Settings.MillisecondsPerTick;
            }
            return true;
        }

    }
}
