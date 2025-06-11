namespace Trabalho1.Models
{
    //  para receber os dados do frontend ao registrar um ticket
    public class RegistrarTicketRequest
    {
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int TipoVeiculoId { get; set; }
    }
}
