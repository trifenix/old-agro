﻿using System.ComponentModel.DataAnnotations;

using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.input;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de trabajo
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.JOB)]
    public class JobInput : InputBase {

        /// <summary>
        /// Nombre
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }
    }

 

}