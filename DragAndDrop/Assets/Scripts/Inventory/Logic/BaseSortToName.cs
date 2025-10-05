using System;
using System.Collections.Generic;

namespace Inventory.Logic
{
    public class BaseSortToName : BaseSort
    {
        public override List<(ItemData item, int count)> Sort(List<(ItemData item, int count)> list)
        {
            list.Sort((a, b) => 
                string.Compare(a.item.name, b.item.name, StringComparison.OrdinalIgnoreCase));
            return list;
        }
    }
}