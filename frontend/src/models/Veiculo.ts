import { TipoVeiculo } from "./TipoVeiculo";

export interface Veiculo
{
    id: string;
    nome: string;
    descricao: string;
    preco: number;
    criadoEm: Date;
    categoria: TipoVeiculo;
    categoriaId: number;
}