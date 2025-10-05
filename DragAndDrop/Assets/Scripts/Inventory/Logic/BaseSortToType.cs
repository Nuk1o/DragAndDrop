using System.Collections.Generic;

namespace Inventory.Logic
{
    public class BaseSortToType : BaseSort
    {
        public override List<(ItemData item, int count)> Sort(List<(ItemData item, int count)> list)
        {
            list.Sort((a, b) => a.item.type.CompareTo(b.item.type));
            return list;
        }
    }
}