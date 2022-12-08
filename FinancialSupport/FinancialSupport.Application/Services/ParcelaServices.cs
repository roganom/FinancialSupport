using AutoMapper;
using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using FinancialSupport.Domain.Entities;
using FinancialSupport.Domain.Interfaces;

namespace FinancialSupport.Application.Services
{
    public class ParcelaService : IParcelaService
    {
        private IParcelaRepository _parcelaRepository;
        private readonly IMapper _mapper;
        public ParcelaService(IParcelaRepository parcelaRepository, IMapper mapper)
        {
            _parcelaRepository = parcelaRepository ?? throw new ArgumentNullException(nameof(parcelaRepository));
            _mapper = mapper;
        }
        public async Task<List<ParcelaDTO>> GetParcelas()
        {
            var parcelaEntity = await _parcelaRepository.GetParcelas();
            return _mapper.Map<List<ParcelaDTO>>(parcelaEntity);
        }
        public async Task Add(ParcelaDTO parcelaDto)
        {
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);

            parcelaEntity.DataPagamento = parcelaEntity.DataPagamento < DateTime.MinValue ? (DateTime?)null : parcelaEntity.DataPagamento;
            await _parcelaRepository.CreateAsync(parcelaEntity);
        }
        public async Task<ParcelaDTO> GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                var parcelaEntity = await _parcelaRepository.GetParcelaByIdAsync(id);

                if (parcelaEntity == null)
                    return null; 
                else
                    return _mapper.Map<ParcelaDTO>(parcelaEntity);
            }
        }
        public async Task<ParcelaDTO> GetByIdEmprestimo(int? id)
        {
            var parcelaEntity = await _parcelaRepository.GetParcelaByIdAsync(id);         ///_EmprestimoContext.Emprestimos.Include(c => c.IdUsuario).Where(p => p.Ativo).ToListAsync();
            return _mapper.Map<ParcelaDTO>(parcelaEntity);
        }
        public async Task Update(ParcelaDTO parcelaDto)
        {
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            await _parcelaRepository.UpdateAsync(parcelaEntity);
        }
        public async Task Update2(ParcelaDTO parcelaDto)
        {
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            await _parcelaRepository.Update2Async(parcelaEntity);
        }
        public async Task Remove(int? id)
        {
            var parcelaEntity = _parcelaRepository.GetParcelaByIdAsync(id).Result;
            await _parcelaRepository.RemoveAsync(parcelaEntity);
        }
    }
}
