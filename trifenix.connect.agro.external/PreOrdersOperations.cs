﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.queries;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;

namespace trifenix.connect.agro.external
{
    public class PreOrdersOperations<T> : MainOperation<PreOrder, PreOrderInput,T>, IGenericOperation<PreOrder, PreOrderInput> {
        private readonly ICommonAgroQueries Queries;

        public PreOrdersOperations(IDbExistsElements existsElement, IMainGenericDb<PreOrder> repo, IAgroSearch<T> search, ICommonDbOperations<PreOrder> commonDb, ICommonAgroQueries queries, IValidatorAttributes<PreOrderInput> validator) : base(repo, search, commonDb, validator) { 
            Queries = queries;
        }
    
        public async override Task Validate(PreOrderInput input)
        {
            var OFIngredient = await Queries.GetOrderFolderIngredientFromPreOrder(input.OrderFolderId);

            if (input.IngredientId == OFIngredient)
            {
                var POIngredients = await Queries.GetPreOrderIngredientFromOrderFolder(input.OrderFolderId);
                foreach (var item in POIngredients)
                {
                    if (input.IngredientId == item)
                    {
                        throw new Exception("El ingrediente de la carpeta de ordenes ya se encuentra en uso");
                    }
                }
            }
            bool isUnique = input.BarrackIds.Distinct().Count() == input.BarrackIds.Count();
            if (!isUnique)
            {
                throw new Exception("No se pueden ingresar barracks duplicados");
            }

            if (!input.BarrackIds.Any())
            {
                throw new Exception("No se puede ingresar una pre orden sin un barrack asociado");
            }

            if (!Enum.IsDefined(typeof(PreOrderType), input.PreOrderType))
                throw new ArgumentOutOfRangeException();

            await base.Validate(input);
        }

        public override async Task<ExtPostContainer<string>> SaveInput(PreOrderInput preOrderInput)
        {
            var id = !string.IsNullOrWhiteSpace(preOrderInput.Id) ? preOrderInput.Id : Guid.NewGuid().ToString("N");

            /// Valida cada pre orden
            await Validate(preOrderInput);

            var preOrder = new PreOrder
            {
                Id = id,
                Name = preOrderInput.Name,
                OrderFolderId = preOrderInput.OrderFolderId,
                PreOrderType = preOrderInput.PreOrderType,
                Ingredient = preOrderInput.IngredientId,
                BarrackIds = preOrderInput.BarrackIds
            };

            await SaveDb(preOrder);
            var result = await SaveSearch(preOrder);
            return result;
        }   
 
    }

}