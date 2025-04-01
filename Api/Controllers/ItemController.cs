using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.Item;
using Api.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Params;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/item")]
    public class ItemController(IGenericRepository<ItemEntity> itemRepository, IMapper mapper) : ControllerBase
    {
        private readonly IGenericRepository<ItemEntity> _itemRepository = itemRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<Pagination<ItemToReturnDto>>> GetItems([FromQuery] EntitySpecParams itemSpecParams)
        {
            var spec = new ItemsWithFiltersSpecification(itemSpecParams);
            var items = await _itemRepository.GetEntitiesWithSpecAsync(spec);
            int count = await _itemRepository.CountAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ItemToReturnDto>>(items);
            return Ok(new Pagination<ItemToReturnDto>(itemSpecParams.PageNumber, itemSpecParams.PageSize, count, data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var entity = await _itemRepository.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ItemToReturnDto>(entity));
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemDto itemDto)
        {
            var item = _mapper.Map<ItemEntity>(itemDto);
            await _itemRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, _mapper.Map<ItemToReturnDto>(item));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _itemRepository.DeleteAsync(id);
            if (item == null) return NotFound();
            return Ok(_mapper.Map<ItemToReturnDto>(item));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] UpdateItemDto itemDto)
        {
            var item = await _itemRepository.UpdateAsync(id, itemDto);
            if (item == null) return NotFound();
            return Ok(_mapper.Map<ItemToReturnDto>(item));
        }
    }
}