﻿using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.interfaces.db.cosmos
{
    public interface ICommonQueries {

        Task<string> GetSpecieAbbreviation(string idSpecie);

        Task<string> GetSpecieAbbreviationFromVariety(string idVariety);

        Task<string> GetSpecieAbbreviationFromBarrack(string idBarrack);

        Task<string> GetSpecieAbbreviationFromOrder(string idOrder);

        Task<List<string>> GetUsersMailsFromRoles(List<string> idsRoles);

        Task<string> GetSeasonId(string idBarrack);

        Task<string> GetUserIdFromAAD(string idAAD);

        Task<string> GetDefaultDosesId(string idProduct);

        Task<IEnumerable<string>> GetActiveDosesIdsFromProductId(string idProduct);

        Task<string> GetEntityName<T>(string id) where T : DocumentBaseName;



    }

}