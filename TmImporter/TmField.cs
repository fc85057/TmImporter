using System.Collections.Generic;

namespace TmImporter
{
    public class TmField
    {
        public string Name { get; private set; }
        public bool IsPicklist { get; private set; }
        public List<string> PicklistValues { get; private set; }
        public string SelectedValue { get; set; }

        public TmField (string name, bool isPicklist, List<string> picklistValues)
        {
            Name = name;
            IsPicklist = isPicklist;
            PicklistValues = picklistValues;
        }

    }
}
