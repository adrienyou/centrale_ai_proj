﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Map
    {
        private int _Height;
        private int _Width;
        private State _State; // ref to the current state of the game
        private CellType _FriendlyType;

        public Map(int height, int width)
        {
            _Height = height;
            _Width = width;
            _State = null;
            _FriendlyType = CellType.Empty;
        }
 
        public Map(int height, int width, CellType type)
        {
            _Height = height;
            _Width = width;
            _State = null;
            _FriendlyType = type;
        }

        // Fields accessors
        public int Height
        {
            get { return _Height; }
        }

        public int Width
        {
            get { return _Width; }
        }

        public CellType FriendlyType
        {
            get { return _FriendlyType; }
            set { _FriendlyType = value;  }
        }

        public State State
        {
            get { return _State; }
            set
            {
                if (value == null)
                    return;
                _State = value;
            }
        }
    }
}
