﻿using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.fields
{
    public class BarrackOperations : IBarrackOperations
    {
        private readonly IPlotLandRepository _repoPlotLand;
        private readonly string _idSeason;
        private readonly IBarrackRepository _repo;
        private readonly IVarietyRepository _repoVariety;

        public BarrackOperations(IBarrackRepository repo, IVarietyRepository repoVariety,IPlotLandRepository repoPlotLand, string idSeason)
        {
            _repo = repo;
            _repoPlotLand = repoPlotLand;
            _idSeason = idSeason;
            _repoVariety = repoVariety;
        }

        public async Task<ExtGetContainer<Barrack>> GetBarrack(string id)
        {
            var barrack = await _repo.GetBarrack(id);
            return OperationHelper.GetElement(barrack);
        }

        public async Task<ExtGetContainer<List<Barrack>>> GetBarracks()
        {
            var barracks = await _repo.GetBarracks().ToListAsync();
            return OperationHelper.GetElements(barracks);
        }

        public async Task<ExtPostContainer<Barrack>> SaveEditBarrack(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator)
        {
            var elements = await GetElementToBarracks(idPlotLand, idVariety, idPollinator);

            if (!elements.Success) return OperationHelper.PostNotFoundElementException<Barrack>(elements.Message, elements.IdNotfound);



            var element = await _repo.GetBarrack(id);

            return await OperationHelper.EditElement(id,
                element,
                s => {
                    s.Name = name;
                    s.SeasonId = _idSeason;
                    s.PlotLand = elements.PlotLand;
                    s.NumberOfPlants = numberOfPlants;
                    s.PlantingYear = plantingYear;
                    s.Pollinator = elements.Pollinator;
                    s.Variety = elements.Variety;
                    s.Hectares = hectares;
                    
                    return s;
                },
                _repo.CreateUpdateBarrack,
                 $"No existe Parcela con id : {id}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewBarrack(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator)
        {
            var elements = await GetElementToBarracks(idPlotLand, idVariety, idPollinator);

            if (!elements.Success) return OperationHelper.PostNotFoundElementException<string>(elements.Message, elements.IdNotfound);

            return await OperationHelper.CreateElement(_repo.GetBarracks(),
                async s => await _repo.CreateUpdateBarrack(new Barrack
                {
                    Id = s,
                    Name = name,
                    SeasonId = _idSeason,
                    PlotLand = elements.PlotLand,
                    Hectares = hectares,
                    NumberOfPlants = numberOfPlants,
                    PlantingYear = plantingYear,
                    Pollinator = elements.Pollinator,
                    Variety = elements.Variety
                }),
                s => s.Name.Equals(name),
                $"ya existe Cuartel con nombre {name} "

            );

        }

        private async Task<ElementsBarracks> GetElementToBarracks(string idPlotLand, string idVariety, string idVarietyPollinator) {
            var elementBarrack = new ElementsBarracks();
            elementBarrack.PlotLand = await _repoPlotLand.GetPlotLand(idPlotLand);
            if (elementBarrack.PlotLand == null)
            {
                elementBarrack.Message = $"no existe parcela con id {idPlotLand}";
                elementBarrack.IdNotfound = idPlotLand;
                elementBarrack.Success = false;
            }
            elementBarrack.Variety = await _repoVariety.GetVariety(idVariety);
            if (elementBarrack.Variety == null)
            {
                elementBarrack.IdNotfound = idVariety;
                elementBarrack.Message = $"no existe variedad con id {idVariety}";
                elementBarrack.Success = false;
            }
            if (!string.IsNullOrWhiteSpace(idVarietyPollinator))
            {
                elementBarrack.Pollinator = await _repoVariety.GetVariety(idVarietyPollinator);
            }
            elementBarrack.Success = true;
            return elementBarrack;
        }

    }

    public class ElementsBarracks {
        public bool Success { get; set; }

        public Variety Variety { get; set; }

        public Variety Pollinator { get; set; }

        public PlotLand PlotLand { get; set; }

        public string Message { get; set; }

        public string IdNotfound { get; set; }





    }
}
