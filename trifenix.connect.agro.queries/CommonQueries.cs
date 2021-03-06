﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.model_queries;
using trifenix.connect.agro_model;
using trifenix.connect.arguments;
using trifenix.connect.db.cosmos;

namespace trifenix.connect.agro.queries
{
    /// <summary>
    /// Consultas comunes a la base de datos
    /// </summary>
    public class CommonQueries : BaseQueries, ICommonAgroQueries
    {

        public CommonQueries(CosmosDbArguments dbArguments) : base(dbArguments) { }


        public string Queries(DbQuery query) => new Queries().Get(query);

        /// <summary>
        /// Obtener el e-mail de un usuario según su rol
        /// </summary>
        /// <param name="idsRoles"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUsersMailsFromRoles(List<string> idsRoles)
        {
            var result = await MultipleQuery<User, string>(Queries(DbQuery.MAILUSERS_FROM_ROLES), string.Join(",", idsRoles.Select(idRole => $"'{idRole}'").ToArray()));
            List<string> emails = result.ToList();
            return emails;
        }
        /// <summary>
        /// Obtener la temporada de un cuartel segun su id
        /// </summary>
        /// <param name="idBarrack"></param>
        /// <returns></returns>
        public async Task<string> GetSeasonId(string idBarrack) => await SingleQuery<Barrack, string>(Queries(DbQuery.SEASONID_FROM_BARRACKID), idBarrack);

        /// <summary>
        /// Obtener el id de usuario segun su AAD(?
        /// </summary>
        /// <param name="idAAD"></param>
        /// <returns></returns>
        public async Task<string> GetUserIdFromAAD(string idAAD) => await SingleQuery<User, string>(Queries(DbQuery.USERID_FROM_IDAAD), idAAD);

        /// <summary>
        /// Obtener default dosis de un producto segun su id
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public async Task<string> GetDefaultDosesId(string idProduct) => await SingleQuery<Dose, string>(Queries(DbQuery.DEFAULTDOSESID_BY_PRODUCTID), idProduct);

        /// <summary>
        /// Obtener dosis activas de un producto segun su id
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetActiveDosesIdsFromProductId(string idProduct) => await MultipleQuery<Dose, string>(Queries(DbQuery.ACTIVEDOSESIDS_FROM_PRODUCTID), idProduct);

        /// <summary>
        /// Comprueba si el business name ingresado posee un cost center asociado
        /// </summary>
        /// <param name="idBusinessName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetCostCenterFromBusinessName(string idBusinessName) => await MultipleQuery<CostCenter, string>(Queries(DbQuery.COSTCENTER_FROM_BUSINESSNAME), idBusinessName);

        /// <summary>
        /// Obtiene el ingrediente de la order folder desde el order folder de la pre orden
        /// </summary>
        /// <param name="OrderFolderId"></param>
        /// <returns></returns>
        public async Task<string> GetOrderFolderIngredientFromPreOrder(string OrderFolderId) => await SingleQuery<OrderFolder, string>(Queries(DbQuery.ORDERFOLDERINGREDIENT_FROM_PREORDER), OrderFolderId);

        /// <summary>
        /// Obtiene los ingredientes de todas las pre ordenes de una older folder
        /// </summary>
        /// <param name="OrderFolderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetPreOrderIngredientFromOrderFolder(string OrderFolderId) => await MultipleQuery<PreOrder, string>(Queries(DbQuery.PREORDERINGREDIENT_FROM_ORDERFOLDER), OrderFolderId);

        /// <summary>
        /// Comprueba si existe una order folder duplicada
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDuplicatedOrderFolders(string ApplicationTargetId, string IngredientId, string PhenologicalEventId, string SpecieId) => await SingleQuery<OrderFolder, string>(Queries(DbQuery.DUPLICATED_ORDERFOLDERS), ApplicationTargetId, IngredientId, PhenologicalEventId, SpecieId);

        /// <summary>
        /// Obtiene la temporada activa si es que existe
        /// </summary>
        /// <param name="IdCostCenter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetCostCenterActiveSeason(string IdCostCenter) => await MultipleQuery<Season, string>(Queries(DbQuery.COSTCENTER_ACTIVESEASON), IdCostCenter);

        /// <summary>
        /// Obtiene el estado de una temporada
        /// </summary>
        /// <param name="IdSeason"></param>
        /// <returns></returns>
        public async Task<string> GetSeasonStatus(string IdSeason) => await SingleQuery<Season, string>(Queries(DbQuery.SEASONSTATUS), IdSeason);

        /// <summary>
        /// Obtiene todos los barracks asociados a una order folder
        /// </summary>
        /// <param name="IdOrderFolder"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IEnumerable<string>>> GetBarracksFromOrderFolderId(string IdOrderFolder) => await MultipleQuery<PreOrder, IEnumerable<string>>(Queries(DbQuery.BARRACKS_FROM_ORDERFOLDER), IdOrderFolder);

        /// <summary>
        /// Obtiene la variedad de un barrack
        /// </summary>
        /// <param name="IdBarrack"></param>
        /// <returns></returns>
        public async Task<string> GetBarrackVarietyFromBarrackId(string IdBarrack) => await SingleQuery<Barrack, string>(Queries(DbQuery.VARIETYID_FROM_BARRACKID), IdBarrack);

        /// <summary>
        /// Obtiene la especie de una variedad
        /// </summary>
        /// <param name="IdVariety"></param>
        /// <returns></returns>
        public async Task<string> GetSpecieFromVarietyId(string IdVariety) => await SingleQuery<Variety, string>(Queries(DbQuery.SPECIEID_FROM_VARIETYID), IdVariety);

        /// <summary>
        /// Obtiene la especie de la order folder
        /// </summary>
        /// <param name="IdOrderFolder"></param>
        /// <returns></returns>
        public async Task<string> GetOFSpecie(string IdOrderFolder) => await SingleQuery<OrderFolder, string>(Queries(DbQuery.SPECIE_FROM_ORDERFOLDER), IdOrderFolder);

        /// <summary>
        /// Obtiene los atributos de una order folder
        /// </summary>
        /// <param name="IdOrderFolder"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Dictionary<string, string>>> GetOFAttributes(string IdOrderFolder) => await MultipleQuery<OrderFolder, Dictionary<string, string>>(Queries(DbQuery.ORDERFOLDER_ATTRIBUTES), IdOrderFolder);

        /// <summary>
        /// Obtiene las order folder que tengan el mismo evento fenológico, el mismo objetivo de aplicación y la misma especie
        /// </summary>
        /// <param name="IdPhenologicalEvent"></param>
        /// <param name="IdApplicationTarget"></param>
        /// <param name="IdSpecie"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetSimilarOF(string IdPhenologicalEvent, string IdApplicationTarget, string IdSpecie) => await MultipleQuery<OrderFolder, string>(Queries(DbQuery.SIMILAR_ORDERFOLDER), IdPhenologicalEvent, IdApplicationTarget, IdSpecie);

        /// <summary>
        /// Obtiene los barracks de una misma order folder
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IEnumerable<string>>> GetOFBarracks(string Id) => await MultipleQuery<PreOrder, IEnumerable<string>>(Queries(DbQuery.OFBARRACKS), Id);

    }
}