import { useEffect, useState } from 'react';//Executa um código quando o componente é montado
import axios from 'axios';//Biblioteca para fazer requisições HTTP (GET, POST, etc).
import { Ticket } from '../models/Ticket'; 
import { Veiculo } from '../models/Veiculo'; 
import './historico-tickets-modulo.css';

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api";//define a URL base da API, primeiro .env ou o localhost

/**
 * @interface RawTicketBackend
 * @description Define como os dados de ticket chegam **diretamente do backend**.
 * É diferente do modelo `Ticket` do frontend porque o backend pode ter nomes de campos ou estruturas ligeiramente diferentes.
 */
interface RawTicketBackend {//Interface auxiliar, como os dados vêm do backend.
    id: number;
    veiculoId: number;
    vagaId: number | null;
    entrada: string; // Data de entrada vinda do backend
    saida?: string | null; // Data de saída vinda do backend
    valorTotal: number | null;
    pago: boolean;
    /**
     * @property veiculo - Detalhes do veículo aninhado, conforme retornado pelo backend.
     */
    veiculo?: {
        id: number;
        placa: string;
        modelo: string;
        tipoVeiculoId: number;
        tipoVeiculo?: any;
    };
    /**
     * @property vaga - Detalhes da vaga aninhada, conforme retornado pelo backend.
     */
    vaga?: {
        id: number;
        numero: number;
        // Adicione outras propriedades da Vaga se precisar
    };
}

/**
 * Função auxiliar para converter o formato do ticket do backend (`RawTicketBackend`)
 * para o formato do nosso frontend (`Ticket`).
 * @param rawTicket O objeto de ticket recebido do backend.
 * @returns Um objeto Ticket formatado.
 */
const transformTicketData = (rawTicket: RawTicketBackend): Ticket => {
    return {
        id: rawTicket.id,
        veiculoId: rawTicket.veiculoId,
        vagaId: rawTicket.vagaId,
        dataEntrada: rawTicket.entrada, // Mapeia 'entrada' para 'dataEntrada'
        dataSaida: rawTicket.saida, // Mapeia 'saida' para 'dataSaida'
        valorTotal: rawTicket.valorTotal !== null ? rawTicket.valorTotal : 0,
        pago: rawTicket.pago,
        veiculo: rawTicket.veiculo ? {
            id: rawTicket.veiculo.id,
            placa: rawTicket.veiculo.placa,
            modelo: rawTicket.veiculo.modelo,
            tipoVeiculoId: rawTicket.veiculo.tipoVeiculoId,
        } as Veiculo : undefined,
    };
};

function HistoricoTickets() {//Cria 3 estados
    const [tickets, setTickets] = useState<Ticket[]>([]);
    const [erro, setErro] = useState<string>('');
    const [carregando, setCarregando] = useState<boolean>(true);

    useEffect(() => {
        setCarregando(true);
        setErro('');
        axios.get<RawTicketBackend[]>(`${API_BASE_URL}/Ticket`) // Busca dados do backend usando o tipo RawTicketBackend
            .then(response => {
                // Transforma os dados recebidos para o formato de Ticket do frontend
                const transformedTickets = response.data.map(transformTicketData);
                setTickets(transformedTickets);
            })
            .catch(error => {
                console.error("Erro ao carregar tickets:", error);
                setErro("Erro ao carregar tickets. Verifique sua conexão ou tente novamente.");
            })
            .finally(() => setCarregando(false));
    }, []);//garante que execute só uma vez.

    const formatarData = (dataString: string | null | undefined): string => {//Converte string de data para formato legível brasileiro
        if (!dataString) return '';
        const data = new Date(dataString);
        if (isNaN(data.getTime())) return 'Data Inválida';
        return data.toLocaleDateString('pt-BR') + ' ' + data.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
    };

    const formatarValor = (valor: number | undefined): string => {//Formata um número para valor monetário brasileiro
        return valor !== undefined && valor !== null ?
            `R$ ${valor.toFixed(2).replace('.', ',')}` :
            "-";
    };

    return (
        <div className="historico-tickets-container">
            <h1 className="page-title">Histórico de Tickets</h1>

            {carregando && <p className="loading-message">Carregando histórico de tickets...</p>} 
            {erro && <p className="error-message">{erro}</p>} 

            {!carregando && tickets.length === 0 && !erro && (
                <p className="no-records-message">Nenhum ticket encontrado no histórico.</p>
            )}

            {!carregando && tickets.length > 0 && (
                <div className="table-responsive">
                    <table className="tickets-table">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Placa do Veículo</th>
                                <th>ID Vaga</th>
                                <th>Entrada</th>
                                <th>Saída</th>
                                <th>Valor Total</th>
                                <th>Pago</th>
                            </tr>
                        </thead>
                        <tbody>
                            {tickets.map(ticket => (
                                <tr key={ticket.id}>
                                    <td data-label="ID">{ticket.id}</td>
                                    <td data-label="Placa do Veículo">{ticket.veiculo?.placa || ticket.veiculoId || "N/A"}</td>
                                    <td data-label="ID Vaga">{ticket.vagaId || "N/A"}</td>
                                    <td data-label="Entrada">{formatarData(ticket.dataEntrada)}</td>
                                    <td data-label="Saída">{ticket.dataSaida ? formatarData(ticket.dataSaida) : "Em aberto"}</td>
                                    <td data-label="Valor Total">{formatarValor(ticket.valorTotal)}</td>
                                    <td data-label="Pago">{ticket.pago ? "Sim" : "Não"}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}

export default HistoricoTickets;
