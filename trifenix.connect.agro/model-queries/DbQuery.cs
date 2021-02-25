﻿using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.connect.agro.model_queries
{
    public enum DbQuery
    {
        SPECIEABBREVIATION_FROM_SPECIEID = 0,
        SPECIEID_FROM_VARIETYID = 1,
        VARIETYID_FROM_BARRACKID = 2,
        IDBARRACK_FROM_ORDERID = 3,
        MAILUSERS_FROM_ROLES = 4,
        SEASONID_FROM_BARRACKID = 5,
        USERID_FROM_IDAAD = 6,
        COUNT_BY_ID = 7,
        COUNT_BY_NAMEVALUE = 8,
        COUNT_BY_NAMEVALUE_AND_NOID = 9,
        COUNT_EXECUTION_OR_ORDERS_BY_DOSESID = 10,
        COUNT_DOSES_BY_PRODUCTID = 11,
        MAXCORRELATIVE_DOSES_BY_PRODUCTID = 12,
        CORRELATIVE_FROM_DOSESID = 13,
        DEFAULTDOSESID_BY_PRODUCTID  = 14,
        ACTIVEDOSESIDS_FROM_PRODUCTID = 15,
        NAME_BY_ID = 16,
        ORDERFOLDERINGREDIENT_FROM_PREORDER = 18,
        PREORDERINGREDIENT_FROM_ORDERFOLDER = 19,
        DUPLICATED_ORDERFOLDERS = 20,
        COSTCENTER_ACTIVESEASON = 21,
        SEASONSTATUS = 22,
        COSTCENTER_FROM_BUSINESSNAME = 23,
        BARRACKS_FROM_ORDERFOLDER = 24,
        SPECIE_FROM_ORDERFOLDER = 25,
        ORDERFOLDER_ATTRIBUTES = 26,
        SIMILAR_ORDERFOLDER = 27,
    }
}
