import { TipoVeiculo } from './TipoVeiculo';

export interface Veiculo {
    id: number;
    placa: string; // Adicionado: A placa do veículo
    modelo: string;
    
    tipoVeiculoId: number; // Chave estrangeira para o tipo de veículo
    tipoVeiculo?: TipoVeiculo; // Propriedade de navegação
}
