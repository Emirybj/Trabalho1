export interface Ticket {// Define a estrutura para um ticket de estacionamento.
  id: number; // Identificador do ticket.
  veiculoId: number; // ID do veículo.
  vagaId: number;   // ID da vaga.
  dataEntrada: string; // Momento da entrada.
  dataSaida?: string | null; // Momento da saída (opcional).
  valorTotal?: number | null; // Valor final (opcional).
}