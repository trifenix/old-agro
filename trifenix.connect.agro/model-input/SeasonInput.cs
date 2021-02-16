﻿using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.input;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de temporadas
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.SEASON)]
    public class SeasonInput : InputBase {

        /// <summary>
        /// Fecha de inicio
        /// </summary>
        [Required]
        [DateSearch(DateRelated.START_DATE_SEASON)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de término
        /// </summary>
        [Required]
        [DateSearch(DateRelated.END_DATE_SEASON)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Función que se encarga de determinar si la temporada está activa o no
        /// </summary>
        [Required]
        [BoolSearch(BoolRelated.CURRENT)]
        public bool Current { get; set; }

    }
}