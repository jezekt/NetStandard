using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Services.DataProviders;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.DataProviders
{
    public class AssociationClassEntityProvider<TEntity, FirstId, SecondId> : IProvideAssociationClassItemByIds<TEntity, FirstId, SecondId>
        where TEntity : class, IAssociationClass<FirstId, SecondId>
    {
        private readonly Data.DataProviders.IProvideAssociationClassItemByIds<TEntity, FirstId, SecondId> _entityProvider;


        public async Task<TEntity> GetByIdsAsync(FirstId firstObjId, SecondId secondObjId)
        {
            return await _entityProvider.GetByIdsAsync(firstObjId, secondObjId);
        }


        public AssociationClassEntityProvider(Data.DataProviders.IProvideAssociationClassItemByIds<TEntity, FirstId, SecondId> entityProvider)
        {
            if (entityProvider == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _entityProvider = entityProvider;
        }
    }
}
