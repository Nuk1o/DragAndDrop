using System.Collections.Generic;

namespace Inventory.Logic
{
    public abstract class BaseSort
    {
        public abstract List<(ItemData item, int count)> Sort(List<(ItemData item, int count)> list);
    }
}