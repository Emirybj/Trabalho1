import { useState } from 'react';
import axios from 'axios';
import './retirar-veiculo-modulo.css'; 

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api";

function RetirarVeiculo() {
    const [placa, setPlaca] = useState('');
    const [erro, setErro] = useState<string>('');
    const [sucesso, setSucesso] = useState<string>('');

    const retirarVeiculo = async (e: React.FormEvent) => { // async para usar await
        e.preventDefault();
        setErro('');
        setSucesso('');

        if (!placa.trim()) {
            setErro("A placa é obrigatória.");
            return;
        }

        console.log("Tentando retirar veículo com placa:", placa); // O que está sendo enviado
        
        try {
            // A rota para retirar é POST /api/Ticket/retirar
            const response = await axios.post(`${API_BASE_URL}/Ticket/retirar`, { placa: placa });

            setSucesso(`Veículo retirado com sucesso. Valor: R$ ${response.data.valorTotal.toFixed(2)}`);
            setPlaca(''); // Limpa o campo após o sucesso
            console.log("Retirada bem-sucedida:", response.data); // Resposta de sucesso

        } catch (error: any) {
            console.error("Erro ao retirar o veículo (detalhes):", error); //  O erro completo
            
            // Tenta tirar uma mensagem de erro mais útil da resposta do backend
            if (error.response) {
                // Erro veio do servidor 
                console.error("Dados da resposta de erro (response.data):", error.response.data);
                console.error("Status da resposta de erro (response.status):", error.response.status);
                
                if (typeof error.response.data === 'string') {
                    setErro(`Erro ao retirar o veículo: ${error.response.data}`);
                } else if (error.response.data.errors) {
                    // Se o backend enviar um objeto de erros de validação
                    const validationErrors = Object.values(error.response.data.errors).flat();
                    setErro(`Erro de validação: ${validationErrors.join(' ')}`);
                } else if (error.response.data.title) {
                    // Erro padrão de BadRequest do ASP.NET Core
                    setErro(`Erro ao retirar o veículo: ${error.response.data.title} - ${error.response.data.detail || ''}`);
                } else {
                    setErro(`Erro ao retirar o veículo: Status ${error.response.status}. Verifique os dados e tente novamente.`);
                }
            } else if (error.request) {
                // Requisição foi feita mas não houve resposta (ex: rede offline, CORS bloqueando antes da resposta do servidor)
                setErro("Erro de rede: Não foi possível conectar ao servidor. Verifique sua conexão ou o status do backend.");
                console.error("Não houve resposta do servidor:", error.request);
            } else {
                // Algo aconteceu na configuração da requisição que disparou um erro
                setErro("Erro na configuração da requisição. Tente novamente.");
                console.error("Erro de configuração Axios:", error.message);
            }
        }
    };

    return (
        <div className="form-page-container">
            <h1 className="form-page-title">Retirar Veículo</h1>
            {erro && <p className="error-message">{erro}</p>}
            {sucesso && <p className="success-message">{sucesso}</p>}
            <form onSubmit={retirarVeiculo} className="standard-form">
                <div className="form-group">
                    <label htmlFor="placa-retirada">Placa:</label>
                    <input 
                        id="placa-retirada" 
                        value={placa} 
                        onChange={e => setPlaca(e.target.value)} 
                        required 
                        placeholder="Ex: ABC-1234"
                    />
                </div>
                <div className="form-actions">
                    <button type="submit" className="button submit-button">
                        Retirar
                    </button>
                </div>
            </form>
        </div>
    );
}

export default RetirarVeiculo;