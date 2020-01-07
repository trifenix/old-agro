﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.model.external;
using Cosmonaut.Extensions;
using trifenix.agro.db.interfaces.agro;
using System.Linq;
using trifenix.agro.external.operations.helper;
using trifenix.agro.db.interfaces.common;

namespace trifenix.agro.external.operations.entities.main
{
    public class SeasonOperations : ISeasonOperations
    {

        private readonly ISeasonRepository _repo;
        private readonly ICommonDbOperations<Season> _commonDb;

        public SeasonOperations(ISeasonRepository repo, ICommonDbOperations<Season> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }
        public async Task<ExtGetContainer<List<Season>>> GetSeasons()
        {
            var seasonsQuery = _repo.GetSeasons();
            var seasons = await _commonDb.TolistAsync(seasonsQuery);
            return OperationHelper.GetElements(seasons);
        }

        public async Task<ExtPostContainer<Season>> SaveEditSeason(string id, DateTime init, DateTime end, bool current)
        {
            var element = await _repo.GetSeason(id);

            return await OperationHelper.EditElement(_commonDb, _repo.GetSeasons(),
                id,
                element,
                s => {
                    s.Start = init;
                    s.End = end;
                    s.Current = current;
                    return s;
                },
                _repo.CreateUpdateSeason,
                 $"No existe temporada con id : {id}",
                s => (init.CompareTo(s.Start) >= 0 && init.CompareTo(s.End) <= 0) || (end.CompareTo(s.Start) >= 0 && end.CompareTo(s.End) <= 0),
                $"No se puede sobreponer fecha"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewSeason(DateTime init, DateTime end) {
            return await OperationHelper.CreateElement(_commonDb, _repo.GetSeasons(),
                async s => await _repo.CreateUpdateSeason(new Season
                {
                    Id = s,
                    Start = init,
                    End = end,
                    Current = true
                }),
                s => (init.CompareTo(s.Start) >= 0 && init.CompareTo(s.End) <= 0) || (end.CompareTo(s.Start) >= 0 && end.CompareTo(s.End) <= 0),
                $"No se puede sobreponer fecha"
            );
        }
    }
}
