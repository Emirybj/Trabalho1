import { TipoVeiculo } from './TipoVeiculo';

export interface Vaga {
    id: number; // Identificador do ticket.
    numero: number; // Número da vaga
    ocupada: boolean; //Identifica se a vaga esta ocupada
    veiculoId?: number | null; //Verifica o ID do veiculo
    tipoVeiculoId: number; // verifica o ID do tipoveiculo
    tipo?: TipoVeiculo; // indica que a vaga pode conter um objeto TipoVeiculo
}
