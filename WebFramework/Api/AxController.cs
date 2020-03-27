using AutoMapper.QueryableExtensions;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities.Framework;

namespace WebFramework.Api
{
    public class AxController<TDto, TSelectDto, TEntity, TKey> : BaseController
        where TDto : BaseDto<TDto, TEntity, TKey>, new()
        where TSelectDto : BaseDto<TSelectDto, TEntity, TKey>, new()
        where TEntity : BaseEntity<TKey>, new()
    {
        private readonly IBaseRepository<TEntity> _repository;

        public AxController(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }

        //[HttpGet]
        //public virtual async Task<ActionResult<List<TSelectDto>>> Get(CancellationToken cancellationToken)
        //{
        //    var list = await _repository.TableNoTracking.ProjectTo<TSelectDto>().ToListAsync(cancellationToken);
        //    return Ok(list);
        //}
        [HttpGet]
        public virtual ApiResult<IQueryable<TSelectDto>> Get()
        {
            var list = _repository.GetAll().ProjectTo<TSelectDto>();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<TSelectDto>> Get(TKey id, CancellationToken cancellationToken)
        {
            var dto = await _repository.TableNoTracking.ProjectTo<TSelectDto>().SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
            if (dto == null)
                return NotFound();
            return dto;
        }

        [HttpPost]
        public virtual async Task<ApiResult<TSelectDto>> Create(TDto dto, CancellationToken cancellationToken)
        {
            var model = dto.ToEntity();

            await _repository.AddAsync(model, cancellationToken);

            var resultDto = await _repository.TableNoTracking.ProjectTo<TSelectDto>().SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

            return resultDto;
        }

        [HttpPut]
        public virtual async Task<ApiResult<TSelectDto>> Update(TDto dto, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstAsync(x => x.Id.Equals(dto.Id), cancellationToken);

            model = dto.ToEntity(model);

            await _repository.UpdateAsync(model, cancellationToken);

            var resultDto = await _repository.TableNoTracking.ProjectTo<TSelectDto>().SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

            return resultDto;
        }

        [HttpDelete("{id:guid}")]
        public virtual async Task<ApiResult> Delete(TKey id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstAsync(x => x.Id.Equals(id), cancellationToken);
            await _repository.DeleteAsync(model, cancellationToken);

            return Ok();
        }
    }

    public class AxController<TDto, TSelectDto, TEntity> : AxController<TDto, TSelectDto, TEntity, int>
        where TDto : BaseDto<TDto, TEntity, int>, new()
        where TSelectDto : BaseDto<TSelectDto, TEntity, int>, new()
        where TEntity : BaseEntity<int>, new()
    {
        public AxController(IBaseRepository<TEntity> repository) : base(repository)
        {
        }
    }

    public class AxController<TDto, TEntity> : AxController<TDto, TDto, TEntity, int>
        where TDto : BaseDto<TDto, TEntity, int>, new()
        where TEntity : BaseEntity<int>, new()
    {
        public AxController(IBaseRepository<TEntity> repository) : base(repository)
        {
        }
    }
}
