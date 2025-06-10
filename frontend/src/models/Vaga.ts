import { TipoVeiculo } from "./TipoVeiculo";


export interface vaga{
    id:string;
    numero:number;
    tipo:TipoVeiculo;
    disponivel:boolean;
    ocupadaporveiculoId: string | null;
    andar?: string;
    setor?: string;

}