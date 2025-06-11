import { useEffect, useState } from 'react';
import axios from 'axios';
import { Ticket } from '../models/Ticket';
import { Veiculo } from '../models/Veiculo'; // Importe o modelo Veiculo para tipagem correta
import './historico-tickets-modulo.css';

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api";

// Interface para os dados brutos como vêm do backend (agora sabemos que é camelCase por padrão do C#)
interface RawTicketBackend {
    id: number;
    veiculoId: number;
    vagaId: number | null;
    entrada: string;       // Propriedade em camelCase no backend (confirmado pelo JSON)
    saida?: string | null; // Propriedade em camelCase no backend
    valorTotal: number | null; // Propriedade em camelCase no backend
    pago: boolean;         // Propriedade em camelCase no backend
    veiculo?: { // Tipagem para o objeto Veiculo vindo do backend (com placa)
        id: number;
        placa: string;
        modelo: string;
        tipoVeiculoId: number;
        tipoVeiculo?: any;
    };
    vaga?: { // Tipagem para o objeto Vaga vindo do backend
        id: number;
        numero: number;
        // Adicione outras propriedades da Vaga se precisar
    };
}

/**
 * Função auxiliar para transformar os dados do ticket do formato do backend (camelCase)
 * para o formato esperado pelo modelo Ticket.ts no frontend (camelCase, com 'dataEntrada', etc.).
 * Principalmente para mapear 'entrada' para 'dataEntrada'.
 * @param rawTicket O objeto de ticket recebido diretamente do backend.
 * @returns Um objeto Ticket formatado para o frontend.
 */
const transformTicketData = (rawTicket: RawTicketBackend): Ticket => {
    return {
        id: rawTicket.id,
        veiculoId: rawTicket.veiculoId,
        vagaId: rawTicket.vagaId,
        dataEntrada: rawTicket.entrada, // <--- CORREÇÃO AQUI: Mapeia 'entrada' para 'dataEntrada'
        dataSaida: rawTicket.saida,     // Mapeia 'saida' para 'dataSaida'
        valorTotal: rawTicket.valorTotal !== null ? rawTicket.valorTotal : 0,
        pago: rawTicket.pago,
        veiculo: rawTicket.veiculo ? {
            id: rawTicket.veiculo.id,
            placa: rawTicket.veiculo.placa,
            modelo: rawTicket.veiculo.modelo,
            tipoVeiculoId: rawTicket.veiculo.tipoVeiculoId,
            // Adicione outras propriedades do veículo se o modelo Veiculo.ts as tiver
        } as Veiculo : undefined,
    };
};

function HistoricoTickets() {
    const [tickets, setTickets] = useState<Ticket[]>([]);
    const [erro, setErro] = useState<string>('');
    const [carregando, setCarregando] = useState<boolean>(true);

    useEffect(() => {
        setCarregando(true);
        setErro('');
        axios.get<RawTicketBackend[]>(`${API_BASE_URL}/Ticket`)
            .then(response => {
                const transformedTickets = response.data.map(transformTicketData);
                setTickets(transformedTickets);
            })
            .catch(error => {
                console.error("Erro ao carregar tickets:", error);
                setErro("Erro ao carregar tickets. Verifique sua conexão ou tente novamente.");
            })
            .finally(() => setCarregando(false));
    }, []);

    const formatarData = (dataString: string | null | undefined): string => {
        if (!dataString) return '';
        const data = new Date(dataString);
        if (isNaN(data.getTime())) return 'Data Inválida';
        return data.toLocaleDateString('pt-BR') + ' ' + data.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
    };

    const formatarValor = (valor: number | undefined): string => {
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
