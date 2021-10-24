﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualBasic;
using Quoridor.Model;
using Quoridor.View;

namespace Quoridor
{
    static class Program
    {
        static void Main(string[] args)
        {
            var playersPosition = new List<Player> {new ("A", new Cell(0, 4)), new ("B", new Cell(8, 4))};
            var walls = new List<(Vector2, Vector2)> { (new Vector2(0, 0), new Vector2(0, 1)), (new Vector2(0, 2), new Vector2(0, 3)), 
                                                    (new Vector2(0, 2), new Vector2(1, 2)), (new Vector2(2, 2), new Vector2(3, 2))};
            
            var output = new ConsoleView();
            output.PrintGameField(playersPosition, walls);
        }
    }
}
