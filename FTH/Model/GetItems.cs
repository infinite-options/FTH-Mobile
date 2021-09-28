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
        public string receive_uid { get; set; }
        public string receive_supply_uid { get; set; }
        public string receive_business_uid { get; set; }
        public string donation_type { get; set; }
        public string qty_received { get; set; }
        public string receive_date { get; set; }
        public string available_date { get; set; }
        public string exp_date { get; set; }
        public string supply_uid { get; set; }
        public string sup_created_at { get; set; }
        public string sup_brand_uid { get; set; }
        public string sup_item_uid { get; set; }
        public string sup_desc { get; set; }
        public string sup_type { get; set; }
        public string sup_num { get; set; }
        public string sup_measure { get; set; }
        public string sup_unit { get; set; }
        public string detailed_num { get; set; }
        public string detailed_measure { get; set; }
        public string detailed_unit { get; set; }
        public string item_photo { get; set; }
        public string package_upc { get; set; }
        public string item_uid { get; set; }
        public string item_name { get; set; }
        public string item_desc { get; set; }
        public string item_type { get; set; }
        public string item_tags { get; set; }
        public string measure_uid { get; set; }
        public string measure_supply_uid { get; set; }
        public string measure_business_uid { get; set; }
        public string measure_dist_uid { get; set; }
        public string measure_receive_uid { get; set; }
        public string distribution_default { get; set; }
        public string distribution_status { get; set; }
        public string distribution_qty { get; set; }
        public string dist_options_uid { get; set; }
        public string dist_supply_uid { get; set; }
        public string dist_desc { get; set; }
        public string dist_type { get; set; }
        public string dist_num { get; set; }
        public string dist_measure { get; set; }
        public string dist_unit { get; set; }
        public string dist_item_photo { get; set; }
    }
}
