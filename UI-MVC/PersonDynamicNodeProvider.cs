using System.Collections.Generic;
using BAR.BL.Managers;
using MvcSiteMapProvider;

namespace BAR.UI.MVC {
    public class PersonDynamicNodeProvider : DynamicNodeProviderBase {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node) {
            IItemManager itemMgr = new ItemManager();
            // Create a node for each album 
            foreach (var item in itemMgr.GetAllItems()) {
                DynamicNode dynamicNode = new DynamicNode();
                dynamicNode.Title = item.Name;
                dynamicNode.RouteValues.Add("id", item.ItemId);

                yield return dynamicNode;
            }
        }
    }
}