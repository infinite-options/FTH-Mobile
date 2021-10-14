using System;
using System.Collections.Generic;

namespace FTH.Model
{
    public class GetItems
    {
        public List<string> types { get; set; }
        public List<string> ids { get; set; }
    }

    public class GetItemsResponse
    {
        public string message { get; set; }
        public int code { get; set; }
        public BusinessInfo[] result { get; set; }
    }

    public class BusinessInfo
    {
        public string package_uid { get; set; }
        public string package_item_uid { get; set; }
        public string SKU { get; set; }
        public string package_num { get; set; }
        public string package_unit { get; set; }
        public string measure_num { get; set; }
        public string measure_unit { get; set; }
        public string item_num { get; set; }
        public string item_unit { get; set; }
        public string item_uid { get; set; }
        public string created_at { get; set; }
        public string item_name { get; set; }
        public string item_info { get; set; }
        public string item_type { get; set; }
        public string item_desc { get; set; }
        public string brand_name { get; set; }
        public string item_photo { get; set; }
        public string item_display { get; set; }
        public string item_tags { get; set; }
        public string supply_uid { get; set; }
        public string supply_created_at { get; set; }
        public string itm_business_uid { get; set; }
        public string sup_package_uid { get; set; }
        public string sup_type { get; set; }
        public string item_qty { get; set; }
        public string receive_date { get; set; }
        public string available_date { get; set; }
        public string exp_date { get; set; }
        public string item_status { get; set; }
        public double business_price { get; set; }
    }
}
