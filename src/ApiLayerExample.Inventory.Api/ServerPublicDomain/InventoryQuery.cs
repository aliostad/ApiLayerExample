using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayerExample.Inventory.Api.ServerPublicDomain
{
    public class InventoryQuery
    {
        public int ProductId { get; set; }

        public int? ProductVariantId { get; set; }
    }
}