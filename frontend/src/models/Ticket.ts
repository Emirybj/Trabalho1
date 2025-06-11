import { Veiculo } from './Veiculo'; 

export interface Ticket {
    id: number; // Identificador do ticket.
    veiculoId: number; // ID do veículo.
    vagaId: number | null;   // ID da vaga.
    dataEntrada: string; // Momento da entrada.
    dataSaida?: string | null; // Momento da saída
    valorTotal: number; // O valor total a ser pago ou já pago por este ticket de estacionamento.
    pago: boolean; // Indica se o ticket já foi pago (true) ou ainda não (false).
    veiculo?: Veiculo;// Objeto completo do veículo associado a este ticket.
}