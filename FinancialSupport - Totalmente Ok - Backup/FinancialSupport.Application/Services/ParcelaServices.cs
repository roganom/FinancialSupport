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
        private IEmprestimoService _emprestimoService;
        private readonly IMapper _mapper;
        public ParcelaService(IParcelaRepository parcelaRepository, IMapper mapper, IEmprestimoService emprestimoService)
        {
            _parcelaRepository = parcelaRepository ?? throw new ArgumentNullException(nameof(parcelaRepository));
            _mapper = mapper;
            _emprestimoService = emprestimoService;
        }
        #region GETs: GetParcelas, GetById e GetByIdEmprestimo
        public async Task<List<ParcelaDTO>> GetParcelas()
        {
            var parcelaEntity = await _parcelaRepository.GetParcelas();
            return _mapper.Map<List<ParcelaDTO>>(parcelaEntity);
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
        public async Task<List<ParcelaDTO>> GetByIdEmprestimo(int? id)
        {
            var parcelaEntity = await _parcelaRepository.GetParcelaByIdAsync(id);         ///_EmprestimoContext.Emprestimos.Include(c => c.IdUsuario).Where(p => p.Ativo).ToListAsync();
            return _mapper.Map<List<ParcelaDTO>>(parcelaEntity);
        }
        #endregion

        #region Add, Update, Update2 e Remove
        public async Task Add(ParcelaDTO parcelaDto)
        {
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            parcelaDto.DataCriacao = DateTime.Now;
            parcelaEntity.DataPagamento = parcelaEntity.DataPagamento < DateTime.MinValue ? (DateTime?)null : parcelaEntity.DataPagamento;
            await _parcelaRepository.CreateAsync(parcelaEntity);
        }
        public async Task Update(ParcelaDTO parcelaDto)
        {
            parcelaDto.DataAlteracao = DateTime.Now;
            parcelaDto.Valendo = true;
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            await _parcelaRepository.UpdateAsync(parcelaEntity);
        }
        public async Task Update2(ParcelaDTO parcelaDto)
        {
            parcelaDto.DataAlteracao = DateTime.Now;
            parcelaDto.Valendo = true;
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            await _parcelaRepository.Update2Async(parcelaEntity);
        }
        public async Task Remove(ParcelaDTO parcelaDto)
        {
            parcelaDto.DataAlteracao = DateTime.Now;
            parcelaDto.Valendo = false;
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            await _parcelaRepository.Update2Async(parcelaEntity);

            //var parcelaEntity = _parcelaRepository.GetParcelaByIdAsync(id).Result;
            //await _parcelaRepository.RemoveAsync(parcelaEntity);
        }
        #endregion

        #region Altera e Deleta apenas na adminitração, pois é necessário recalcular o valor do limite
        public async Task UpdateAdm(ParcelaDTO parcelaDto)
        {
            parcelaDto.DataAlteracao = DateTime.Now;
            parcelaDto.Valendo = true;
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            await _parcelaRepository.UpdateAsync(parcelaEntity);
            await RecalculaParcela(parcelaDto.IdEmprestimo);
        }
        public async Task Update2Adm(ParcelaDTO parcelaDto)
        {
            parcelaDto.DataAlteracao = DateTime.Now;
            parcelaDto.Valendo = true;
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            await _parcelaRepository.Update2Async(parcelaEntity);
            await RecalculaParcela(parcelaDto.IdEmprestimo);
        }
        public async Task RemoveAdm(ParcelaDTO parcelaDto)
        {
            parcelaDto.DataAlteracao = DateTime.Now;
            parcelaDto.Valendo = false;
            var parcelaEntity = _mapper.Map<Parcela>(parcelaDto);
            await _parcelaRepository.Update2Async(parcelaEntity);
            await RecalculaParcela(parcelaDto.IdEmprestimo);

            //var parcelaEntity = _parcelaRepository.GetParcelaByIdAsync(id).Result;
            //await _parcelaRepository.RemoveAsync(parcelaEntity);
        }
        #endregion
        public async Task RecalculaParcela(int? emprestimo)
        {
            // caso teha alguma deleção ou alteração em alguma parcela, chama RecalculaEmprestimo(int emprestimo)
            await _emprestimoService.RecalculaEmprestimo(emprestimo);
        }
    }
}
