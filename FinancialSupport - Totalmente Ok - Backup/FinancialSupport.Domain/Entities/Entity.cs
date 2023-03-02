namespace FinancialSupport.Domain.Entities
{
    public abstract class Entity
    {
        public int? Id { get; protected set; }
        public DateTime? DataCriacao { get; set; }
        public string? UsuarioCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioAlteracao { get; set; }
        public Boolean? Valendo { get; set; }
    }
}
