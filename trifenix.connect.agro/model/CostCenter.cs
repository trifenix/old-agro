﻿using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{


    /// <summary>
    /// El centro de costos es la unidad central del sistema,
    /// todos los procesos, la bodega y los cuarteles dependen de un centro de costo.
    /// </summary>
    [SharedCosmosCollection("agro", "CostCenter")]
    [ReferenceSearchHeader(EntityRelated.COSTCENTER, PathName = "cost-centers", Kind = EntityKind.ENTITY)]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.GESTION)]
    public class CostCenter : DocumentBaseName<long>, ISharedCosmosEntity {


        /// <summary>
        /// Identificador del centro de costo
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Autonumérico del centro de costo
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }
        


        /// <summary>
        /// Nombre del centro de costo.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }


        /// <summary>
        /// Nombre del negocio.
        /// </summary>
        [ReferenceSearch(EntityRelated.BUSINESSNAME)]
        public string IdBusinessName { get; set; }
        
    }

}