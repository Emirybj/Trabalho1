import { TipoVeiculo } from './TipoVeiculo';

export interface Vaga {
    id: number;
    numero: number;
    ocupada: boolean;
    veiculoId?: number | null; 
    
    tipoVeiculoId: number; 
    tipo?: TipoVeiculo; 

    andar?: string | null; 
    setor?: string | null; 
    
}
