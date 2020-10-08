﻿using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    public class PreOrdersOperations<T> : MainOperation<PreOrder, PreOrderInput,T>, IGenericOperation<PreOrder, PreOrderInput> {
        private readonly ICommonQueries commonQueries;

        public PreOrdersOperations(IMainGenericDb<PreOrder> repo,  IAgroSearch<T> search, ICommonQueries commonQueries, ICommonDbOperations<PreOrder> commonDb, IValidatorAttributes<PreOrderInput, PreOrder> validator) : base(repo, search, commonDb, validator) {
            this.commonQueries = commonQueries;
        }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        //if (!preOrder.BarracksId.Any()) return "Los cuarteles son obligatorios";

        public async Task<ExtPostContainer<string>> Save(PreOrder preOrder) {
            await repo.CreateUpdate(preOrder);
            search.AddDocument(preOrder);

            return new ExtPostContainer<string> {
                IdRelated = preOrder.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(PreOrderInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var preOrder = new PreOrder {
                Id = id,
                IdIngredient = input.IdIngredient,
                BarracksId = input.BarracksId,
                PreOrderType = input.PreOrderType,
                Name = input.Name,
                OrderFolderId = input.OrderFolderId
            };
            if (!isBatch)
                return await Save(preOrder);
            await repo.CreateEntityContainer(preOrder);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}