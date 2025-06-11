import { Veiculo } from './Veiculo'; 

export interface Ticket {
    id: number; // Identificador do ticket.
    veiculoId: number; // ID do veículo.
    vagaId: number | null;   // ID da vaga.
    dataEntrada: string; // Momento da entrada.
    dataSaida?: string | null; // Momento da saída (opcional).
    valorTotal: number; // <<-- REMOVA O '?' AQUI. ELE DEVE SER SEMPRE UM NÚMERO (inicialmente 0).
    pago: boolean;

    veiculo?: Veiculo;
}