using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinesweeperWPF
{
    public interface IGenerator
    {
        int Next(int number);
    }
}
